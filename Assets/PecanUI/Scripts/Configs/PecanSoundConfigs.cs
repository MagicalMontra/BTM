using HotPlay.Sound;
using HotPlay.Utilities;
using System;
using UnityEngine;

namespace HotPlay.PecanUI.Configs
{
    [CreateAssetMenu(menuName ="HotPlay/Pecan UI/Sound Config")]
    public class PecanSoundConfigs : ScriptableObject
    {
        [SerializeField]
        private SoundConfigs configs;
        public SoundConfigs Configs => configs;

        [Serializable]
        public class SoundConfigs : ReferenceClassTree<SoundClip> { }
    }
}
