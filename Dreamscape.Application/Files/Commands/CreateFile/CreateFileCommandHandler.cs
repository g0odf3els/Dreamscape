using Accord.Imaging.ColorReduction;
using AutoMapper;
using Dreamscape.Application.Common.Exceptions;
using Dreamscape.Application.Repositories;
using Dreamscape.Application.Services;
using Dreamscape.Domain.Entities;
using ImageMagick;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Pgvector;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;

namespace Dreamscape.Application.Files.Commands.CreateFile
{
    internal class CreateFileCommandHandler(
        IFileRepository fileRepository,
        IUserRepository userRepository,
        IResolutionRepository resolutionRepository,
        ITagRepository tagRepository,
        IColorRepository colorRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IWebHostEnvironment webHostEnvironment,
        IModelPredictionService modelPredictionService) : IRequestHandler<CreateFileCommand, ImageFileViewModel>
    {
        readonly IFileRepository _fileRepository = fileRepository;
        readonly IUserRepository _userRepository = userRepository;
        readonly IResolutionRepository _resolutionRepository = resolutionRepository;
        readonly ITagRepository _tagRepository = tagRepository;
        readonly IColorRepository _colorRepository = colorRepository;
        readonly IUnitOfWork _unitOfWork = unitOfWork;
        readonly IMapper _mapper = mapper;
        readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;
        readonly IModelPredictionService _modelPredictionService = modelPredictionService;

        public async Task<ImageFileViewModel> Handle(CreateFileCommand request, CancellationToken cancellationToken)
        {
            IFormFile file = request.File;

            var uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
            var previewPath = Path.Combine(_webHostEnvironment.WebRootPath, "previews");

            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }
            if (!Directory.Exists(previewPath))
            {
                Directory.CreateDirectory(previewPath);
            }

            var user = await _userRepository.GetAsync(
               [u => u.Id == request.UserId],
               [u => u.Collections],
               cancellationToken
            ) ?? throw new NotFoundException(nameof(User), request.UserId);

            var fileId = Guid.NewGuid();

            var uniqueFileName = $"{fileId}{Path.GetExtension(file.FileName)}";

            var filePath = Path.Combine(uploadPath, uniqueFileName);
            var filePreviewPath = Path.Combine(previewPath, uniqueFileName);

            var imageFile = new ImageFile
            {
                Id = fileId,
                Name = $"{fileId}{Path.GetExtension(file.FileName)}",
                Tags = [],
                Colors = [],
                FullSizePath = "/uploads/" + uniqueFileName,
                DisplaySizePath = "/previews/" + uniqueFileName,
                Uploader = user,
                UploaderId = request.UserId,
                Length = file.Length,
            };

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream, cancellationToken);
            }

            using (MagickImage image = new(filePath))
            {
                var resolution = await _resolutionRepository.GetAsync([r => r.Width == image.Width && r.Height == image.Height], null, cancellationToken) ??
                    _resolutionRepository.Create(new Resolution()
                    {
                        Width = image.Width,
                        Height = image.Height
                    });

                imageFile.Resolution = resolution;

                double aspectRatio = (double)image.Width / image.Height;

                int _previewWidth = 450;
                int _previewHeight = (int)(_previewWidth / aspectRatio);

                image.Resize(new MagickGeometry(_previewWidth, _previewHeight)
                {
                    FillArea = true
                });

                image.Extent(_previewWidth, _previewHeight, Gravity.Center);
                image.Write(filePreviewPath);
            }

            if (request.Tags != null && request.Tags.Length > 0)
            {
                foreach (var tagName in request.Tags)
                {
                    var tag = await _tagRepository.GetAsync([t => t.Name == tagName], null, cancellationToken) ??
                        _tagRepository.Create(new Tag()
                        {
                            Name = tagName
                        });

                    imageFile.Tags.Add(tag);
                }
            }

            imageFile.Vector = new Vector(_modelPredictionService.ProcessImageToVector(filePath));
            var predictedTags = _modelPredictionService.ConvertVectorToPredictions(imageFile.Vector.ToArray());

            foreach (var prediction in predictedTags.Where(prediction => prediction.Confidence > 1))
            {
                var tag = await _tagRepository.GetAsync([tag => tag.Name == prediction.Label], null, cancellationToken) ??
                    _tagRepository.Create(new Tag()
                    {
                        Name = prediction.Label
                    });

                imageFile.Tags.Add(tag);
            }

            foreach (var color in GetColorPalette(filePath, 5))
            {
                var attachedColor = await _colorRepository.GetAsync(
                   [c => c.A == color.A && c.R == color.R && c.G == color.G && c.B == color.B], null, cancellationToken) ??
                   _colorRepository.Create(new Domain.Entities.Color()
                   {
                       A = color.A,
                       R = color.R,
                       G = color.G,
                       B = color.B,
                   });

                imageFile.Colors.Add(attachedColor);
            }

            _fileRepository.Create(imageFile);
            await _unitOfWork.Save(cancellationToken);

            return _mapper.Map<ImageFileViewModel>(imageFile);
        }

        public static List<System.Drawing.Color> GetColorPalette(string path, int paletteSize)
        {
            HashSet<Rgba32> colors = [];

            using (var image = Image.Load<Rgba32>(path))
            {
                var pixels = image.GetPixelMemoryGroup();

                foreach (var pixel in pixels)
                {
                    for (int i = 0; i < pixel.Length; i++)
                    {
                        colors.Add(pixel.Span[i]);
                    }
                }
            }

            IColorQuantizer quantizer = new MedianCutQuantizer();

            foreach (var color in colors)
            {
                quantizer.AddColor(System.Drawing.Color.FromArgb(color.R, color.G, color.B));
            }

            return quantizer.GetPalette(paletteSize).ToList();
        }
    }
}
