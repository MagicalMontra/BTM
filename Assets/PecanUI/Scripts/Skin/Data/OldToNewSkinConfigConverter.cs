#if UNITY_EDITOR
using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace HotPlay.PecanUI.Skin
{
    [CreateAssetMenu(menuName = "Create OldToNewSkinConfigConverter", fileName = "OldToNewSkinConfigConverter", order = 0)]
    public class OldToNewSkinConfigConverter : ScriptableObject
    {
        public SkinConfigs oldSkinConfig;
        public PecanSkinConfigs skinConfig;
        
        public void Convert()
        {
            CovertCommon();
            ConvertButtons();
            ConvertDailyLogin();
            ConvertLeaderboard();
            ConvertSettings();
            ConvertShop();
        }
        
        #region Common

        private void CovertCommon()
        {
            ConvertPanel();
            CovertHeader();
            ConvertCurrencyBar();
            ConvertResultBar();
            ConvertHighScore();
        }
        
        private void ConvertPanel()
        {
            var bgColor = oldSkinConfig.DialogPanel.BackgroundColor;
            var outlineColor = oldSkinConfig.DialogPanel.OutlineColor;
            var label = new TMPSkinData{ FontAsset = oldSkinConfig.DialogLabel.FontAsset, FontColor = oldSkinConfig.DialogLabel.FontColor, PresetMaterial = oldSkinConfig.DialogLabel.PresetMaterial };

            skinConfig.TMPSkin.TryEdit("Label", label);
            skinConfig.ImageSkin.TryEdit("Panel/BG", new ImageSkinData{ Color = bgColor });
            skinConfig.ImageSkin.TryEdit("Panel/Outline", new ImageSkinData{ Color = outlineColor });
        }

        private void CovertHeader()
        {
            var bgColor = oldSkinConfig.HeaderPanel.BackgroundColor;
            var outlineColor = oldSkinConfig.HeaderPanel.OutlineColor;
            var label = new TMPSkinData
            {
                FontAsset = oldSkinConfig.HeaderLabel.FontAsset,
                FontColor = oldSkinConfig.HeaderLabel.FontColor,
                PresetMaterial = oldSkinConfig.HeaderLabel.PresetMaterial
            };
            
            skinConfig.TMPSkin.TryEdit("Label/Header", label);
            skinConfig.ImageSkin.TryEdit("Panel/Header/BG", new ImageSkinData{ Color = bgColor});
            skinConfig.ImageSkin.TryEdit("Panel/Header/Outline", new ImageSkinData{ Color = outlineColor});
        }

        private void ConvertCurrencyBar()
        {
            var bgColor = oldSkinConfig.CurrencyBar.BackgroundColor;
            var outlineColor = oldSkinConfig.CurrencyBar.OutlineColor;
            var label = new TMPSkinData
            {
                FontAsset = oldSkinConfig.CurrencyBarLabel.FontAsset,
                FontColor = oldSkinConfig.CurrencyBarLabel.FontColor,
                PresetMaterial = oldSkinConfig.CurrencyBarLabel.PresetMaterial
            };
            
            skinConfig.TMPSkin.TryEdit("Label/Subtext/Currency", label);
            
            skinConfig.ImageSkin.TryEdit("Capsule/BG", new ImageSkinData{ Color = bgColor});
            skinConfig.ImageSkin.TryEdit("Capsule/Outline", new ImageSkinData{ Color = outlineColor});
            
            skinConfig.ImageSkin.TryEdit("Capsule/BG/Currency", new ImageSkinData{ Color = bgColor});
            skinConfig.ImageSkin.TryEdit("Capsule/Outline/Currency", new ImageSkinData{ Color = outlineColor});
        }
        
        private void ConvertResultBar()
        {
            var bgColor = oldSkinConfig.ResultCurrencyBar.BackgroundColor;
            var outlineColor = oldSkinConfig.ResultCurrencyBar.OutlineColor;
            var label = new TMPSkinData
            { 
                FontAsset = oldSkinConfig.ResultCurrencyBarLabel.FontAsset,
                FontColor = oldSkinConfig.ResultCurrencyBarLabel.FontColor,
                PresetMaterial = oldSkinConfig.ResultCurrencyBarLabel.PresetMaterial 
            };
            
            skinConfig.TMPSkin.TryEdit("Label/Subtext/Currency/GameResult", label);
            skinConfig.ImageSkin.TryEdit("Capsule/BG/GameResult", new ImageSkinData{ Color = bgColor});
            skinConfig.ImageSkin.TryEdit("Capsule/Outline/GameResult", new ImageSkinData{ Color = outlineColor});
        }
        
        private void ConvertHighScore()
        {
            var highScoreMainMenuTitle = new TMPSkinData
            { 
                FontAsset = oldSkinConfig.HighScoreMainMenuLabel.FontAsset,
                FontColor = oldSkinConfig.HighScoreMainMenuLabel.FontColor,
                PresetMaterial = oldSkinConfig.HighScoreMainMenuLabel.PresetMaterial 
            };
            
            var highScoreMainMenuLabel = new TMPSkinData
            { 
                FontAsset = oldSkinConfig.HighScoreMainMenu.FontAsset,
                FontColor = oldSkinConfig.HighScoreMainMenu.FontColor,
                PresetMaterial = oldSkinConfig.HighScoreMainMenu.PresetMaterial 
            };
            
            var highScoreResult = new TMPSkinData
            { 
                FontAsset = oldSkinConfig.HighScoreResult.FontAsset,
                FontColor = oldSkinConfig.HighScoreResult.FontColor,
                PresetMaterial = oldSkinConfig.HighScoreResult.PresetMaterial 
            };
            
            skinConfig.TMPSkin.TryEdit("Label/Subheading/ScoreValue", highScoreResult);
            skinConfig.TMPSkin.TryEdit("Label/Subheading/Highscore/MainMenu", highScoreMainMenuLabel);
            skinConfig.TMPSkin.TryEdit("Label/Subheading/HighscoreTitle/MainMenu", highScoreMainMenuTitle);
        }

        #endregion
        
        #region Buttons

        private void ConvertButtons()
        {
            ConvertCloseButton();
            ConvertLargeButton();
            ConvertCommonButton();
            ConvertPositiveButton();
            ConvertNegativeButton();
        }
        
        private void ConvertCloseButton()
        {
            var stroke = new ImageSkinData { Color = oldSkinConfig.CloseButton.StrokeColor };
            var bottom = new ImageSkinData { Color = oldSkinConfig.CloseButton.BottomColor };
            var top = new ImageSkinData { Color = oldSkinConfig.CloseButton.TopColor };
            var highlight = new ImageSkinData { Color = oldSkinConfig.CloseButton.HighlightColor };
            var navigator = new ImageSkinData { Color = oldSkinConfig.CloseButton.NavigatorColor };
            var icon = new ImageSkinData { Color = oldSkinConfig.CloseButton.IconColor };
            
            skinConfig.ImageSkin.TryEdit("Button/Small/Stroke/Close", stroke);
            skinConfig.ImageSkin.TryEdit("Button/Small/Bottom/Close", bottom);
            skinConfig.ImageSkin.TryEdit("Button/Small/Top/Close", top);
            skinConfig.ImageSkin.TryEdit("Button/Small/Highlight/Close", highlight);
            skinConfig.ImageSkin.TryEdit("Button/Small/Highlighted/Close", navigator);
            skinConfig.ImageSkin.TryEdit("Button/Small/Icon/Close", icon);
        }
        
        private void ConvertLargeButton()
        {
            var stroke = new ImageSkinData { Color = oldSkinConfig.BigButton.StrokeColor };
            var bottom = new ImageSkinData { Color = oldSkinConfig.BigButton.BottomColor };
            var top = new ImageSkinData { Color = oldSkinConfig.BigButton.TopColor };
            var highlight = new ImageSkinData { Color = oldSkinConfig.BigButton.HighlightColor };
            var navigator = new ImageSkinData { Color = oldSkinConfig.BigButton.NavigatorColor };
            var label = new TMPSkinData { FontAsset = oldSkinConfig.BigButtonLabel.FontAsset, FontColor = oldSkinConfig.BigButtonLabel.FontColor, PresetMaterial = oldSkinConfig.BigButtonLabel.PresetMaterial };
            
            skinConfig.ImageSkin.TryEdit("Button/Large/Stroke", stroke);
            skinConfig.ImageSkin.TryEdit("Button/Large/Bottom", bottom);
            skinConfig.ImageSkin.TryEdit("Button/Large/Top", top);
            skinConfig.ImageSkin.TryEdit("Button/Large/Highlight", highlight);
            skinConfig.ImageSkin.TryEdit("Button/Large/Highlighted", navigator);
            skinConfig.TMPSkin.TryEdit("Label/Button/Large", label);
        }
        
        private void ConvertCommonButton()
        {
            var stroke = new ImageSkinData { Color = oldSkinConfig.CommonButton.StrokeColor };
            var bottom = new ImageSkinData { Color = oldSkinConfig.CommonButton.BottomColor };
            var top = new ImageSkinData { Color = oldSkinConfig.CommonButton.TopColor };
            var highlight = new ImageSkinData { Color = oldSkinConfig.CommonButton.HighlightColor };
            var navigator = new ImageSkinData { Color = oldSkinConfig.CommonButton.NavigatorColor };
            var label = new TMPSkinData
            {
                FontAsset = oldSkinConfig.CommonButtonLabel.FontAsset,
                FontColor = oldSkinConfig.CommonButtonLabel.FontColor,
                PresetMaterial = oldSkinConfig.CommonButtonLabel.PresetMaterial
            };
            
            skinConfig.ImageSkin.TryEdit("Button/Circle/Stroke", stroke);
            skinConfig.ImageSkin.TryEdit("Button/Circle/Bottom", bottom);
            skinConfig.ImageSkin.TryEdit("Button/Circle/Top", top);
            skinConfig.ImageSkin.TryEdit("Button/Circle/Highlight", highlight);
            skinConfig.ImageSkin.TryEdit("Button/Circle/Highlighted", navigator);
            skinConfig.TMPSkin.TryEdit("Label/Button/Circle", label);
        }
        
        private void ConvertPositiveButton()
        {
            var stroke = new ImageSkinData { Color = oldSkinConfig.PositiveButton.StrokeColor };
            var bottom = new ImageSkinData { Color = oldSkinConfig.PositiveButton.BottomColor };
            var top = new ImageSkinData { Color = oldSkinConfig.PositiveButton.TopColor };
            var highlight = new ImageSkinData { Color = oldSkinConfig.PositiveButton.HighlightColor };
            var navigator = new ImageSkinData { Color = oldSkinConfig.PositiveButton.NavigatorColor };
            var icon = new ImageSkinData { Color = oldSkinConfig.PositiveButton.IconColor };
            var label = new TMPSkinData
            {
                FontAsset = oldSkinConfig.PositiveButtonLabel.FontAsset,
                FontColor = oldSkinConfig.PositiveButtonLabel.FontColor,
                PresetMaterial = oldSkinConfig.PositiveButtonLabel.PresetMaterial
            };
            
            skinConfig.ImageSkin.TryEdit("Button/Circle/Stroke/Right", stroke);
            skinConfig.ImageSkin.TryEdit("Button/Circle/Bottom/Right", bottom);
            skinConfig.ImageSkin.TryEdit("Button/Circle/Top/Right", top);
            skinConfig.ImageSkin.TryEdit("Button/Circle/Highlight/Right", highlight);
            skinConfig.ImageSkin.TryEdit("Button/Circle/Highlighted/Right", navigator);
            
            skinConfig.ImageSkin.TryEdit("Button/Small/Stroke/Right", stroke);
            skinConfig.ImageSkin.TryEdit("Button/Small/Bottom/Right", bottom);
            skinConfig.ImageSkin.TryEdit("Button/Small/Top/Right", top);
            skinConfig.ImageSkin.TryEdit("Button/Small/Highlight/Right", highlight);
            skinConfig.ImageSkin.TryEdit("Button/Small/Highlighted/Right", navigator);
            skinConfig.ImageSkin.TryEdit("Button/Small/Icon/Right", icon);
            
            skinConfig.TMPSkin.TryEdit("Label/Button/Circle/Right", label);
        }
        
        private void ConvertNegativeButton()
        {
            var stroke = new ImageSkinData { Color = oldSkinConfig.NegativeButton.StrokeColor };
            var bottom = new ImageSkinData { Color = oldSkinConfig.NegativeButton.BottomColor };
            var top = new ImageSkinData { Color = oldSkinConfig.NegativeButton.TopColor };
            var highlight = new ImageSkinData { Color = oldSkinConfig.NegativeButton.HighlightColor };
            var navigator = new ImageSkinData { Color = oldSkinConfig.NegativeButton.NavigatorColor };
            var icon = new ImageSkinData { Color = oldSkinConfig.NegativeButton.IconColor };
            var label = new TMPSkinData
            {
                FontAsset = oldSkinConfig.NegativeButtonLabel.FontAsset,
                FontColor = oldSkinConfig.NegativeButtonLabel.FontColor,
                PresetMaterial = oldSkinConfig.NegativeButtonLabel.PresetMaterial
            };
            
            skinConfig.ImageSkin.TryEdit("Button/Circle/Stroke/Left", stroke);
            skinConfig.ImageSkin.TryEdit("Button/Circle/Bottom/Left", bottom);
            skinConfig.ImageSkin.TryEdit("Button/Circle/Top/Left", top);
            skinConfig.ImageSkin.TryEdit("Button/Circle/Highlight/Left", highlight);
            skinConfig.ImageSkin.TryEdit("Button/Circle/Highlighted/Left", navigator);
            
            skinConfig.ImageSkin.TryEdit("Button/Small/Stroke/Left", stroke);
            skinConfig.ImageSkin.TryEdit("Button/Small/Bottom/Left", bottom);
            skinConfig.ImageSkin.TryEdit("Button/Small/Top/Left", top);
            skinConfig.ImageSkin.TryEdit("Button/Small/Highlight/Left", highlight);
            skinConfig.ImageSkin.TryEdit("Button/Small/Highlighted/Left", navigator);
            skinConfig.ImageSkin.TryEdit("Button/Small/Icon/Left", icon);
            
            skinConfig.TMPSkin.TryEdit("Label/Button/Circle/Left", label);
        }

        #endregion

        #region DailyLogin

        private void ConvertDailyLogin()
        {
            ConvertDailyTileSmall();
            ConvertDailyTileBig();
        }

        private void ConvertDailyTileSmall()
        {
            var stroke = new ImageSkinData { Color = oldSkinConfig.LoginSmallReward.StrokeColor };
            var bottom = new ImageSkinData { Color = oldSkinConfig.LoginSmallReward.BottomColor };
            var top = new ImageSkinData { Color = oldSkinConfig.LoginSmallReward.TopColor };
            var depth = new ImageSkinData { Color = oldSkinConfig.LoginSmallReward.DepthColor };
            var navigator = new ImageSkinData { Color = oldSkinConfig.LoginSmallReward.NavigatorColor };
            var head = new ImageSkinData { Color = oldSkinConfig.LoginSmallReward.DayBarColor };
            var dayLabel = new TMPSkinData
            {
                FontAsset = oldSkinConfig.LoginRewardDayLabel.FontAsset,
                FontColor = oldSkinConfig.LoginRewardDayLabel.FontColor,
                PresetMaterial = oldSkinConfig.LoginRewardDayLabel.PresetMaterial
            };
            var amountLabel = new TMPSkinData
            {
                FontAsset = oldSkinConfig.LoginRewardAmountLabel.FontAsset,
                FontColor = oldSkinConfig.LoginRewardAmountLabel.FontColor,
                PresetMaterial = oldSkinConfig.LoginRewardAmountLabel.PresetMaterial
            };
            
            skinConfig.ImageSkin.TryEdit("Tile/Bottom/Small/DailyReward", bottom);
            skinConfig.ImageSkin.TryEdit("Tile/Stroke/Small/DailyReward", stroke);
            skinConfig.ImageSkin.TryEdit("Tile/Top/Small/DailyReward", top);
            skinConfig.ImageSkin.TryEdit("Tile/Depth/Small/DailyReward", depth);
            skinConfig.ImageSkin.TryEdit("Tile/Highlight/Small/DailyReward", navigator);
            skinConfig.ImageSkin.TryEdit("Tile/Head/Small/DailyReward", head);
            skinConfig.TMPSkin.TryEdit("Label/Header/Tile/DailyReward", dayLabel);
            skinConfig.TMPSkin.TryEdit("Label/Subtext/Tile/Small/DailyReward", amountLabel);
        }

        private void ConvertDailyTileBig()
        {
            var stroke = new ImageSkinData { Color = oldSkinConfig.LoginBigReward.StrokeColor };
            var bottom = new ImageSkinData { Color = oldSkinConfig.LoginBigReward.BottomColor };
            var top = new ImageSkinData { Color = oldSkinConfig.LoginBigReward.TopColor };
            var depth = new ImageSkinData { Color = oldSkinConfig.LoginBigReward.DepthColor };
            var navigator = new ImageSkinData { Color = oldSkinConfig.LoginBigReward.NavigatorColor };
            var head = new ImageSkinData { Color = oldSkinConfig.LoginBigReward.DayBarColor };
            var dayLabel = new TMPSkinData
            {
                FontAsset = oldSkinConfig.LoginBigRewardDayLabel.FontAsset,
                FontColor = oldSkinConfig.LoginBigRewardDayLabel.FontColor,
                PresetMaterial = oldSkinConfig.LoginBigRewardDayLabel.PresetMaterial
            };
            var amountLabel = new TMPSkinData
            {
                FontAsset = oldSkinConfig.LoginBigRewardAmountLabel.FontAsset,
                FontColor = oldSkinConfig.LoginBigRewardAmountLabel.FontColor,
                PresetMaterial = oldSkinConfig.LoginBigRewardAmountLabel.PresetMaterial
            };
            
            skinConfig.ImageSkin.TryEdit("Tile/Stroke/Big/DailyReward", stroke);
            skinConfig.ImageSkin.TryEdit("Tile/Bottom/Big/DailyReward", bottom);
            skinConfig.ImageSkin.TryEdit("Tile/Top/Big/DailyReward", top);
            skinConfig.ImageSkin.TryEdit("Tile/Depth/Big/DailyReward", depth);
            skinConfig.ImageSkin.TryEdit("Tile/Highlight/Big/DailyReward", navigator);
            skinConfig.ImageSkin.TryEdit("Tile/Head/Big/DailyReward", head);
            skinConfig.TMPSkin.TryEdit("Label/Header/Tile/Big/DailyReward", dayLabel);
            skinConfig.TMPSkin.TryEdit("Label/Subtext/Tile/Big/DailyReward", amountLabel);
        }
        
        #endregion

        #region Leaderboard

        private void ConvertLeaderboard()
        {
            ConvertItem();
            ConvertDisplay();
            ConvertTag();
        }

        private void ConvertItem()
        {
            var bgColor = new ImageSkinData { Color = oldSkinConfig.LeaderboardItem.BackgroundColor };
            var outlineColor = new ImageSkinData { Color = oldSkinConfig.LeaderboardItem.OutlineColor };
            var nameLabel = new TMPSkinData
            {
                FontAsset = oldSkinConfig.LeaderboardItemNameLabel.FontAsset,
                FontColor = oldSkinConfig.LeaderboardItemNameLabel.FontColor,
                PresetMaterial = oldSkinConfig.LeaderboardItemNameLabel.PresetMaterial
            };
            var scoreLabel = new TMPSkinData
            {
                FontAsset = oldSkinConfig.LeaderboardItemScoreLabel.FontAsset,
                FontColor = oldSkinConfig.LeaderboardItemScoreLabel.FontColor,
                PresetMaterial = oldSkinConfig.LeaderboardItemScoreLabel.PresetMaterial
            };
            
            skinConfig.ImageSkin.TryEdit("PlayerRank/BG", bgColor);
            skinConfig.ImageSkin.TryEdit("PlayerRank/Outline", outlineColor);
            
            skinConfig.TMPSkin.TryEdit("Label/Subtext/Name/Leaderboard", nameLabel);
            skinConfig.TMPSkin.TryEdit("Label/Subtext/Score/Leaderboard", scoreLabel);
        }

        private void ConvertDisplay()
        {
            var bgColor = new ImageSkinData { Color = oldSkinConfig.LeaderboardDisplay.BackgroundColor };
            var outlineColor = new ImageSkinData { Color = oldSkinConfig.LeaderboardDisplay.OutlineColor };

            skinConfig.ImageSkin.TryEdit("PlayerRank/BG/Image", bgColor);
            skinConfig.ImageSkin.TryEdit("PlayerRank/Outline/Image", outlineColor);
        }

        private void ConvertTag()
        {
            var outline = new ImageSkinData { Color = oldSkinConfig.LeaderboardTag.OutlineColor };
            var bottom = new ImageSkinData { Color = oldSkinConfig.LeaderboardTag.BottomColor };
            var top = new ImageSkinData { Color = oldSkinConfig.LeaderboardTag.TopColor };
            var depth = new ImageSkinData { Color = oldSkinConfig.LeaderboardTag.DepthColor };
            
            var rankLabel = new TMPSkinData
            {
                FontAsset = oldSkinConfig.LeaderboardTag.Label.FontAsset,
                FontColor = oldSkinConfig.LeaderboardTag.Label.FontColor,
                PresetMaterial = oldSkinConfig.LeaderboardTag.Label.PresetMaterial
            };
            
            skinConfig.ImageSkin.TryEdit("PlayerRank/Tag/Outline", outline);
            skinConfig.ImageSkin.TryEdit("PlayerRank/Tag/Bottom", bottom);
            skinConfig.ImageSkin.TryEdit("PlayerRank/Tag/Top", top);
            skinConfig.ImageSkin.TryEdit("PlayerRank/Tag/Depth", depth);
            
            skinConfig.TMPSkin.TryEdit("Label/PlayerRank", rankLabel);
        }

        #endregion

        #region Settings

        private void ConvertSettings()
        {
            ConvertLanguage();
            ConvertSupport();
        }

        private void ConvertLanguage()
        {
            var bgColor = new ImageSkinData { Color = oldSkinConfig.SettingLanguageSelection.BackgroundColor };
            var outlineColor = new ImageSkinData { Color = oldSkinConfig.SettingLanguageSelection.OutlineColor };
            var selectionLabel = new TMPSkinData
            {
                FontAsset = oldSkinConfig.SettingLanguageSelectionLabel.FontAsset,
                FontColor = oldSkinConfig.SettingLanguageSelectionLabel.FontColor,
                PresetMaterial = oldSkinConfig.SettingLanguageSelectionLabel.PresetMaterial
            };

            skinConfig.ImageSkin.TryEdit("Capsule/BG/Language/Settings", bgColor);
            skinConfig.ImageSkin.TryEdit("Capsule/Outline/Language/Settings", outlineColor);
            
            skinConfig.TMPSkin.TryEdit("Label/Subtext/Language/Settings", selectionLabel);
        }
        
        private void ConvertSupport()
        {
            var stroke = new ImageSkinData { Color = oldSkinConfig.SupportButton.StrokeColor };
            var bottom = new ImageSkinData { Color = oldSkinConfig.SupportButton.BottomColor };
            var top = new ImageSkinData { Color = oldSkinConfig.SupportButton.TopColor };
            var highlight = new ImageSkinData { Color = oldSkinConfig.SupportButton.HighlightColor };
            var navigator = new ImageSkinData { Color = oldSkinConfig.SupportButton.NavigatorColor };
            var icon = new ImageSkinData { Color = oldSkinConfig.SupportButton.IconColor };
            
            var supportButtonLabel = new TMPSkinData
            {
                FontAsset = oldSkinConfig.SupportButtonLabel.FontAsset,
                FontColor = oldSkinConfig.SupportButtonLabel.FontColor,
                PresetMaterial = oldSkinConfig.SupportButtonLabel.PresetMaterial
            };

            skinConfig.ImageSkin.TryEdit("Button/External/Stroke/Support", stroke);
            skinConfig.ImageSkin.TryEdit("Button/External/Outline/Support", bottom);
            skinConfig.ImageSkin.TryEdit("Button/External/Top/Support", top);
            skinConfig.ImageSkin.TryEdit("Button/External/Highlight/Support", highlight);
            skinConfig.ImageSkin.TryEdit("Button/External/Highlighted/Support", navigator);
            skinConfig.ImageSkin.TryEdit("Button/External/Logo/Support", icon);
            skinConfig.TMPSkin.TryEdit("Label/Button/External/Facebook", supportButtonLabel);
        }

        #endregion

        #region Shop

        private void ConvertShop()
        {
            ConvertShopTab();
            ConvertShopItem();
        }

        private void ConvertShopTab()
        {
            var stroke = new ImageSkinData { Color = oldSkinConfig.ShopTab.StrokeColor };
            var bottom = new ImageSkinData { Color = oldSkinConfig.ShopTab.BottomColor };
            var top = new ImageSkinData { Color = oldSkinConfig.ShopTab.TopColor };
            var depth = new ImageSkinData { Color = oldSkinConfig.ShopTab.DepthColor };
            var navigator = new ImageSkinData { Color = oldSkinConfig.ShopTab.NavigatorColor };
            var barBlack = new ImageSkinData { Color = oldSkinConfig.ShopTab.TabBorderColor };
            var barWhite = new ImageSkinData { Color = oldSkinConfig.ShopTab.TabActiveBorderColor };
            var icon = new ImageSkinData { Color = oldSkinConfig.ShopTab.IconColor };
            var label = new TMPSkinData
            {
                FontColor = oldSkinConfig.ShopTab.TabLabel.FontColor,
                FontAsset = oldSkinConfig.ShopTab.TabLabel.FontAsset,
                PresetMaterial = oldSkinConfig.ShopTab.TabLabel.PresetMaterial
            };
            
            skinConfig.ImageSkin.TryEdit("Button/Tab/Stroke", stroke);
            skinConfig.ImageSkin.TryEdit("Button/Tab/Bottom", bottom);
            skinConfig.ImageSkin.TryEdit("Button/Tab/Top", top);
            skinConfig.ImageSkin.TryEdit("Button/Tab/Depth", depth);
            skinConfig.ImageSkin.TryEdit("Button/Tab/Icon", icon);
            skinConfig.ImageSkin.TryEdit("Button/Tab/Highlight", navigator);
            skinConfig.ImageSkin.TryEdit("Button/Tab/Bar/Shade", barBlack);
            skinConfig.ImageSkin.TryEdit("Button/Tab/Bar/White", barWhite);
            
            skinConfig.TMPSkin.TryEdit("Label/Button/Tab", label);
        }
        
        private void ConvertShopItem()
        {
            var stroke = new ImageSkinData { Color = oldSkinConfig.ShopItem.StrokeColor };
            var bottom = new ImageSkinData { Color = oldSkinConfig.ShopItem.BottomColor };
            var top = new ImageSkinData { Color = oldSkinConfig.ShopItem.TopColor };
            var depth = new ImageSkinData { Color = oldSkinConfig.ShopItem.DepthColor };
            var navigator = new ImageSkinData { Color = oldSkinConfig.ShopItem.NavigatorColor };
            var head = new ImageSkinData { Color = oldSkinConfig.ShopItem.NameBackground };
            var foot = new ImageSkinData { Color = oldSkinConfig.ShopItem.PriceBackground };
            var itemNameLabel = new TMPSkinData
            {
                FontColor = oldSkinConfig.ShopItemNameLabel.FontColor,
                FontAsset = oldSkinConfig.ShopItemNameLabel.FontAsset,
                PresetMaterial = oldSkinConfig.ShopItemNameLabel.PresetMaterial
            };
            var itemPriceLabel = new TMPSkinData
            {
                FontColor = oldSkinConfig.ShopItemPriceLabel.FontColor,
                FontAsset = oldSkinConfig.ShopItemPriceLabel.FontAsset,
                PresetMaterial = oldSkinConfig.ShopItemPriceLabel.PresetMaterial
            };
            var itemPriceNegativeLabel = new TMPSkinData
            {
                FontColor = oldSkinConfig.ShopItemPriceNegativeLabel.FontColor,
                FontAsset = oldSkinConfig.ShopItemPriceNegativeLabel.FontAsset,
                PresetMaterial = oldSkinConfig.ShopItemPriceNegativeLabel.PresetMaterial
            };
            var itemOwnedLabel = new TMPSkinData
            {
                FontColor = oldSkinConfig.ShopItemOwnedLabel.FontColor,
                FontAsset = oldSkinConfig.ShopItemOwnedLabel.FontAsset,
                PresetMaterial = oldSkinConfig.ShopItemOwnedLabel.PresetMaterial
            };
            
            skinConfig.ImageSkin.TryEdit("Tile/Stroke/Small/Shop", stroke);
            skinConfig.ImageSkin.TryEdit("Tile/Bottom/Small/Shop", bottom);
            skinConfig.ImageSkin.TryEdit("Tile/Top/Small/Shop", top);
            skinConfig.ImageSkin.TryEdit("Tile/Depth/Small/Shop", depth);
            skinConfig.ImageSkin.TryEdit("Tile/Highlight/Small/Shop", navigator);
            skinConfig.ImageSkin.TryEdit("Tile/Head/Small/Shop", head);
            skinConfig.ImageSkin.TryEdit("Tile/Foot/Small/Shop", foot);
            
            skinConfig.ImageSkin.TryEdit("Tile/Stroke/Big/Shop", stroke);
            skinConfig.ImageSkin.TryEdit("Tile/Bottom/Big/Shop", bottom);
            skinConfig.ImageSkin.TryEdit("Tile/Top/Big/Shop", top);
            skinConfig.ImageSkin.TryEdit("Tile/Depth/Big/Shop", depth);
            skinConfig.ImageSkin.TryEdit("Tile/Highlight/Big/Shop", navigator);
            skinConfig.ImageSkin.TryEdit("Tile/Head/Big/Shop", head);
            skinConfig.ImageSkin.TryEdit("Tile/Foot/Big/Shop", foot);
            
            skinConfig.TMPSkin.TryEdit("Label/Header/Tile/Shop", itemNameLabel);
            skinConfig.TMPSkin.TryEdit("Label/Subtext/Price/Positive", itemPriceLabel);
            skinConfig.TMPSkin.TryEdit("Label/Subtext/Price/Negative", itemPriceNegativeLabel);
            skinConfig.TMPSkin.TryEdit("Label/Subtext/Owned", itemOwnedLabel);
            Debug.Log(itemOwnedLabel.FontColor);
        }

        #endregion
    }
}
#endif