using UnityEngine;

namespace HotPlay.BoosterMath.Core.UI
{
    public interface IGameplayPanel
    {
        Transform transform { get; }
        
        Transform EnemyPivot { get; }
        
        Transform PlayerPivot { get; }

        void ResetEnemyPivot();
    }
}