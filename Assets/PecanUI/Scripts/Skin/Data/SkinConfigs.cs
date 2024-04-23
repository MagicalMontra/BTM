using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Sirenix.OdinInspector;

namespace HotPlay.PecanUI.Skin
{
    [CreateAssetMenu(menuName = "Pecan UI/Skin Configs", fileName = "PecanSkinConfigs")]
    public partial class SkinConfigs : ScriptableObject
    {
        #region Dialogs
        [TabGroup("Common")]
        [Title("Header Panel", subtitle: "Key: header_panel", titleAlignment: TitleAlignments.Split)]
        [SerializeField]
        [HideLabel]
        private PanelSkinData headerPanel;
        public PanelSkinData HeaderPanel => headerPanel;

        [TabGroup("Common")]
        [Title("Header Label", subtitle: "Key: header_label", titleAlignment: TitleAlignments.Split)]
        [SerializeField]
        [HideLabel]
        private TMPSkinData headerLabel;
        public TMPSkinData HeaderLabel => headerLabel;

        [TabGroup("Common")]
        [Title("Dialog Panel", subtitle: "Key: dialog_panel", titleAlignment: TitleAlignments.Split)]
        [SerializeField]
        [HideLabel]
        private PanelSkinData dialogPanel;
        public PanelSkinData DialogPanel => dialogPanel;

        [TabGroup("Common")]
        [Title("Dialog Label", subtitle: "Key: dialog_label", titleAlignment: TitleAlignments.Split)]
        [SerializeField]
        [HideLabel]
        private TMPSkinData dialogLabel;
        public TMPSkinData DialogLabel => dialogLabel;
        #endregion

        [Space(30)]

        #region Currency Bar
        [TabGroup("Common")]
        [Title("Currency Bar", subtitle: "Key: currency_bar", titleAlignment: TitleAlignments.Split)]
        [SerializeField]
        [HideLabel]
        private PanelSkinData currencyBar;
        public PanelSkinData CurrencyBar => currencyBar;

        [TabGroup("Common")]
        [Title("Currency Bar Label", subtitle: "Key: currency_bar_label", titleAlignment: TitleAlignments.Split)]
        [SerializeField]
        [HideLabel]
        private TMPSkinData currencyBarLabel;
        public TMPSkinData CurrencyBarLabel => currencyBarLabel;

        [TabGroup("Common")]
        [Title("Result Currency Bar", subtitle: "Key: result_currency_bar", titleAlignment: TitleAlignments.Split)]
        [SerializeField]
        [HideLabel]
        private PanelSkinData resultCurrencyBar;
        public PanelSkinData ResultCurrencyBar => resultCurrencyBar;

        [TabGroup("Common")]
        [Title("Result Currency Bar Label", subtitle: "Key: result_currency_bar_label", titleAlignment: TitleAlignments.Split)]
        [SerializeField]
        [HideLabel]
        private TMPSkinData resultCurrencyBarLabel;
        public TMPSkinData ResultCurrencyBarLabel => resultCurrencyBarLabel;
        #endregion

        [Space(30)]
        
        [TabGroup("Common")]
        [Title("High Score MainMenu Label", subtitle: "Key: high_score_main_label", titleAlignment: TitleAlignments.Split)]
        [SerializeField]
        [HideLabel]
        private TMPSkinData highScoreMainMenuLabel;
        public TMPSkinData HighScoreMainMenuLabel => highScoreMainMenuLabel;

        [TabGroup("Common")]
        [Title("High Score MainMenu", subtitle: "Key: high_score_main", titleAlignment: TitleAlignments.Split)]
        [SerializeField]
        [HideLabel]
        private TMPSkinData highScoreMainMenu;
        public TMPSkinData HighScoreMainMenu => highScoreMainMenu;

        [TabGroup("Common")]
        [Title("High Score Result", subtitle: "Key: high_score_result", titleAlignment: TitleAlignments.Split)]
        [SerializeField]
        [HideLabel]
        private TMPSkinData highScoreResult;
        public TMPSkinData HighScoreResult => highScoreResult;

        private Dictionary<string, SkinData> skinLookup;

        public void UpdateLookupData()
        {
            skinLookup = new Dictionary<string, SkinData>();

            skinLookup.Add("", closeButton);

            #region Dialogs
            skinLookup.Add("header_panel", headerPanel);
            skinLookup.Add("header_label", headerLabel);
            skinLookup.Add("dialog_panel", dialogPanel);
            skinLookup.Add("dialog_label", dialogLabel);
            #endregion

            #region Common
            skinLookup.Add("currency_bar", currencyBar);
            skinLookup.Add("currency_bar_label", currencyBarLabel);
            skinLookup.Add("result_currency_bar", resultCurrencyBar);
            skinLookup.Add("result_currency_bar_label", resultCurrencyBarLabel);
            skinLookup.Add("high_score_main_label", highScoreMainMenuLabel);
            skinLookup.Add("high_score_main", highScoreMainMenu);
            skinLookup.Add("high_score_result", highScoreResult);
            #endregion

            #region Buttons
            skinLookup.Add("close_button", closeButton);
            skinLookup.Add("big_button", bigButton);
            skinLookup.Add("big_button_label", bigButtonLabel);
            skinLookup.Add("common_button", commonButton);
            skinLookup.Add("common_button_label", commonButtonLabel);
            skinLookup.Add("positive_button", positiveButton);
            skinLookup.Add("positive_button_label", positiveButtonLabel);
            skinLookup.Add("negative_button", negativeButton);
            skinLookup.Add("negative_button_label", negativeButtonLabel);
            #endregion

            #region DailyLogin
            skinLookup.Add("login_small_reward", loginSmallReward);
            skinLookup.Add("login_big_reward", loginBigReward);
            skinLookup.Add("login_reward_day_label", loginRewardDayLabel);
            skinLookup.Add("login_reward_amount_label", loginRewardAmountLabel);
            skinLookup.Add("login_big_reward_day_label", loginBigRewardDayLabel);
            skinLookup.Add("login_big_reward_amount_label", loginBigRewardAmountLabel);
            #endregion

            #region Leaderboard
            skinLookup.Add("leaderboard_item", leaderboardItem);
            skinLookup.Add("leaderboard_display", leaderboardDisplay);
            skinLookup.Add("leaderboard_item_name_label", leaderboardItemNameLabel);
            skinLookup.Add("leaderboard_item_score_label", leaderboardItemScoreLabel);
            skinLookup.Add("leaderboard_item_tag", leaderboardTag);
            #endregion

            #region Settings
            skinLookup.Add("setting_lang_selection", settingLanguageSelection);
            skinLookup.Add("setting_lang_selection_label", settingLanguageSelectionLabel);
            skinLookup.Add("support_button", supportButton);
            skinLookup.Add("support_button_label", supportButtonLabel);
            #endregion

            #region Shop
            skinLookup.Add("shop_tab", shopTab);
            skinLookup.Add("shop_item", shopItem);
            skinLookup.Add("shop_item_name_label", shopItemNameLabel);
            skinLookup.Add("shop_item_price_label", shopItemPriceLabel);
            skinLookup.Add("shop_item_price_negative_label", shopItemPriceNegativeLabel);
            skinLookup.Add("shop_item_owned_label", shopItemOwnedLabel);
            #endregion
        }

        public void CleanUpLookupData()
        {
            skinLookup.Clear();
            skinLookup = null;
        }

        public T GetSkinData<T>(string skinKey) where T: SkinData
        {
            Debug.Assert(skinLookup.ContainsKey(skinKey), $"Skin key [{skinKey}] not found");
            return skinLookup[skinKey] as T;
        }
    }
}