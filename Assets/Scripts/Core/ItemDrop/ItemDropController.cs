using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using HotPlay.BoosterMath.Core.Enemy;
using HotPlay.BoosterMath.Core.Player;
using HotPlay.BoosterMath.Core.UI;
using HotPlay.Utilities;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;
using Random = UnityEngine.Random;

namespace HotPlay.BoosterMath.Core
{
    public class ItemDropController : IDisposable
    {
        public Sprite NextDropIcon => icons[nextDrop];
        
        private ItemDropTypeEnum nextDrop;

        private IItemDropWorker currentItemDrop;

        private Dictionary<ItemDropTypeEnum, bool> activateBooster;

        [Inject]
        private readonly IGameplayPanel panel;

        [Inject]
        private readonly IEnemySpawner enemySpawner;
        
        [Inject]
        private readonly PlayerSpawner playerSpawner;

        [Inject]
        private readonly ShadowGenerator shadowGenerator;
        
        [Inject]
        private readonly QuestionController questionController;

        private readonly GameModeController gameModeController;
        
        private readonly Dictionary<ItemDropTypeEnum, Sprite> icons;
        
        private readonly Dictionary<ItemDropTypeEnum, float> dropChances;

        private readonly Dictionary<ItemDropTypeEnum, ItemDropBase.Pool> pools;

        private readonly Dictionary<ItemDropTypeEnum, IItemDropWorker> workers;
        
        private readonly Dictionary<ItemDropTypeEnum, List<ItemDropBase>> temporalDrops = new Dictionary<ItemDropTypeEnum, List<ItemDropBase>>();

        private List<UniTask> despawnTasks = new List<UniTask>();

        public ItemDropController(GameModeController gameModeController, IEnumerable<IItemDropWorker> dropAmountWorkers)
        {
            icons = new Dictionary<ItemDropTypeEnum, Sprite>();
            dropChances = new Dictionary<ItemDropTypeEnum, float>();
            pools = new Dictionary<ItemDropTypeEnum, ItemDropBase.Pool>();
            this.workers = new Dictionary<ItemDropTypeEnum, IItemDropWorker>();

            foreach (var worker in dropAmountWorkers)
            {
                pools.Add(worker.Type, worker.Pool);
                icons.Add(worker.Type, worker.Sprite);
                this.workers.Add(worker.Type, worker);
            }
            
            gameModeController.OnGameModeChanged += OnGameModeChanged;
        }
        
        public void Dispose()
        {
            if (gameModeController != null)
            {
                gameModeController.OnGameModeChanged -= OnGameModeChanged;
            }
        }

        public void PreDrop()
        {
            var type = ItemDropTypeEnum.Null;
            var chance = (float)Random.Range(1, 100);
            chance *= 0.01f;
            var lowest = 1f;

#if UNITY_EDITOR
            Debug.Log("Chance: " + chance);
#endif

            foreach (var drop in dropChances)
            {
                if (chance <= drop.Value)
                {
                    Assert.IsTrue(workers.ContainsKey(drop.Key));
                    
                    if (!workers[drop.Key].CanDrop)
                        continue;
                    
                    if (drop.Value > lowest)
                        continue;
                
                    if (nextDrop == drop.Key)
                        continue;
                    
                    if (playerSpawner.Current.CurrentHealth >= playerSpawner.Current.MaxHealth && drop.Key == ItemDropTypeEnum.Life)
                        continue;
                        
                    if (drop.Key is not (ItemDropTypeEnum.Rewind or ItemDropTypeEnum.Slow or ItemDropTypeEnum.ScoreBoost))
                    {
                        type = drop.Key;
                        lowest = drop.Value;
                        continue;
                    }

                    lowest = drop.Value;
                    type = type.RandomEnumValue(3);

                    var isRewindAbleToDrop = workers[ItemDropTypeEnum.Rewind].CanDrop;
                    var isSlowAbleToDrop = workers[ItemDropTypeEnum.Slow].CanDrop;
                    var isScoreBoostAbleToDrop = workers[ItemDropTypeEnum.ScoreBoost].CanDrop;

                    while (!workers[type].CanDrop)
                    {
                        if (!isRewindAbleToDrop && !isSlowAbleToDrop && !isScoreBoostAbleToDrop)
                        {
                            break;
                        }

                        type = type.RandomEnumValue(3);
                    }
                    
                }
            }

            // NOTE: in case that cannot pick any
            if (type == ItemDropTypeEnum.Null)
                type = ItemDropTypeEnum.Coin;

            nextDrop = type;
        }
        
