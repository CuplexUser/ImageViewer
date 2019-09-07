using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using Castle.Core.Internal;

namespace ImageViewer.Storage
{
    public class DataContractComparer<T> : IEqualityComparer<T> where T : class
    {
        private readonly T _compareBase;
        public DataContractComparer(T compareBase)
        {
            if (compareBase.GetType().GetAttribute<DataContractAttribute>().TypeId.ToString() != typeof(DataContractAttribute).FullName)
            {
                throw new InvalidEnumArgumentException("Compare type was missing DataContract attribute");
            }

            _compareBase = compareBase;
            var dataMemberAttributeInfo = (compareBase as T).GetType().GetProperties();

            bool dataMemberAttr = dataMemberAttributeInfo.ToList().Any(dm => dm.GetAttribute<DataMemberAttribute>().TypeId.ToString() == typeof(DataMemberAttribute).FullName);

            if (!dataMemberAttr)
            {
                throw new InvalidEnumArgumentException("Compare type did not have any dataMember property attribute");
            }
        }

        public DataContractComparer()
        {

        }


        public bool Equals(T y)
        {
            return Equals(_compareBase, y);
        }

        private static Type GetType(PropertyInfo[] propArrayX, int i)
        {
            Type propType = propArrayX[i].PropertyType;
            return propType;
        }

        public bool Equals(T x, T y)
        {
            if (x?.GetType() != y?.GetType())
            {
                return false;
            }

            if (x == null)
            {
                return false;
            }

            var propArrayX = x.GetType().GetProperties();
            var propArrayY = y.GetType().GetProperties().ToList();

            for (int i = 0; i < propArrayX.Length; i++)
            {
                Type propType = GetType(propArrayX, i);
                if (propType.BaseType != null)
                {
                    Type baseType = propType.BaseType.UnderlyingSystemType.DeclaringType;
                }

                string name = propArrayX[i].GetMethod.Name;
                var propValue = propArrayX[i].GetValue(x);
                var yProperty = propArrayY[i].GetValue(y);

                if (yProperty == null || !yProperty.Equals(propValue))
                {
                    return false;
                }

            }

            return true;
        }

        public int GetHashCode(T obj)
        {
            return obj.GetHashCode();
        }
    }
}
