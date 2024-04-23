using UnityEngine;
using Zenject;

namespace HotPlay.BoosterMath.Core
{
    public interface IItemDropWorker
    {
        ItemDropTypeEnum Type { get; }

        ItemDropBase.Pool Pool { get; }
        
        Sprite Sprite { get; }
        
        bool CanDrop { get; }

        bool IsActivated { get; set; }
        
        bool HasDropped { get; }

        int GetAmount();

        void SetDropped(bool value);

        void ForceStop();
    }
}