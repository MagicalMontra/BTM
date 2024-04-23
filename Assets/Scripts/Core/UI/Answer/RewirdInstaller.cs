using Rewired;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace HotPlay.BoosterMath.Core.UI
{
    public class RewirdInstaller : MonoInstaller<RewirdInstaller>
    {
        [SerializeField]
        private EventSystem eventSystem;
    
        [SerializeField]
        private InputManager_Base rewired;
        
        public override void InstallBindings()
        {
            Container.Bind<EventSystem>().FromComponentsInNewPrefab(eventSystem).AsSingle().NonLazy();
            Container.Bind<InputManager_Base>().FromComponentsInNewPrefab(rewired).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<RewirdInputController>().AsSingle();
        }
    }
}