namespace ImageViewer.Utility
{
    public class ExtraParam<T> where T : struct
    {
        public string Name { get; init; }
        public T Value { get; init; }

        public ExtraParam(string name, T value)
        {
            Name = name;
            Value = value;
        }
    }
}