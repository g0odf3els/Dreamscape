using Dreamscape.ImageRecognition;

namespace Dreamscape.Application.Services
{
    public interface IModelPredictionService
    {
        float[] ProcessImageToVector(string filePath);
        float[] ProcessImageToVector(Stream stream);
        IEnumerable<Prediction> ConvertVectorToPredictions(float[] featureVector);
        IEnumerable<Prediction> PredictTags(string filePath);
    }
}
