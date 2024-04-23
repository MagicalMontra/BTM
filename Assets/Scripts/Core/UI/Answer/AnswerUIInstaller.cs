using UnityEngine;
using Zenject;

namespace HotPlay.BoosterMath.Core.UI
{
    public class AnswerUIInstaller : MonoInstaller<AnswerUIInstaller>
    {
        [SerializeField]
        private AnswerPanel panelPrefab;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<AnswerUIController>().AsSingle();
            Container.Bind<AnswerPanel>().FromInstance(panelPrefab).AsSingle().NonLazy();
            Container.BindFactory<AnswerPanel, Transform, AnswerPanel, AnswerPanel.Factory>().FromFactory<UIFactory<AnswerPanel>>();
        }
    }
}