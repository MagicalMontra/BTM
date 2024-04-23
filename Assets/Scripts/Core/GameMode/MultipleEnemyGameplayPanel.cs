using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace HotPlay.BoosterMath.Core.UI
{
    public class MultipleEnemyGameplayPanel : MonoBehaviour, IGameplayPanel
    {
        public Transform EnemyPivot => GetPivot();

        public Transform PlayerPivot => playerPivot;

        [SerializeField]
        private EnemyPoint[] enemyPivots;

        [SerializeField]
        private Transform playerPivot;
        
        private Transform GetPivot()
        {
            Assert.IsTrue(enemyPivots.Length > 0);
            var point = enemyPivots.First(point => !point.isOccupied);
            Assert.IsNotNull(point);
            point.isOccupied = true;
            return point.pivot;
        }
        
        public void ResetEnemyPivot()
        {
            foreach (var pivot in enemyPivots)
            {
                pivot.isOccupied = false;
            }
        }

        [Serializable]
        private class EnemyPoint
        {
            public bool isOccupied;
            public Transform pivot;
        }
    }
}