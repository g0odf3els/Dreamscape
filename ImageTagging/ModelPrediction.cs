using Microsoft.ML.OnnxRuntime;

namespace Dreamscape.ImageRecognition;

public class ModelPrediction
{
    private readonly ImageProcessor _imageProcessor;

    private readonly InferenceSession _inferenceSession;

    private readonly LabelsRegistry _labelsRegistry;

    public ModelPrediction(string modelPath, string labelsPath)
    {
        _inferenceSession = new InferenceSession(modelPath);
        _imageProcessor = new ImageProcessor();
        _labelsRegistry = new LabelsRegistry(labelsPath);
    }

    public float[] ProcessImageToVector(string imagePath)
    {
        var inputs = new List<NamedOnnxValue>
        {
            NamedOnnxValue.CreateFromTensor("input.1", _imageProcessor.ProcessImage(imagePath))
        };

        using IDisposableReadOnlyCollection<DisposableNamedOnnxValue> results = _inferenceSession.Run(inputs);

        var featureVector = results.First().AsEnumerable<float>().ToArray();

        return featureVector;
    }

    public float[] ProcessImageToVector(Stream stream)
    {
        var inputs = new List<NamedOnnxValue>
        {
            NamedOnnxValue.CreateFromTensor("input.1", _imageProcessor.ProcessImage(stream))
        };

        using IDisposableReadOnlyCollection<DisposableNamedOnnxValue> results = _inferenceSession.Run(inputs);

        var featureVector = results.First().AsEnumerable<float>().ToArray();

        return featureVector;
    }

    public IEnumerable<Prediction> PredictTags(string imagePath)
    {
        var inputs = new List<NamedOnnxValue>
        {
            NamedOnnxValue.CreateFromTensor("input.1", _imageProcessor.ProcessImage(imagePath))
        };

        using IDisposableReadOnlyCollection<DisposableNamedOnnxValue> results = _inferenceSession.Run(inputs);

        var output = results.First().AsEnumerable<float>();

        var predictionResults = output
            .Select((confidence, labelIndex) => new Prediction(_labelsRegistry[labelIndex], confidence))
            .OrderBy(x => x.Confidence).ToList();

        return predictionResults;
    }
}