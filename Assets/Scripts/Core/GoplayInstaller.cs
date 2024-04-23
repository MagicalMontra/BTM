using HotPlay.goPlay.Services;
using HotPlay.goPlay.Services.Rewards;
using HotPlay.goPlay.Services.Rewards.UI;
using UnityEngine;
using Zenject;

namespace HotPlay.BoosterMath.Core
{
    public class GoplayInstaller : MonoInstaller<GoplayInstaller>
    {
        [SerializeField]
        private RewardDialogManager goPlayDialogPrefab;
        
        [SerializeField]
        private GoPlayRewardService goPlayServicePrefab;
        
        [SerializeField]
        private GoPlayInitializer goPlayInitializerPrefab;
        
        public override void InstallBindings()
        {
            Container.Bind<RewardDialogManager>().FromComponentsInNewPrefab(goPlayDialogPrefab).AsSingle().NonLazy();
            Container.Bind<GoPlayRewardService>().FromComponentsInNewPrefab(goPlayServicePrefab).AsSingle().NonLazy();
            Container.Bind<GoPlayInitializer>().FromComponentsInNewPrefab(goPlayInitializerPrefab).AsSingle().NonLazy();
        }
    }
}