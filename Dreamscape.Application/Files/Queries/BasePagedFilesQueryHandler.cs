using AutoMapper;
using Dreamscape.Application.Repositories;
using Dreamscape.Domain.Entities;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Dreamscape.Application.Files.Queries
{
    public abstract class BasePagedFilesQueryHandler
    {
        protected readonly IFileRepository _fileRepository;
        protected readonly IMapper _mapper;

        protected BasePagedFilesQueryHandler(IFileRepository fileRepository, IMapper mapper)
        {
            _fileRepository = fileRepository;
            _mapper = mapper;
        }

        protected void AddResolutionFilter(ref Expression<Func<ImageFile, bool>>[] expressions, string resolutions)
        {
            var f = Expression.Parameter(typeof(ImageFile), "f");

            var resolutionParameter = Expression.PropertyOrField(f, "Resolution");
            var widthParameter = Expression.PropertyOrField(resolutionParameter, "Width");
            var heightParameter = Expression.PropertyOrField(resolutionParameter, "Height");

            Expression? finalExpression = null;

            foreach (var resolution in ParsePairs(resolutions))
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
                expressions = expressions.Append(lambda).ToArray();
            }
        }

        protected void AddAspectRatioFilter(ref Expression<Func<ImageFile, bool>>[] expressions, string aspectRatios)
        {
            var f = Expression.Parameter(typeof(ImageFile), "f");

            var resolutionParameter = Expression.PropertyOrField(f, "Resolution");
            var widthExpression = Expression.PropertyOrField(resolutionParameter, "Width");
            var heightExpression = Expression.PropertyOrField(resolutionParameter, "Height");

            Expression? finalExpression = null;

            foreach (var aspect in ParsePairs(aspectRatios))
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
                expressions = expressions.Append(lambda).ToArray();
            }
        }

        private static List<(int, int)> ParsePairs(string input)
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
