using HotPlay.BoosterMath.Core.Character;
using HotPlay.BoosterMath.Core.UI;
using HotPlay.Utilities;
using JetBrains.Annotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Assertions;

namespace HotPlay.BoosterMath.Core.Enemy
{
    public class MultipleEnemySpawner : IEnemySpawner
    {
        public ICharacter Current
        {
            get
            {
                if (currentIndex >= enemies.Count)
                    return null;
                
                if (enemies[currentIndex]?.CurrentHealth > 0)
                    return enemies[currentIndex];

                enemies[currentIndex].Renderer.materials[1].SetFloat(FillPhase, 0.95f);
                enemies[currentIndex].Renderer.sortingOrder = currentIndex + 1;
                gameplayPanel.ResetEnemyPivot();
                currentIndex++;

                if (currentIndex >= enemies.Count)
                    return null;
                
                enemies[currentIndex] =  enemies[currentIndex];
                enemies[currentIndex].Renderer.sortingOrder = enemies.Count;
                return enemies[currentIndex];
            }
        }

        private readonly string defaultKey = "wolf01";

        private int currentIndex;

        private List<ICharacter> enemies;

        private int enemyIndex = -1;

        private readonly ICharacter.Factory factory;

        private readonly ThemeSelector themeSelector;
        
        private readonly IGameplayPanel gameplayPanel;

        private readonly GameModeController gameModeController;
        
        private readonly GameSessionController gameSessionController;
        
        private readonly int FillPhase = Shader.PropertyToID("_FillPhase");

        public MultipleEnemySpawner(
            ICharacter.Factory factory,
            ThemeSelector themeSelector,
            IGameplayPanel gameplayPanel,
            GameModeController gameModeController,
            GameSessionController gameSessionController)
        {
            this.factory = factory;
            this.gameplayPanel = gameplayPanel;
            this.themeSelector = themeSelector;
            this.gameModeController = gameModeController;
            this.gameSessionController = gameSessionController;
        }
        
        public async UniTask Spawn(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                await UniTask.Yield();
                return;
            }
            
            if (enemyIndex < 0)
                enemyIndex = Random.Range(0, themeSelector.Current.enemies.Length);
            
            enemies?.Clear();
            currentIndex = 0;
            enemies ??= new List<ICharacter>();
            Assert.IsNotNull(gameModeController.CurrentGameMode);

            List<UniTask> tweens = new List<UniTask>();
            int enemyLength = themeSelector.Current.enemies.Length;

            var spawnCount = Mathf.FloorToInt(gameModeController.CurrentGameMode.Settings.EnemyCurve.Evaluate(gameSessionController.CurrentStage));
            for (int i = 0; i < spawnCount; i++)
            {
                var selectedPrefab = themeSelector.Current.enemies[enemyIndex].prefab;
                var instance = factory.Create(selectedPrefab, gameplayPanel.EnemyPivot);
                var position = instance.transform.position;
                var originalPos = position;
                position = new Vector3(position.x + 30f, position.y, position.z);
                instance.transform.position = position;
                var task = instance.transform.DOMoveX(originalPos.x, 1f).ToUniTask().AttachExternalCancellation(cancellationToken);
                tweens.Add(task);
                instance.Walk(cancellationToken);
                instance.Renderer.sortingOrder = spawnCount - i;
                instance.Reinitialize();
                enemies.Add(instance);
                enemyIndex++;
                enemyIndex %= enemyLength;
            }

            await UniTask.WhenAll(tweens).AttachExternalCancellation(cancellationToken);
            
            if (cancellationToken.IsCancellationRequested)
            {
                await UniTask.Yield();
                return;
            }
            
            await UniTask.Delay(250, DelayType.DeltaTime, PlayerLoopTiming.Update, cancellationToken);
            
            if (cancellationToken.IsCancellationRequested)
            {
                await UniTask.Yield();
                return;
            }

            foreach (var enemy in enemies)
            {
                enemy.Idle(cancellationToken);
            }
        }

        public async UniTask Despawn()
        {
            if (enemies == null)
                return;

            foreach (var enemy in enemies)
            {
                enemy?.Dispose();
            }
            
            enemies?.Clear();
            gameplayPanel.ResetEnemyPivot();
            await UniTask.Yield();
        }
    }
}