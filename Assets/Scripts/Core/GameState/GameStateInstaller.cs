using UnityEngine;
using Zenject;

namespace HotPlay.BoosterMath.Core
{
    [CreateAssetMenu(menuName = "HotPlay/BoosterMath/Core/Create GameStateInstaller", fileName = "GameStateInstaller", order = 0)]
    public class GameStateInstaller : ScriptableObjectInstaller<GameStateInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GameStateProcessor>().AsSingle();
            Container.BindInterfacesAndSelfTo<EnemyState>().AsCached();
            Container.BindInterfacesAndSelfTo<PlayerState>().AsCached();
            Container.BindInterfacesAndSelfTo<ProcessState>().AsCached();
            Container.BindInterfacesAndSelfTo<MainMenuState>().AsCached();
            Container.BindInterfacesAndSelfTo<InitializationState>().AsCached();
            Container.BindInterfacesAndSelfTo<ReinitializationState>().AsCached();
            Container.BindInterfacesAndSelfTo<TransitState>().AsCached();
            Container.BindInterfacesAndSelfTo<TerminationState>().AsCached();
            
            Container.DeclareSignalWithInterfaces<ChangeGameStateSignal>();

            Container.BindSignal<IChangeGameStateSignal>()
                .ToMethod<GameStateProcessor>(handler => handler.OnStateChanged)
                .FromResolveAll();
        }
    }
}