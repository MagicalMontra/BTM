#nullable enable

using HotPlay.PecanUI.Events;
using HotPlay.Sound;
using System;
using static HotPlay.PecanUI.Configs.PecanSoundConfigs;

namespace HotPlay.PecanUI.Sound
{
    public class PecanSoundManager
    {
        private readonly SoundManager soundManager;
        private readonly SoundConfigs soundConfigs;

        public bool EnableBGM { get; private set; }
        public bool EnableSFX { get; private set; }

        public PecanSoundManager(SoundManager soundManager, EventsHandler events, SoundConfigs soundConfigs)
        {
            if (soundManager == null)
            {
                throw new ArgumentNullException(nameof(soundManager));
            }

            if (events is null)
            {
                throw new ArgumentNullException(nameof(events));
            }

            if (soundConfigs == null)
            {
                throw new ArgumentNullException(nameof(soundConfigs));
            }

            this.soundManager = soundManager;
            this.soundConfigs = soundConfigs;

            events.EnableBGM += OnEnableBGM;
            events.EnableSFX += OnEnableSFX;

            events.SettingsEventHandler.EnableBGM += OnEnableBGM;
            events.SettingsEventHandler.EnableSFX += OnEnableSFX;
        }

        public void PlayOnce(string soundConfigsPath)
        {
            if(soundConfigs.TryGet(soundConfigsPath, out var soundClip))
            {
                soundManager.SoundPlayer.Play(soundClip, false);
            }
        }

        public void PlayLoop(string soundConfigsPath)
        {
            if (soundConfigs.TryGet(soundConfigsPath, out var soundClip))
            {
                soundManager.SoundPlayer.Play(soundClip, true);
            }
        }

        public void PlayBGM(string soundConfigsPath)
        {
            if (soundConfigs.TryGet(soundConfigsPath, out var soundClip))
            {
                soundManager.SoundPlayer.PlayBGM(soundClip);
            }
        }

        private void OnEnableSFX(bool isEnable)
        {
            EnableSFX = isEnable;
            soundManager.SoundPlayer.SetEnableSFX(isEnable);
        }

        private void OnEnableBGM(bool isEnable)
        {
            EnableBGM = isEnable;
            soundManager.SoundPlayer.SetEnableBGM(isEnable);
        }
    }
}