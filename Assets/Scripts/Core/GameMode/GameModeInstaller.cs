using UnityEngine;
using Zenject;

namespace HotPlay.BoosterMath.Core.UI
{
    [CreateAssetMenu(menuName = "HotPlay/BoosterMath/Core/Create GameModeInstaller", fileName = "GameModeInstaller", order = 0)]
    public class GameModeInstaller : ScriptableObjectInstaller<GameModeInstaller>
    {
        [SerializeField]
        private GameObject[] modesPrefab;

        [SerializeField]
        private GameModeSelectionUI selectionUIPrefab;
        
        public override void InstallBindings()
        {
            Container.Bind<GameModeController>().AsSingle();
            Container.Bind<GameModeSelectionUI>().FromInstance(selectionUIPrefab).AsSingle();
            Container.BindFactory<GameModeSelectionUI, Transform, GameModeSelectionUI, GameModeSelectionUI.Factory>().FromFactory<UIFactory<GameModeSelectionUI>>();
            ModeBind();
        }

        private void ModeBind()
        {
            foreach (var modePrefab in modesPrefab)
            {
                Container.Bind<GameMode>().FromSubContainerResolve().ByNewContextPrefab(modePrefab).AsTransient();
            }
        }
    }
}