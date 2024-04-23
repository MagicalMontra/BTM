using UnityEngine;

namespace HotPlay.DailyLogin
{
    [CreateAssetMenu(fileName = "DailyLoginRewardData", menuName = "GameData/DailyLoginRewardData")]
    public class DailyLoginRewardData : ScriptableObject
    {
        [Tooltip("Array of rewards sorted by day")]
        [SerializeField]
        private int[] rewards;
        public int[] Rewards => rewards;
    }
}
