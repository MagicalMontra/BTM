using Newtonsoft.Json;
using UnityEngine;
using Zenject;

namespace HotPlay.BoosterMath.Core
{
    public class JsonDataDecoder : IDataDecoder
    {
        public T Decrypt<T>(string key)
        {
            var rawString = PlayerPrefs.GetString(key);
            return JsonConvert.DeserializeObject<T>(rawString);
        }
    }
}