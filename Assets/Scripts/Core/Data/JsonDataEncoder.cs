using Newtonsoft.Json;
using UnityEngine;


namespace HotPlay.BoosterMath.Core
{
    public class JsonDataEncoder : IDataEncoder
    {
        public void Encrypt<T>(string key, T data)
        {
            var json = JsonConvert.SerializeObject(data);
            PlayerPrefs.SetString(key, json);
            PlayerPrefs.Save();
        }
    }
}