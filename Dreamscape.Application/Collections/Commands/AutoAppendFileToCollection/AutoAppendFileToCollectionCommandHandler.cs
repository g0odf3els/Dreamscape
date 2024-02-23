using MediatR;
using AutoMapper;
using Dreamscape.Application.Repositories;
using Dreamscape.Application.Services;
using Dreamscape.Application.Common.Exceptions;
using Dreamscape.Domain.Entities;
using Pgvector.EntityFrameworkCore;
using Dreamscape.Application.Common.Helpers;

namespace Dreamscape.Application.Collections.Commands.AutoAppendFileToCollection
{
    public class AutoAppendFileToCollectionCommandHandler(
        IUserRepository userRepository,
        IFileRepository fileRepository,
        ITagRepository tagRepository,
        ICollectionRepository collectionRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IModelPredictionService modelPredictionService)
        : IRequestHandler<AutoAppendFileToCollectionCommand, CollectionViewModel>
    {

        readonly IUserRepository _userRepository = userRepository;
        readonly IFileRepository _fileRepository = fileRepository;
        readonly ITagRepository _tagRepository = tagRepository;
        readonly ICollectionRepository _collectionRepository = collectionRepository;
        readonly IUnitOfWork _unitOfWork = unitOfWork;
        readonly IMapper _mapper = mapper;
        readonly IModelPredictionService _modelPredictionService = modelPredictionService;


        public async Task<CollectionViewModel> Handle(AutoAppendFileToCollectionCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetAsync(
             [u => u.Id == request.UserId],
             [u => u.Collections],
             cancellationToken
           ) ?? throw new NotFoundException(nameof(User), request.UserId);

            var file = await _fileRepository.GetAsync(
              [c => c.Id.ToString() == request.FileId],
              null,
              cancellationToken
            ) ?? throw new NotFoundException(nameof(ImageFile), request.FileId);

            var collections = await _collectionRepository.GetPagedAsync(
                    1,
                    15,
                    [c => c.OwnerId == request.UserId],
                    null,
                    [
                        c => c.Files,
                        c => c.Tags
                    ],
                    false,
                    cancellationToken
                );

            var collection = collections.Items.First();
            
            collection.Files.Add(file);

            var collectionVectors = collection.Files
                .Select(file => file.Vector)
                .Where(vector => vector != null)
                .ToList();

            collection.Tags.Clear();

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
