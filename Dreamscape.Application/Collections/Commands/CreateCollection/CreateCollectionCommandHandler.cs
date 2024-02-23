using AutoMapper;
using Dreamscape.Application.Common.Exceptions;
using Dreamscape.Application.Common.Helpers;
using Dreamscape.Application.Repositories;
using Dreamscape.Application.Services;
using Dreamscape.Domain.Entities;
using MediatR;

namespace Dreamscape.Application.Collections.Commands.CreateCollection
{
    public class CreateCollectionCommandHandler(
        IUserRepository userRepository,
        IFileRepository fileRepository,
        ICollectionRepository collectionRepository,
        ITagRepository tagRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IModelPredictionService modelPredictionService)
        : IRequestHandler<CreateCollectionCommand, CollectionViewModel>
    {
        readonly IUserRepository _userRepository = userRepository;
        readonly IFileRepository _fileRepository = fileRepository;
        readonly ITagRepository _tagRepository = tagRepository;
        readonly ICollectionRepository _collectionRepository = collectionRepository;
        readonly IUnitOfWork _unitOfWork = unitOfWork;
        readonly IMapper _mapper = mapper;
        readonly IModelPredictionService _modelPredictionService = modelPredictionService;

        public async Task<CollectionViewModel> Handle(CreateCollectionCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetAsync(
                [u => u.Id == request.UserId],
                [u => u.Collections],
                cancellationToken
            ) ?? throw new NotFoundException(nameof(User), request.UserId);

            var collection = new Collection()
            {
                Name = request.Name,
                Owner = user,
                Description = request.Description,
                IsPrivate = request.IsPrivate,
                OwnerId = request.UserId,
            };

            if (request.FilesId != null)
            {
                foreach (var file in request.FilesId)
                {
                    var result = await _fileRepository.GetAsync(
                    [f => f.Id.ToString() == file],
                    null,
                    cancellationToken
                    ) ?? throw new NotFoundException(nameof(ImageFile), file);

                    collection.Files.Add(result);
                }

                var collectionVectors = collection.Files
                   .Select(file => file.Vector)
                   .Where(vector => vector != null)
                   .ToList();

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
            }

            _collectionRepository.Create(collection);
            await _unitOfWork.Save(cancellationToken);

            return _mapper.Map<CollectionViewModel>(collection);
        }
    }
}
