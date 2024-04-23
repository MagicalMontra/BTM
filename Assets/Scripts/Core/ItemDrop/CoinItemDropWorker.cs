using HotPlay.BoosterMath.Core.UI;
using UnityEngine;
using Zenject;

namespace HotPlay.BoosterMath.Core
{
    public class CoinItemDropWorker : IItemDropWorker
    {
        [Inject]
        public ItemDropTypeEnum Type { get; }
        
        [Inject]
        public ItemDropBase.Pool Pool { get; }
        
        [Inject]
        public Sprite Sprite { get; }

        public bool IsActivated { get; set; }
        public bool CanDrop => !IsActivated && !HasDropped;
        public bool HasDropped { get; private set; }

        private readonly GameModeController gameModeController;

        public CoinItemDropWorker(GameModeController gameModeController)
        {
            this.gameModeController = gameModeController;
        }
        
        public int GetAmount()
        {
            var dropRate = gameModeController.CurrentGameMode.Settings.CoinDropRate;
            var chance = Random.Range(dropRate, 1f);
            return chance < dropRate ? 0 : Random.Range(gameModeController.CurrentGameMode.Settings.MinCoinAmount, gameModeController.CurrentGameMode.Settings.MaxCoinAmount + 1);
        }

        public void SetDropped(bool value)
        {
            HasDropped = value;
        }

        public void ForceStop() { }
    }
}