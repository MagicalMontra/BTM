using UnityEngine;
using Zenject;

namespace HotPlay.BoosterMath.Core.UI
{
    public class GameplayUIInstaller : MonoInstaller<GameplayUIInstaller>
    {
        [SerializeField]
        private GameObject answerUIPrefab;
        
        [SerializeField]
        private GameObject questionUiPrefab;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GameplayUIController>().AsSingle();
            Container.BindInterfacesAndSelfTo<AnswerUIController>().FromSubContainerResolve().ByNewContextPrefab(answerUIPrefab).AsSingle();
            Container.BindInterfacesAndSelfTo<QuestionController>().FromSubContainerResolve().ByNewContextPrefab(questionUiPrefab).AsSingle();
        }
    }
}