using AutoMapper;
using Dreamscape.Application.Common.Exceptions;
using Dreamscape.Application.Repositories;
using Dreamscape.Domain.Entities;
using MediatR;
using Dreamscape.Application.Common.Helpers;
using Dreamscape.Application.Services;

namespace Dreamscape.Application.Collections.Commands.AppendFileToCollection
{
    public class AppendFileToCollectionCommandHandler(
        IUserRepository userRepository,
        IFileRepository fileRepository,
        ITagRepository tagRepository,
        ICollectionRepository collectionRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IModelPredictionService modelPredictionService)
        : IRequestHandler<AppendFileToCollectionCommand, CollectionViewModel>
    {
        readonly IUserRepository _userRepository = userRepository;
        readonly IFileRepository _fileRepository = fileRepository;
        readonly ITagRepository _tagRepository = tagRepository;
        readonly ICollectionRepository _collectionRepository = collectionRepository;
        readonly IUnitOfWork _unitOfWork = unitOfWork;
        readonly IMapper _mapper = mapper;
        readonly IModelPredictionService _modelPredictionService = modelPredictionService;

        public async Task<CollectionViewModel> Handle(AppendFileToCollectionCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetAsync(
              [u => u.Id == request.UserId],
              [u => u.Collections],
              cancellationToken
            ) ?? throw new NotFoundException(nameof(User), request.UserId);

            var collection = await _collectionRepository.GetAsync(
              [c => c.Id.ToString() == request.CollectionId],
              [
                  c => c.Owner,
                  c => c.Files,
                  c => c.Tags
              ],
              cancellationToken
            ) ?? throw new NotFoundException(nameof(Collection), request.CollectionId);

            if (collection.OwnerId != request.UserId)
            {
                throw new ForbiddenException();
            }

            var file = await _fileRepository.GetAsync(
              [c => c.Id.ToString() == request.FileId],
              null,
              cancellationToken
            ) ?? throw new NotFoundException(nameof(ImageFile), request.FileId);

            collection.Files.Add(file);

            var collectionVectors = collection.Files
                .Select(file => file.Vector)
                .Where(vector => vector != null)
                .ToList();

            if (collectionVectors != null && collectionVectors.Count > 0)
            {
                collection.Vector = VectorHelper.ComputeAverageVector(collectionVectors!);

                var predictions = _modelPredictionService.ConvertVectorToPredictions(collection.Vector.ToArray());

                collection.Tags.Clear();

                foreach (var prediction in predictions.Where(prediction => prediction.Confidence > 1))
                {
                    var tag = await _tagRepository.GetAsync([tag => tag.Name == prediction.Label], null, cancellationToken) ??
                        _tagRepository.Create(new Tag()
                        {
                            Name = prediction.Label
                        });

                    collection.Tags.Add(tag);
                }
            }

            _collectionRepository.Update(collection);

            await _unitOfWork.Save(cancellationToken);

            return _mapper.Map<CollectionViewModel>(collection);
        }
    }
}
