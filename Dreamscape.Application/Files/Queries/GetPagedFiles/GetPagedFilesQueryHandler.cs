using AutoMapper;
using Dreamscape.Application.Repositories;
using Dreamscape.Application.Services;
using Dreamscape.Domain.Entities;
using MediatR;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using Tensorflow.Contexts;

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
                filterExpressions = filterExpressions.Append(f => f.UploaderId == request.UploaderId).ToArray();
            }

            if (request.Search != null && request.Search.Length > 0)
            {
                filterExpressions = filterExpressions.Append(f => f.Tags.Any(t => t.Name == request.Search.ToLower())).ToArray();
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

            Expression<Func<ImageFile, object>>? orderBy = null;

            switch (request.Order)
            {
                case (int)FileSortOrderEnum.Random:
                    orderBy = f => Guid.NewGuid();
                    break;

                default:
                    orderBy = f => f.DataCreated;
                    break;
            }

            var result = await _fileRepository.GetPagedAsync(
                pageNumber: request.Page,
                pageSize: request.PageSize,
                predicate: filterExpressions,
                orderBy: orderBy,
                orderByDescending: request.OrderByDescending,
                include: [
                    f => f.Resolution,
                    f => f.Uploader,
                    f => f.Colors
                 ],
               cancellationToken: cancellationToken);

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
