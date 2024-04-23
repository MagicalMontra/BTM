using HotPlay.PecanUI;
using UnityEngine;
using Zenject;

namespace HotPlay.BoosterMath.Core.UI
{
    public class QuestionUIInstaller : MonoInstaller<QuestionUIInstaller>
    {
        [Inject]
        private PecanServices pecanServices;
        
        [SerializeField]
        private QuestionPanel panelPrefab;
        
        [SerializeField]
        private QuestionComponent numberPrefab;
        
        [SerializeField]
        private QuestionComponent operatorPrefab;

        [SerializeField]
        private Transform componentDisableGroup;

        private const string numberPoolId = "NumberPool";
        private const string operatorPoolId = "OperatorPool";

        public override void InstallBindings()
        {
            var disableGroup = pecanServices.GetCustomGamePlayPanel<GameplayUI>().DisableGroup;
            Container.BindInterfacesAndSelfTo<QuestionController>().AsSingle();
            Container.Bind<QuestionPanel>().FromInstance(panelPrefab).AsSingle();
            Container.Bind<Transform>().FromComponentInNewPrefab(componentDisableGroup).AsSingle();
            Container.Bind<QuestionComponent>().WithId("Number").FromInstance(numberPrefab).AsTransient();
            Container.Bind<QuestionComponent>().WithId("Operator").FromInstance(operatorPrefab).AsTransient();
            Container.BindFactory<QuestionPanel, Transform, QuestionPanel, QuestionPanel.Factory>().FromFactory<UIFactory<QuestionPanel>>();
            Container.BindMemoryPool<IGameUIComponent, IGameUIComponent.Pool>()
                .WithId(numberPoolId)
                .WithInitialSize(3)
                .FromComponentInNewPrefab(numberPrefab)
                .UnderTransform(disableGroup);
            
            Container.BindMemoryPool<IGameUIComponent, IGameUIComponent.Pool>()
                .WithId(operatorPoolId)
                .WithInitialSize(3)
                .FromComponentInNewPrefab(operatorPrefab)
                .UnderTransform(disableGroup);
        }
    }
}