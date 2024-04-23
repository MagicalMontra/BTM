using UnityEngine;
using Zenject;

namespace HotPlay.BoosterMath.Core
{
    public class PecanSettingUIInitializer : PecanUIInitializerBase
    {
        private const string statusId = "SoundStatus";

        [Inject]
        private SoundStatus soundStatus;
        
        public override void Setup()
        {
            soundStatus.Initialize();
            Services.Events.UpdateBGMStatus(soundStatus.bgmStatus);
            Services.Events.UpdateSFXStatus(soundStatus.sfxStatus);
            Services.Events.SettingsEventHandler.EnableBGM += OnBGMToggleValueChanged;
            Services.Events.SettingsEventHandler.EnableSFX += OnSFXToggleValueChanged;
        }

        public override void Terminate()
        {
            Services.Events.SettingsEventHandler.EnableBGM -= OnBGMToggleValueChanged;
            Services.Events.SettingsEventHandler.EnableSFX -= OnSFXToggleValueChanged;
        }
        
        private void OnBGMToggleValueChanged(bool value)
        {
            soundStatus.bgmStatus = value;
            Services.Events.UpdateBGMStatus(value);
            Save();

        }

        private void OnSFXToggleValueChanged(bool value)
        {
            soundStatus.sfxStatus = value;
            Services.Events.UpdateSFXStatus(value);
            Save();
        }

        private void Save()
        {
            var json = JsonUtility.ToJson(soundStatus);
            PlayerPrefs.SetString(statusId, json);
            PlayerPrefs.Save();
        }
    }
}