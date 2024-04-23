using HotPlay.BoosterMath.Core.Enemy;
using HotPlay.BoosterMath.Core.Player;
using HotPlay.BoosterMath.Core.UI;
using UnityEngine;
using Zenject;

namespace HotPlay.BoosterMath.Core
{
    public class GameplayInstaller : MonoInstaller<GameplayInstaller>
    {
        [SerializeField]
        private GameObject gameplayPanel;
        
        [SerializeField]
        private GameObject enemyContextPrefab;
        
        [SerializeField]
        private GameObject playerContextPrefab;

        [SerializeField]
        private GameObject heartContextPrefab;
        
        [SerializeField]
        private GameObject answerContextPrefab;
        
        [SerializeField]
        private GameObject boosterContextPrefab;
        
        [SerializeField]
        private GameObject questionContextPrefab;

        public override void InstallBindings()
        {
            Container.Bind<AnswerValidator>().AsSingle();
            Container.Bind<GameSessionController>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameplayUIController>().AsSingle();
            Container.Bind<PauseController>().FromNewComponentOnNewGameObject().AsSingle();
            Container.Bind<IGameplayPanel>().FromComponentInNewPrefab(gameplayPanel).AsSingle();
            Container.Bind<HeartPanel>().FromSubContainerResolve().ByNewContextPrefab(heartContextPrefab).AsSingle();
            Container.Bind<IEnemySpawner>().FromSubContainerResolve().ByNewContextPrefab(enemyContextPrefab).AsSingle();
            Container.Bind<BoosterPanel>().FromSubContainerResolve().ByNewContextPrefab(boosterContextPrefab).AsSingle();
            Container.Bind<PlayerSpawner>().FromSubContainerResolve().ByNewContextPrefab(playerContextPrefab).AsSingle();
            Container.Bind<QuestionController>().FromSubContainerResolve().ByNewContextPrefab(questionContextPrefab).AsSingle();
            Container.Bind<TutorialController>().AsSingle();
            Container.BindInterfacesAndSelfTo<AnswerUIController>().FromSubContainerResolve().ByNewContextPrefab(answerContextPrefab).AsSingle();
        }
    }
}