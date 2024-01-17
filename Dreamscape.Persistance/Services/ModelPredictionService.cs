using Dreamscape.Application.Services;
using Dreamscape.ImageRecognition;
using Microsoft.Extensions.Configuration;

namespace WallpaperPortal.Services
{
    public class ModelPredictionService : IModelPredictionService
    {
        private readonly ModelPrediction _modelPrediction;

        public ModelPredictionService(IConfiguration configuration)
        {
            _modelPrediction = new ModelPrediction(configuration["MlModel:Path"], configuration["MlModel:LabelsPath"]);
        }

        public IEnumerable<Prediction> PredictTags(string filePath)
        {
            return _modelPrediction.PredictTags(filePath);
        }

        public float[] ProcessImageToVector(string filePath)
        {
            return _modelPrediction.ProcessImageToVector(filePath);
        }

        public float[] ProcessImageToVector(Stream stream)
        {
            return _modelPrediction.ProcessImageToVector(stream);
        }
    }
}
