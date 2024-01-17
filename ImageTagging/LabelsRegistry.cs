namespace Dreamscape.ImageRecognition;

public class LabelsRegistry
{
    private readonly List<string> _labels;


    public LabelsRegistry(string labelsFile)
    {
        _labels = new List<string>();

        using var sr = new StreamReader(labelsFile);
        while (!sr.EndOfStream)
        {
            var line = sr.ReadLine() ?? string.Empty;
            if (line != string.Empty)
            {
                _labels.Add(line);
            }
        }
    }

    public int Count => _labels.Count;

    public string this[int i] => _labels[i];
}