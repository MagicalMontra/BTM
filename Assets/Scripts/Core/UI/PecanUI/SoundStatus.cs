using System;
using UnityEngine;
using Zenject;

namespace HotPlay.BoosterMath.Core
{
    [Serializable]
    public class SoundStatus
    {
        public bool bgmStatus;
        public bool sfxStatus;
        
        [InjectOptional(Id = "SoundStatus")]
        private readonly string statusId = "SoundStatus";

        public SoundStatus()
        {
            bgmStatus = true;
            sfxStatus = true;
        }

        public void Initialize()
        {
            var json = PlayerPrefs.GetString(statusId);
            var temp = string.IsNullOrEmpty(json) ? new SoundStatus() : JsonUtility.FromJson<SoundStatus>(json);
            bgmStatus = temp.bgmStatus;
            sfxStatus = temp.sfxStatus;
        }
    }
}