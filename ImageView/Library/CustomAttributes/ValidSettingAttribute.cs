namespace ImageViewer.Library.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    internal sealed class ValidRangeAttribute : Attribute
    {
        public readonly int MaxValue;
        public readonly int MinValue;

        // See the attribute guidelines at 
        //  http://go.microsoft.com/fwlink/?LinkId=85236


        // This is a positional argument
        public ValidRangeAttribute(int min, int max)
        {
            MinValue = min;
            MaxValue = max;
        }


        // This is a named argument
        public int NamedInt { get; set; }
    }
}