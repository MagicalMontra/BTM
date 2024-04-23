using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HotPlay.PecanUI.Skin
{
    public partial class SkinConfigs
    {
        [TabGroup("Login")]
        [Title("Small Reward", subtitle: "Key: login_small_reward", titleAlignment: TitleAlignments.Split)]
        [SerializeField]
        [HideLabel]
        private LoginElementSkinData loginSmallReward;
        public LoginElementSkinData LoginSmallReward => loginSmallReward;

        [TabGroup("Login")]
        [Title("Big Reward", subtitle: "Key: login_big_reward", titleAlignment: TitleAlignments.Split)]
        [SerializeField]
        [HideLabel]
        private LoginElementSkinData loginBigReward;
        public LoginElementSkinData LoginBigReward => loginBigReward;

        [TabGroup("Login")]
        [Title("Day Label", subtitle: "Key: login_reward_day_label", titleAlignment: TitleAlignments.Split)]
        [SerializeField]
        [HideLabel]
        private TMPSkinData loginRewardDayLabel;
        public TMPSkinData LoginRewardDayLabel => loginRewardDayLabel;

        [TabGroup("Login")]
        [Title("Amount Label", subtitle: "Key: login_reward_amount_label", titleAlignment: TitleAlignments.Split)]
        [SerializeField]
        [HideLabel]
        private TMPSkinData loginRewardAmountLabel;
        public TMPSkinData LoginRewardAmountLabel => loginRewardAmountLabel;

        [TabGroup("Login")]
        [Title("Day Label", subtitle: "Key: login_big_reward_day_label", titleAlignment: TitleAlignments.Split)]
        [SerializeField]
        [HideLabel]
        private TMPSkinData loginBigRewardDayLabel;
        public TMPSkinData LoginBigRewardDayLabel => loginBigRewardDayLabel;

        [TabGroup("Login")]
        [Title("Amount Label", subtitle: "Key: login_big_reward_amount_label", titleAlignment: TitleAlignments.Split)]
        [SerializeField]
        [HideLabel]
        private TMPSkinData loginBigRewardAmountLabel;
        public TMPSkinData LoginBigRewardAmountLabel => loginBigRewardAmountLabel;
    }
}
