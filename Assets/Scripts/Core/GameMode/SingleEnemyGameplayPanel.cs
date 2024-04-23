using UnityEngine;

namespace HotPlay.BoosterMath.Core.UI
{
    public class SingleEnemyGameplayPanel : MonoBehaviour, IGameplayPanel
    {
        public Transform EnemyPivot => enemyPivot;

        public Transform PlayerPivot => playerPivot;

        [SerializeField]
        private Transform enemyPivot;
        
        [SerializeField]
        private Transform playerPivot;

        public void ResetEnemyPivot() { }
    }
}