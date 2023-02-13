namespace ImageViewer.Utility;

public class ExtraParam<T> where T : struct
{
    public ExtraParam(string name, T value)
    {
        Name = name;
        Value = value;
    }

    public string Name { get; init; }
    public T Value { get; init; }
}