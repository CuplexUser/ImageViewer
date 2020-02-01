using System;
using System.CodeDom;
using System.Configuration;
using System.Linq;
using ImageViewer.Library.Extensions;

namespace ImageViewer.Library.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public abstract class ValidRangeAttribute : Attribute
    {
        public int MaxValue;
        public int MinValue;

        protected ValidRangeAttribute()
        {
            
        }

        [ConfigurationProperty()]
        protected virtual bool IsValid()
        {
            new CodeThisReferenceExpression().ValidateObject().ToList().Any(x=>x.)
        };
    }
}