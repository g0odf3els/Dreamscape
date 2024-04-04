using AutoMapper;
using Dreamscape.Application.Common.Exceptions;
using Dreamscape.Application.Common.Helpers;
using Dreamscape.Application.Repositories;
using Dreamscape.Application.Services;
using Dreamscape.Domain.Entities;
using MediatR;

namespace Dreamscape.Application.Collections.Commands.RemoveFileFromCollection
{
    public class RemoveFileFromCollectionCommandHandler(
        IUserRepository userRepository,
        IFileRepository fileRepository,
        ITagRepository tagRepository,
        ICollectionRepository collectionRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IModelPredictionService modelPredictionService)
        : IRequestHandler<RemoveFileFromCollectionCommand, CollectionViewModel>
    {
        readonly IUserRepository _userRepository = userRepository;
        readonly IFileRepository _fileRepository = fileRepository;
        readonly ITagRepository _tagRepository = tagRepository;
        readonly ICollectionRepository _collectionRepository = collectionRepository;
        readonly IUnitOfWork _unitOfWork = unitOfWork;
        readonly IMapper _mapper = mapper;
        readonly IModelPredictionService _modelPredictionService = modelPredictionService;

        public async Task<CollectionViewModel> Handle(RemoveFileFromCollectionCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetAsync(
             [u => u.Id == request.UserId],
             [u => u.Collections],
             cancellationToken
           ) ?? throw new NotFoundException(nameof(User), request.UserId);

            var collection = await _collectionRepository.GetAsync(
                 [c => c.Id.ToString() == request.CollectionId],
                 [
                     c => c.Files,
                     c => c.Tags,
                 ],
                 cancellationToken
            ) ?? throw new NotFoundException(nameof(Collection), request.CollectionId);

            collection.Files.RemoveAll(f => f.Id.ToString() == request.FileId);

            var collectionVectors = collection.Files
                .Select(file => file.Vector)
                .Where(vector => vector != null)
                .ToList();

            collection.Tags.Clear();

            if (collectionVectors != null && collectionVectors.Count > 0)
            {
                collection.Vector = VectorHelper.ComputeAverageVector(collectionVectors!);

                var predictions = _modelPredictionService.ConvertVectorToPredictions(collection.Vector.ToArray());

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
            else
            {
                collection.Vector = null;
            }

            await _unitOfWork.Save(cancellationToken);

            return _mapper.Map<CollectionViewModel>(collection);
        }
    }
}
