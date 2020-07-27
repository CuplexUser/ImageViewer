using System;
using ImageViewer.Models.Interface;
using JetBrains.Annotations;

namespace ImageViewer.Models
{
    [Serializable]
    public class VolatileSetting
    {
        private object _val;
        public VolatileSetting(string name, object value)
        {
            _val = value;
            Name = name;
        }

        public string Name { get; private set; }


        public void SetGenericValue(object value)
        {
            _val = value;
        }

        public object GetGenericValue()
        {
            return _val;
        }
    }


    public abstract class SettingTypeBase<TVal>
    {
        [NotNull] private TVal _objValue;
        [NotNull] private readonly string _settingName;
        private Guid _settingId;

        protected TVal SettingValue
        {
            get => _objValue;
            set => _objValue = value;
        }

        public string SettingName => _settingName;

        public Guid UniqueId
        {
            get
            {
                if (_settingId == Guid.Empty)
                {
                    _settingId = Guid.NewGuid();
                }

                return _settingId;
            }
        }

        protected SettingTypeBase(string name, TVal value)
        {
            if (value == null)
            {
                throw new NullReferenceException(nameof(value));
            }

            _objValue = value;
            _settingName = name ?? throw new NullReferenceException(nameof(name));
            _settingId = Guid.Empty;
        }

        public abstract void SetGenericValue(TVal value);

        public abstract TVal GetGenericValue();

        public virtual VolatileSetting GetVolatileSetting()
        {
            return new VolatileSetting(SettingName, SettingValue);
        }

        public Type GetValueType()
        {
            return typeof(TVal);
        }

    }

    public class SettingTypeBool : SettingTypeBase<bool>
    {
        public SettingTypeBool(string name, bool value) : base(name, value)
        {


        }

        public override void SetGenericValue(bool value)
        {
            SettingValue = value;
        }

        public override bool GetGenericValue()
        {
            return SettingValue;
        }
    }

    public class SettingTypeString : SettingTypeBase<string>
    {
        public SettingTypeString(string name, string value) : base(name, value)
        {
        }

        public override void SetGenericValue(string value)
        {
            SettingValue = value;
        }

        public override string GetGenericValue()
        {
            return SettingValue;
        }
    }

    public class SettingTypeInt : SettingTypeBase<int>
    {
        public SettingTypeInt(string name, int value) : base(name, value)
        {
        }

        public override void SetGenericValue(int value)
        {
            SettingValue = value;
        }

        public override int GetGenericValue()
        {
            return SettingValue;
        }
    }

    public class SettingTypeDouble : SettingTypeBase<double>
    {
        public SettingTypeDouble(string name, double value) : base(name, value)
        {
        }

        public override void SetGenericValue(double value)
        {
            SettingValue = value;
        }

        public override double GetGenericValue()
        {
            return SettingValue;
        }
    }

    public class SettingTypeFloat : SettingTypeBase<float>
    {
        public SettingTypeFloat(string name, float value) : base(name, value)
        {
        }

        public override void SetGenericValue(float value)
        {
            SettingValue = value;
        }

        public override float GetGenericValue()
        {
            return SettingValue;
        }
    }
}
