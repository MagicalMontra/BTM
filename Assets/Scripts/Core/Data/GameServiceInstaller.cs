using UnityEngine;
using Zenject;

namespace HotPlay.BoosterMath.Core
{
    [CreateAssetMenu(menuName = "HotPlay/BoosterMath/Data/Create GameServiceInstaller", fileName = "GameServiceInstaller", order = 0)]
    public class GameServiceInstaller : ScriptableObjectInstaller<GameServiceInstaller>
    {
        [SerializeField]
        private GameData gameData;

        [SerializeField]
        private SoundData soundData;
        
        [SerializeField]
        private ShopDatabase shopDatabase;
        
        public override void InstallBindings()
        {
            DataInstaller.Install(Container);
            Container.Bind<GameData>().FromInstance(gameData).AsSingle();
            Container.Bind<SoundData>().FromInstance(soundData).AsSingle();
            Container.Bind<ShopDatabase>().FromInstance(shopDatabase).AsSingle();
        }
    }
}