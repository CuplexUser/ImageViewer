using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using ImageViewer.Models;

namespace ImageViewer.Collections
{
    [Serializable]
    public class VolatileSettingsCollection : ICollection<VolatileSetting>
    {
        private Dictionary<string, VolatileSetting> _settings;

        public int Count => _settings.Count;

        public bool IsReadOnly => false;

        public VolatileSettingsCollection()
        {
            _settings = new Dictionary<string, VolatileSetting>();
        }

        public bool AddSetting(VolatileSetting setting)
        {
            if (!_settings.ContainsKey(setting.Name))
            {
                _settings.Add(setting.Name, setting);
                return true;
            }

            return false;
        }

        public bool ExistsInCache(string name)
        {
            return _settings.ContainsKey(name);
        }

        public VolatileSetting GetByName(string name)
        {
            if (!_settings.ContainsKey(name))
            {
                throw new ArgumentException("No key existed with the value: " + name, nameof(name));
            }

            return _settings[name];
        }

        public VolatileSetting AddOrUpdateSetting(VolatileSetting setting, object valueIfNotFound = null)
        {
            if (setting == null)
            {
                throw new ArgumentNullException(nameof(setting));
            }

            if (!_settings.ContainsKey(setting.Name))
            {
                var newVal = valueIfNotFound ?? setting.GetGenericValue();

                VolatileSetting newSetting = new VolatileSetting(setting.Name, newVal);

                _settings.Add(newSetting.Name, newSetting);
                return newSetting;
            }

            var loadedSetting = _settings[setting.Name];
            loadedSetting.SetGenericValue(setting.GetGenericValue());

            return loadedSetting;
        }

        public VolatileSetting GetCachedSetting(string name, object defValIdEmpty)
        {
            VolatileSetting returnVal;

            if (_settings.ContainsKey(name))
            {
                returnVal = _settings[name];
            }
            else
            {
                returnVal = new VolatileSetting(name, defValIdEmpty);
            }

            return returnVal;
        }


        public void Add(VolatileSetting item)
        {
            if (item != null) _settings.Add(item.Name, item);
        }

        public void Clear()
        {
            _settings.Clear();
        }

        public bool Contains(VolatileSetting item)
        {
            return _settings.Values.Any(x => x == item);
        }

        public void CopyTo(VolatileSetting[] array, int arrayIndex)
        {
            var settingsArray = _settings.AsQueryable().Select(x => x.Value).ToArray();
            int index = 0;
            for (int i = arrayIndex; i < array.Length; i++)
            {
                if (index >= settingsArray.Length)
                {
                    break;
                }
                array[i] = settingsArray[index++];
            }
        }

        public bool Remove(VolatileSetting item)
        {
            return item != null && _settings.Remove(item.Name);
        }

        public IEnumerator<VolatileSetting> GetEnumerator()
        {
            foreach (VolatileSetting setting in _settings.Values)
            {
                yield return setting;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (KeyValuePair<string, VolatileSetting> keyValuePair in _settings.AsEnumerable())
            {
                yield return keyValuePair;
            }
        }

        public TValue GetCachedSetting<TValue>(string name, TValue val)
        {
            if (_settings.ContainsKey(name))
            {
                object obj = _settings[name];
                if (obj is TValue value)
                {
                    return value;
                }
                throw new TypeLoadException($"Failed to return a value with key: {name} and TValue {val} because of conversion error between {obj.GetType().Name} and {val.GetType().Name}");
            }
            throw new KeyNotFoundException($"The collection did not contain any key with val: {name}");
        }

        internal TInv GetCachedSetting<TInv>(string key)
        {
            TInv def = default(TInv);
            return GetCachedSetting<TInv>(key, def);
        }
    }
}