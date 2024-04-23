using HotPlay.goPlay.Services.Rewards;
using UnityEngine;

namespace HotPlay.PecanUI
{
    public class RewardButton : MonoBehaviour
    {
        public void ShowRewardBasket() 
        {
            if (GoPlayRewardService.Instance.CanReceiveReward())
                GoPlayRewardService.Instance.ReceiveReward();
        }
    }
}