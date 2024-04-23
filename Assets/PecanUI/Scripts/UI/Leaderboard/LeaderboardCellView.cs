using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Threading.Tasks;
using EnhancedUI.EnhancedScroller;
using System.Threading;

namespace HotPlay.PecanUI.Leaderboard
{
    public class LeaderboardCellView : EnhancedScrollerCellView
    {
        public LeaderboardListRow Prefab => prefab;
        public HorizontalLayoutGroup LayoutGroup => layoutGroup;

        [SerializeField]
        private LeaderboardListRow prefab;

        [SerializeField]
        private RectTransform rectTransform;

        [SerializeField]
        private HorizontalLayoutGroup layoutGroup;

        [SerializeField]
        private int maxItemCount;

        private LeaderboardListRow[] items;

        private void Awake()
        {
            items = new LeaderboardListRow[maxItemCount];
            for(int i = 0; i < maxItemCount; i++)
            {
                LeaderboardListRow shopItem = Instantiate(prefab, rectTransform);
                items[i] = shopItem;
                items[i].gameObject.SetActive(false);
            }
        }

        public void Init(PlayerLeaderboardProfileData[] dataArray, int rank, int playerRank)
        {
            Debug.Assert(dataArray.Length <= maxItemCount, "Data exceed the maximum range of container");
            int count = 0;

            RefreshItems();

            for (int i = 0; i < dataArray.Length; i++)
            {
                LeaderboardListRow item = items[count];
                item.Setup(dataArray[i], rank, playerRank, true);
                item.gameObject.SetActive(true);
                count++;

                if(count > items.Length)
                {
                    break;
                }
            }
        }

        public override void RefreshCellView()
        {
            base.RefreshCellView();
            RefreshItems();
        }

        private void RefreshItems()
        {
            foreach (LeaderboardListRow item in items)
            {
                item.gameObject.SetActive(false);
            }
        }
    }
}