using Microsoft.ML.OnnxRuntime;

namespace Dreamscape.ImageRecognition;

public class ModelPrediction
{
    private Lazy<InferenceSession> _inferenceSession;
    private Lazy<ImageProcessor> _imageProcessor;
    private readonly LabelsRegistry _labelsRegistry;

    public ModelPrediction(string modelPath, string labelsPath)
    {
        _inferenceSession = new Lazy<InferenceSession>(() => new InferenceSession(modelPath));
        _imageProcessor = new Lazy<ImageProcessor>(() => new ImageProcessor());
        _labelsRegistry = new LabelsRegistry(labelsPath);
    }

    public float[] ProcessImageToVector(string imagePath)
    {
        var inputs = new List<NamedOnnxValue>
            {
                NamedOnnxValue.CreateFromTensor("input", _imageProcessor.Value.ProcessImage(imagePath))
            };

        using var results = _inferenceSession.Value.Run(inputs);

        return results.First().AsEnumerable<float>().ToArray();
    }

    public float[] ProcessImageToVector(Stream stream)
    {
        var inputs = new List<NamedOnnxValue>
            {
                NamedOnnxValue.CreateFromTensor("input", _imageProcessor.Value.ProcessImage(stream))
            };

        using var results = _inferenceSession.Value.Run(inputs);

        return results.First().AsEnumerable<float>().ToArray();
    }

    public List<Prediction> ConvertVectorToPredictions(float[] featureVector)
    {
        return featureVector.Select((confidence, labelIndex) =>
            new Prediction(_labelsRegistry[labelIndex], confidence)).ToList();
    }

    public IEnumerable<Prediction> PredictTagsForImage(string imagePath)
    {
        var featureVector = ProcessImageToVector(imagePath);
        return ConvertVectorToPredictions(featureVector);
    }
}