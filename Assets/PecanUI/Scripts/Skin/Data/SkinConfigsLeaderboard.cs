using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HotPlay.PecanUI.Skin
{
    public partial class SkinConfigs
    {
        [TabGroup("Leaderboard")]
        [Title("Item", subtitle: "Key: leaderboard_item", titleAlignment: TitleAlignments.Split)]
        [SerializeField]
        [HideLabel]
        private PanelSkinData leaderboardItem;
        public PanelSkinData LeaderboardItem => leaderboardItem;

        [TabGroup("Leaderboard")]
        [Title("Display", subtitle: "Key: leaderboard_display", titleAlignment: TitleAlignments.Split)]
        [SerializeField]
        [HideLabel]
        private PanelSkinData leaderboardDisplay;
        public PanelSkinData LeaderboardDisplay => leaderboardDisplay;

        [TabGroup("Leaderboard")]
        [Title("Item Name Label", subtitle: "Key: leaderboard_item_name_label", titleAlignment: TitleAlignments.Split)]
        [SerializeField]
        [HideLabel]
        private TMPSkinData leaderboardItemNameLabel;
        public TMPSkinData LeaderboardItemNameLabel => leaderboardItemNameLabel;

        [TabGroup("Leaderboard")]
        [Title("Item Label", subtitle: "Key: leaderboard_item_score_label", titleAlignment: TitleAlignments.Split)]
        [SerializeField]
        [HideLabel]
        private TMPSkinData leaderboardItemScoreLabel;
        public TMPSkinData LeaderboardItemScoreLabel => leaderboardItemScoreLabel;

        [TabGroup("Leaderboard")]
        [Title("Item Tag", subtitle: "Key: leaderboard_item_tag", titleAlignment: TitleAlignments.Split)]
        [SerializeField]
        [HideLabel]
        private LeaderboardTagSkinData leaderboardTag;
        public LeaderboardTagSkinData LeaderboardTag => leaderboardTag;
    }
}