        public void MarkDrop()
        {
            currentItemDrop = workers[nextDrop];
        }

        public void Drop()
        {
            if (currentItemDrop == null)
                return;
            
            var dropAmount = currentItemDrop.GetAmount();

            Assert.IsNotNull(enemySpawner.Current);

            if (!temporalDrops.ContainsKey(nextDrop))
                temporalDrops.Add(nextDrop, new List<ItemDropBase>());

            var position = new Vector3(
                enemySpawner.Current.transform.position.x,
                playerSpawner.Current.transform.position.y, 
                enemySpawner.Current.transform.position.z);

            for (int i = 0; i < dropAmount; i++)
            {
                var item = pools[nextDrop].Spawn(position, panel.transform);
                var shadow = shadowGenerator.Spawn(item.transform, position + new Vector3(0f, -0.29f, 0f));
                item.AttachShadow(shadow);
                temporalDrops[nextDrop].Add(item);
            }

            OnItemDropped(nextDrop);
            
            currentItemDrop = null;
        }

        public void OnItemDropped(ItemDropTypeEnum itemType)
        {
            workers[itemType].SetDropped(true);
        }

        public void OnItemCollected(ItemDropTypeEnum itemType)
        {
            workers[itemType].SetDropped(false);
        }

        public void OnBoosterActivated(ItemDropTypeEnum itemType, bool isActivated)
        {
            workers[itemType].IsActivated = isActivated;
        }

        public bool IsBoosterActivate(ItemDropTypeEnum booster)
        {
            return workers[booster].IsActivated;
        }

        public async UniTask Collect()
        {
            despawnTasks.Clear();
            List<ItemDropBase> select;
            var temp = temporalDrops.ToList();
            
            foreach (var drop in temp)
            {
                select = drop.Value;
                OnItemCollected(drop.Key);
                
                foreach (var item in select)
                {
                    despawnTasks.Add(item.Activate());
                }
            }
            
            await UniTask.WhenAll(despawnTasks);

            foreach (var drop in temp)
            {
                select = drop.Value;
                
                foreach (var item in select)
                {
                    shadowGenerator.Despawn(item.Shadow);
                    pools[drop.Key].Despawn(item);
                }
                
                select.Clear();
            }

            temporalDrops.Clear();
        }
#if CHEAT_ENABLED
        public async UniTask DebugCollect(ItemDropTypeEnum drop)
        {
            var position = playerSpawner.Current.transform.position;
            var item = pools[drop].Spawn(position, panel.transform);
            await item.Activate();
            pools[drop].Despawn(item);
        }
        
        public void DebugDrop(ItemDropTypeEnum drop)
        {
            nextDrop = drop;
            questionController.Panel.QuestionTimeGauge.UpdateDropIcon();
        }

        public void DebugForceDeactivate(ItemDropTypeEnum drop)
        {
            workers[drop].ForceStop();
        }
#endif
        public void DisposeDrops()
        {
            List<ItemDropBase> select;
            var temp = temporalDrops.ToList();
            
            foreach (var drop in temp)
            {
                select = drop.Value;
                
                foreach (var item in select)
                {
                    shadowGenerator.Despawn(item.Shadow);
                    pools[drop.Key].Despawn(item);
                }
                
                select.Clear();
            }

            temporalDrops.Clear();
        }

        private void OnGameModeChanged(GameMode gameMode)
        {
            if (gameMode == null)
                return;
            
            dropChances.Clear();
            dropChances.Add(ItemDropTypeEnum.Coin, gameMode.Settings.CoinDropRate);
            dropChances.Add(ItemDropTypeEnum.Life, gameMode.Settings.HeartDropRate);
            dropChances.Add(ItemDropTypeEnum.Slow, gameMode.Settings.BoosterModifier);
            dropChances.Add(ItemDropTypeEnum.Rewind, gameMode.Settings.BoosterModifier);
            dropChances.Add(ItemDropTypeEnum.ScoreBoost, gameMode.Settings.BoosterModifier);
        }
    }
}