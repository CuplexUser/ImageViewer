using Castle.Core.Resource;

namespace ImageViewer.Models.Interface
{
    public interface INameValueGenericSetting<TValueType>
    {
        TValueType GetValue();

        void SetValue(TValueType value);

        string GetName();
        
        object ParentObject
        {
            get;
            set;
        }

        string SettingName { get; }
    }
}