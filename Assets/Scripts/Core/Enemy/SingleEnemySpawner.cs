using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using HotPlay.BoosterMath.Core.Character;
using HotPlay.BoosterMath.Core.UI;
using HotPlay.Utilities;
using UnityEngine;

namespace HotPlay.BoosterMath.Core.Enemy
{
    public class SingleEnemySpawner : IEnemySpawner
    {
        public ICharacter Current { get; private set; }

        private readonly string defaultKey = "wolf01";

        private readonly ICharacter.Factory factory;
        
        private readonly IGameplayPanel gameplayPanel;

        private readonly ThemeSelector themeSelector;

        public SingleEnemySpawner(IGameplayPanel gameplayPanel, ICharacter.Factory factory, ThemeSelector themeSelector)
        {
            this.factory = factory;
            this.gameplayPanel = gameplayPanel;
            this.themeSelector = themeSelector;
        }
        
        public async UniTask Spawn(CancellationToken cancellationToken)
        {
            if (Current != null)
            {
                Current.Dispose();
                Current = null;
            }
            
            if (cancellationToken.IsCancellationRequested)
            {
                await UniTask.Yield();
                return;
            }

            var selectedPrefab = themeSelector.Current.enemies.RandomPick().prefab;
            Current = factory.Create(selectedPrefab, gameplayPanel.EnemyPivot);
            Current.Reinitialize();
            await UniTask.Yield();
        }

        public async UniTask Despawn()
        {
            if (Current == null)
                return;
            
            await Current.transform.DOMove(Current.transform.position + Vector3.left * 30f, 5f);
            Current?.Dispose();
            Current = null;
        }
    }
}