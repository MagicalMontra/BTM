using UnityEngine;
using Zenject;

namespace HotPlay.BoosterMath.Core
{
    //NOTE: Just in case we decide to add more drop amount logic to these boosters
    
    public class SlowItemDropWorker : IItemDropWorker
    {
        [Inject]
        public ItemDropTypeEnum Type { get; }
        
        [Inject]
        public ItemDropBase.Pool Pool { get; }
        
        [Inject]
        public Sprite Sprite { get; }
        
        [InjectLocal]
        private ITimer timer;

        public bool IsActivated { get;  set; }
        public bool HasDropped { get; private set; }
        public bool CanDrop => !IsActivated && !HasDropped;

        [InjectOptional]
        private int dropAmount = 1;

        public int GetAmount()
        {
            return dropAmount;
        }

        public void SetActivated(bool value)
        {
            IsActivated = value;
        }

        public void SetDropped(bool value)
        {
            HasDropped = value;
        }
        
        public void ForceStop()
        {
            if (timer.Counter > 0)
                timer.Stop();
        }
    }
    
    public class LifeItemDropWorker : IItemDropWorker
    {
        [Inject]
        public ItemDropTypeEnum Type { get; }
        
        [Inject]
        public ItemDropBase.Pool Pool { get; }
        
        [Inject]
        public Sprite Sprite { get; }

        public bool IsActivated { get; set; }
        public bool HasDropped { get; private set; }
        public bool CanDrop => !IsActivated && !HasDropped;

        [InjectOptional]
        private int dropAmount = 1;

        public int GetAmount()
        {
            return dropAmount;
        }

        public void SetActivated(bool value)
        {
            IsActivated = value;
        }

        public void SetDropped(bool value)
        {
            HasDropped = value;
        }
        
        public void ForceStop() { }
    }
    
    public class ScoreBoostItemDropWorker : IItemDropWorker
    {
        [Inject]
        public ItemDropTypeEnum Type { get; }
        
        [Inject]
        public ItemDropBase.Pool Pool { get; }
        
        [Inject]
        public Sprite Sprite { get; }

        public bool IsActivated { get; set; }
        public bool HasDropped { get; private set; }
        public bool CanDrop => !IsActivated && !HasDropped;

        [InjectOptional]
        private int dropAmount = 1;
        
        [InjectLocal]
        private ITimer timer;

        public int GetAmount()
        {
            return dropAmount;
        }
        
        public void SetDropped(bool value)
        {
            HasDropped = value;
        }
        
        public void ForceStop()
        {
            if (timer.Counter > 0)
                timer.Stop();
        }
    }
    
    public class RewindItemDropWorker : IItemDropWorker
    {
        [Inject]
        public ItemDropTypeEnum Type { get; }
        
        [Inject]
        public ItemDropBase.Pool Pool { get; }
        
        [Inject]
        public Sprite Sprite { get; }

        public bool IsActivated { get; set; }
        public bool HasDropped { get; private set; }
        public bool CanDrop => !IsActivated && !HasDropped;

        [InjectOptional]
        private int dropAmount = 1;
        
        [InjectLocal]
        private ITimer timer;

        public int GetAmount()
        {
            return dropAmount;
        }

        public void SetDropped(bool value)
        {
            HasDropped = value;
        }

        public void ForceStop()
        {
            if (timer.Counter > 0)
                timer.Stop();
        }
    }
}