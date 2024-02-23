using Pgvector;

namespace Dreamscape.Application.Common.Helpers
{
    public class VectorHelper
    {
        public static Vector ComputeAverageVector(List<Vector> vectors)
        {
            var dimensions = vectors.First().Memory.Length;

            var sum = new float[dimensions];

            foreach (var vector in vectors)
            {
                var vectorArray = vector.ToArray();
                for (int i = 0; i < dimensions; i++)
                {
                    sum[i] += vectorArray[i];
                }
            }

            var average = sum.Select(s => s / vectors.Count).ToArray();
            return new Vector(average);
        }
    }
}
