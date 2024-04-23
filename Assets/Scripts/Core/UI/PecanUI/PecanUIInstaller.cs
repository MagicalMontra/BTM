using HotPlay.PecanUI;
using UnityEngine;
using Zenject;

namespace HotPlay.BoosterMath.Core
{
    [CreateAssetMenu(menuName = "HotPlay/BoosterMath/PecanUI/Create PecanUIInstaller", fileName = "PecanUIInstaller", order = 0)]
    public class PecanUIInstaller : ScriptableObjectInstaller<PecanUIInstaller>
    {
        [SerializeField]
        private PecanServices services;
        
        public override void InstallBindings()
        {
            Container.Bind<PecanServices>().FromComponentInNewPrefab(services).AsSingle().NonLazy();
        }
    }
}