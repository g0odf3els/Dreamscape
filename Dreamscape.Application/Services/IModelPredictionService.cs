using Dreamscape.ImageRecognition;

namespace Dreamscape.Application.Services
{
    public interface IModelPredictionService
    {
        IEnumerable<Prediction> PredictTags(string filePath);
        float[] ProcessImageToVector(string filePath);
        float[] ProcessImageToVector(Stream stream);
    }
}
