using AutoMapper;
using Dreamscape.Application.Repositories;
using Dreamscape.Application.Services;
using Dreamscape.Domain.Entities;
using MediatR;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Dreamscape.Application.Files.Queries.GetPagedFiles
{
    public class GetPagedFilesQueryHandler(
        IFileRepository fileRepository,
        IMapper mapper,
        IModelPredictionService modelPredictionService)
        : IRequestHandler<GetPagedFilesQuery, PagedList<ImageFileViewModel>>
    {

        readonly IFileRepository _fileRepository = fileRepository;
        readonly IMapper _mapper = mapper;
        readonly IModelPredictionService _modelPredictionService = modelPredictionService;

        public async Task<PagedList<ImageFileViewModel>> Handle(GetPagedFilesQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<ImageFile, bool>>[] filterExpressions = [];
           
            if (request.CollectionId != null)
            {
                filterExpressions = filterExpressions.Append(f => f.Collections.Any(c => c.Id.ToString() == request.CollectionId)).ToArray();
            }

            if (request.UploaderId != null)
            {
                filterExpressions = filterExpressions.Append(f => f.UploaderId == request.UserId).ToArray();
            }

            if (request.Tags != null && request.Tags.Length > 0)
            {
                filterExpressions = filterExpressions.Append(f => f.Tags.Any(t => request.Tags.Contains(t.Name))).ToArray();
            }

            if (request.Resolutions != null)
            {
                var f = Expression.Parameter(typeof(ImageFile), "f");

                var resolutionParameter = Expression.PropertyOrField(f, "Resolution");
                var widthParameter = Expression.PropertyOrField(resolutionParameter, "Width");
                var heightParameter = Expression.PropertyOrField(resolutionParameter, "Height");

                Expression? finalExpression = null;

                foreach (var resolution in ParsePairs(request.Resolutions))
                {
                    var widthEquals = Expression.Equal(widthParameter, Expression.Constant(resolution.Item1));
                    var heightEquals = Expression.Equal(heightParameter, Expression.Constant(resolution.Item2));

                    var andExpression = Expression.And(widthEquals, heightEquals);

                    if (finalExpression == null)
                    {
                        finalExpression = andExpression;
                    }
                    else
                    {
                        finalExpression = Expression.Or(finalExpression, andExpression);
                    }
                }

                if (finalExpression != null)
                {
                    var lambda = Expression.Lambda<Func<ImageFile, bool>>(finalExpression, f);
                    filterExpressions = filterExpressions.Append(lambda).ToArray();
                }
            }
            
            if (request.AspectRatios != null)
            {
                var f = Expression.Parameter(typeof(ImageFile), "f");

                var resolutionParameter = Expression.PropertyOrField(f, "Resolution");
                var widthExpression = Expression.PropertyOrField(resolutionParameter, "Width");
                var heightExpression = Expression.PropertyOrField(resolutionParameter, "Height");

                Expression? finalExpression = null;

                foreach (var aspect in ParsePairs(request.AspectRatios))
                {
                    var aspectEquals = Expression.Equal(
                        Expression.Divide(
                            Expression.Convert(widthExpression, typeof(double)),
                            Expression.Convert(heightExpression, typeof(double))),
                        Expression.Constant(aspect.Item1 / (double)aspect.Item2));

                    finalExpression = finalExpression == null ? aspectEquals : Expression.OrElse(finalExpression, aspectEquals);
                }

                if (finalExpression != null)
                {
                    var lambda = Expression.Lambda<Func<ImageFile, bool>>(finalExpression, f);
                    filterExpressions = filterExpressions.Append(lambda).ToArray();
                }
            }

            PagedList<ImageFile> result;

            if (request.File != null)
            {
                float[] vector;
                using (var ms = new MemoryStream())
                {
                    await request.File.CopyToAsync(ms);
                    ms.Seek(0, SeekOrigin.Begin);
                    vector = _modelPredictionService.ProcessImageToVector(ms);
                }

                result =  _fileRepository.GetSimilarPagedAsync(
                   request.Page,
                   request.PageSize,
                   new Pgvector.Vector(vector),
                   filterExpressions,
                   [
                       f => f.Resolution,
                       f => f.Uploader,
                       f => f.Colors
                   ],
                   cancellationToken
               );
            }
            else
            {
                result = await _fileRepository.GetPagedAsync(
                   request.Page,
                   request.PageSize,
                   filterExpressions,
                   null,
                   null,
                   [
                       f => f.Resolution,
                       f => f.Uploader,
                   ],
                   cancellationToken
               );
            }

            return _mapper.Map<PagedList<ImageFileViewModel>>(result);
        }

        private static List<(int, int)> ParsePairs(string? input)
        {
            List<(int, int)> resolutions = [];
            string pattern = @"(\d+)x(\d+)";

            MatchCollection matches = Regex.Matches(input, pattern);
            foreach (Match match in matches.Cast<Match>())
            {
                int width = int.Parse(match.Groups[1].Value);
                int height = int.Parse(match.Groups[2].Value);
                resolutions.Add((width, height));
            }

            return resolutions;
        }
    }
}
