using UnityEngine.Assertions;
using Zenject;

namespace HotPlay.BoosterMath.Core
{
    public class PecanMainMenuUIInitializer : PecanUIInitializerBase
    {
        [Inject]
        private SoundData soundData;
        
        [Inject]
        private SoundStatus soundStatus;

        public override void Setup()
        {
            Assert.IsNotNull(Services);
            
            if (soundStatus.bgmStatus)
                Services.SoundManager.SoundPlayer.PlayBGM(soundData.MainMenuBGM);
        }

        public override void Terminate()
        {
            
        }
    }
}