#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using Doozy.Runtime.Colors.Models;
using UnityEngine;

namespace HotPlay.Utilities
{

    [Serializable]
    public class ReferenceClassTree<T> : ISerializationCallbackReceiver where T : class
    {
        [SerializeField]
        private string separator = "/";

        [SerializeField]
        private SerializedData[] rawData = new SerializedData[0];

        private Dictionary<string, T> database = new Dictionary<string, T>();
        private Dictionary<string, (bool, T)> cached = new Dictionary<string, (bool, T)>();

        public bool TryGet(in string key, out T val)
        {
            if (cached.ContainsKey(key))
            {
                var (ret, value) = cached[key];
                val = value;
                return ret;
            }

            var currentKey = key;
            while (!string.IsNullOrEmpty(currentKey))
            {
                if (database.TryGetValue(currentKey, out val))
                {
                    if (val != default(T))
                    {
                        cached.Add(key, (true, val));
                        return true;
                    }
                }
                var lastIndex = currentKey.LastIndexOf(separator);
                if (lastIndex == -1)
                {
                    break;
                }
                currentKey = key.Substring(0, lastIndex);
            }

            cached.Add(key, (false, default!));
            val = default!;
            return false;
        }
#if UNITY_EDITOR
        public void Edit(string key, T newData, string editKey = "")
        {
            for (int i = 0; i < rawData.Length; i++)
            {
                if (rawData[i].Key != key) 
                    continue;
                
                rawData[i].Value = newData;

                if (!string.IsNullOrEmpty(editKey) && rawData[i].Key != editKey)
                    rawData[i].Key = editKey;
                
                break;
            }
        }
        
        public void TryEdit(string key, T newData, string editKey = "")
        {
            var list = rawData.ToList();
            
            if (!list.Exists(data => data.Key == key))
            {
                list.Add(new SerializedData{ Key = key, Value = newData });
                rawData = list.ToArray();
                return;
            }
            
            for (int i = 0; i < rawData.Length; i++)
            {
                if (rawData[i].Key != key) 
                    continue;
                
                rawData[i].Value = newData;

                if (!string.IsNullOrEmpty(editKey) && rawData[i].Key != editKey)
                    rawData[i].Key = editKey;
                
                break;
            }
        }

        public SerializedData Read(string keySearch)
        {
            return rawData.Where(data => data.Key.Contains(keySearch.ToLower())).Select(data => data).GetEnumerator().Current;
        }
        
        public SerializedData[] ReadMany(string keySearch)
        {
            return rawData.Where(data => data.Key.ToLower().Contains(keySearch.ToLower())).ToArray();
        }

        public string[] GetKeys(string keySearch)
        {
            return rawData.Where(data => data.Key.ToLower().Contains(keySearch.ToLower())).Select(data => data.Key).ToArray();
        }

        public void Add(string key, T val)
        {
            var listData = rawData.ToList();

            if (listData.Exists(data => data.Key == key))
                return;
            
            var newData = new SerializedData
            {
                Key = key,
                Value = val
            };
            
            listData.Add(newData);
            rawData = listData.ToArray();
        }

        public bool Remove(string key)
        {
            var listData = rawData.ToList();

            if (!listData.Exists(data => data.Key == key))
                return false;
            
            listData.RemoveAll(data => data.Key == key);
            rawData = listData.ToArray();
            return true;
        }
#endif
        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
            var splitter = new string[] { separator };
            foreach (var data in rawData)
            {
                var separated = data.Key.Split(splitter, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < separated.Length; ++i)
                {
                    var key = string.Join(separator, separated, 0, i+1);
                    if (!database.ContainsKey(key))
                    {
                        database.Add(key, default!);
                    }
                }
                database[data.Key] = data.Value;
            }
        }

        [Serializable]
        public struct SerializedData
        {
            public string Key;
            public T Value;
        }
    }
}