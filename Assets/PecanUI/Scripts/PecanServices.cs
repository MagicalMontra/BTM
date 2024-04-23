using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Doozy.Runtime.UIManager.Components;
using HotPlay.DailyLogin;
using HotPlay.goPlay.Internal.Config;
using HotPlay.goPlay.Internal.Entity.Enum;
using HotPlay.goPlay.Services.Ads;
using HotPlay.goPlay.Services.Rewards.UI;
using HotPlay.PecanUI.Analytic;
using HotPlay.PecanUI.Configs;
using HotPlay.PecanUI.Events;
using HotPlay.PecanUI.Gameplay;
using HotPlay.PecanUI.Shop;
using HotPlay.PecanUI.Skin;
using HotPlay.PecanUI.Sound;
using HotPlay.Sound;
using UnityEngine;
using UnityEngine.UI;
using ItemType = HotPlay.PecanUI.Shop.ItemType;

namespace HotPlay.PecanUI
{
    public class PecanServices : Utilities.MonoSingleton<PecanServices>
    {
        [SerializeField]
        private bool needSceneChanged;
        public bool NeedSceneChanged => needSceneChanged;
        
        [SerializeField]
        private PecanConfigs configs;
        public PecanConfigs Configs => configs;

        [SerializeField]
        private PecanSkinConfigs skinConfigs;
        public PecanSkinConfigs SkinConfigs => skinConfigs;

        [SerializeField]
        private PecanSoundConfigs soundConfigs;

        [SerializeField]
        private TopBar topBar;
        public TopBar TopBar => topBar;

        [SerializeField]
        private Signals signals;
        public Signals Signals => signals;

        [SerializeField]
        private SignalProcessor signalProcessor;
        public SignalProcessor SignalProcessor => signalProcessor;

        [SerializeField]
        private List<BaseDialog> dialogs = new List<BaseDialog>();

        [SerializeField]
        private EventsHandler events;
        public EventsHandler Events => events;

        [SerializeField]
        private SoundManager soundManager;
        public SoundManager SoundManager => soundManager;

        [SerializeField]
        private PecanSessionTimer sessionTimer;
        public PecanSessionTimer SessionTimer => sessionTimer;
        
        
        public PecanAnalytic Analytic { get; private set; }
        public DailyLoginManager DailyLoginManager { get; private set; }
        public PecanSoundManager PecanSoundManager { get; private set; }

        private Canvas canvas;
        private CanvasScaler canvasScaler;

        /// <summary>
        /// Default file path of pecan configs
        /// </summary>
        private string defaultConfigsPath = "PecanConfigs";

        /// <summary>
        /// Default file path of pecan skin configs
        /// </summary>
        private string defaultSkinConfigsPath = "PecanSkinConfigs";

        private string defaultSoundConfigsPath = "PecanSoundConfigs";

        /// <summary>
        /// Raise event once background UI changed
        /// </summary>
        public event Action<Sprite> BackgroundUIChanged;

        /// <summary>
        /// Nullable if background never change
        /// </summary>
        public Sprite LatestBackgroundUI { get; private set; }

        public string PlayerID { get; private set; }

        private BaseDialog currentDialog;

        public int HighScore { get; private set; }

        public BaseCustomGameplayPanel CustomGameplayPanel { get; private set; }

        private TutorialData[] cachedTutorialsData;

        public override void Awake()
        {
            base.Awake();

            canvas = GetComponent<Canvas>();
            canvasScaler = GetComponent<CanvasScaler>();

            // Try Load config from default path
            LoadConfig(defaultConfigsPath, ref configs);
            LoadConfig(defaultSkinConfigsPath, ref skinConfigs);
            LoadConfig(defaultSoundConfigsPath, ref soundConfigs);

            Analytic = new PecanAnalytic();
            DailyLoginManager = new DailyLoginManager(configs.DailyLogin.Rewards);
            PecanSoundManager = new PecanSoundManager(soundManager, events, soundConfigs.Configs);

            foreach (var dialog in dialogs)
            {
                dialog.Visible += OnVisible;
            }

            foreach (var scrollRect in GetComponentsInChildren<ScrollRect>())
            {
                scrollRect.scrollSensitivity = configs.ScrollSensitivity;
            }

            InitAdvertisement();

            events.PlayerIDUpdate += OnPlayerIDUpdate;
        }

        /// <summary>
        /// Change main game's background UI
        /// </summary>
        public void ChangeBackgroundUI(Sprite sprite)
        {
            LatestBackgroundUI = sprite;
            BackgroundUIChanged?.Invoke(sprite);
        }

        public T GetDialog<T>() where T : BaseDialog
        {
            foreach (var dialog in dialogs)
            {
                if (dialog.GetType() == typeof(T))
                {
                    return dialog as T;
                }
            }

            return null;
        }

        /// <summary>
        /// Reocevery navigator when it's lost selection. (Ex, Subscribe this to "submit" input key).
        /// </summary>
        public void RecoveryNavigator()
        {
            if (currentDialog != null)
            {
                currentDialog.TryRecoveryNavigator();
            }
        }

        public void CreateCustomGameplayPanel(Transform parent)
        {
            if (configs.GameplayUIPrefab != null && CustomGameplayPanel == null)
            {
                CustomGameplayPanel = Instantiate(configs.GameplayUIPrefab, parent, false);
            }
        }

        public T GetCustomGamePlayPanel<T>() where T : BaseCustomGameplayPanel
        {
            if (CustomGameplayPanel != null)
            {
                return CustomGameplayPanel as T;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Get current platform tutorials
        /// </summary>
        public TutorialData[] GetTutorialData()
        {
            if (cachedTutorialsData != null)
            {
                return cachedTutorialsData;
            }

            if (configs.Tutorials != null && configs.Tutorials.Length > 0)
            {
                cachedTutorialsData = configs.Tutorials.Select(x => x.GetCurrentPlatformTutorial()).ToArray();
                return cachedTutorialsData;
            }

            Debug.LogAssertion("Missing tutorial data");

            return cachedTutorialsData;
        }

        /// <summary>
        /// Get current platform tutorials
        /// </summary>
        /// <param name="group">tutorials' group</param>
        public TutorialData[] GetTutorialsByGroup(string group)
        {
            var groups = configs.Tutorials.Where(x => x.Group == group);
            return groups.Select(x => x.GetCurrentPlatformTutorial()).ToArray();
        }

        public float GetCurrentCanvasScale()
        {
            return canvas.scaleFactor;
        }

        private void InitAdvertisement()
        {
#if PECAN_ENABLE_IMPRESSION
            GoPlaySettings.Instance.EnableAdPlacement = true;
#elif PECAN_DISABLE_IMPRESSION
            GoPlaySettings.Instance.EnableAdPlacement = false;
#endif
#if PECAN_ENABLE_COUPON_REWARD
            GoPlaySettings.Instance.EnableRewardSystem = true;
#elif PECAN_DISABLE_COUPON_REWARD
            GoPlaySettings.Instance.EnableRewardSystem = false;
#endif

            // Setup all impressions
            var adPlacements = FindObjectsOfType<AdPlacement>(true);
            if (adPlacements != null)
            {
                foreach (var adPlacement in adPlacements)
                {
                    AdPlacementUGUI adUI = adPlacement as AdPlacementUGUI;
                    AdPlacementRenderer adWorld = adPlacement as AdPlacementRenderer;

                    AdRatio? ratio = adPlacement.AdRatio;
                    if (ratio.HasValue)
                    {
                        var defaultTexture = configs.GetDefaultImpression(ratio.Value);

                        if (adWorld != null)
                        {
                            adWorld.SetDefaultTexture2D(defaultTexture);
                        }
                        else if (adUI != null)
                        {
                            adUI.SetDefaultTexture2D(defaultTexture);
                        }
                        else
                        {
                            Debug.LogAssertion($"adObject miss matched");
                        }
                    }
                }
            }

            // Setup all coupon redeem buttons
            var redeemButtons = FindObjectsOfType<ShowRewardButton>(true);
            if (redeemButtons != null)
            {
                foreach (var button in redeemButtons)
                {
                    button.GetComponent<UIButton>().interactable = GoPlaySettings.Instance.EnableRewardSystem;
                }
            }
        }

        private void OnVisible(BaseDialog target)
        {
            currentDialog = target;

            foreach (var dialog in dialogs)
            {
                dialog.SetCanvasGroupInteractable(dialog == target);
            }
        }

        private void OnPlayerIDUpdate(string id)
        {
            PlayerID = id;
        }

        private void LoadConfig<T>(string path, ref T configs) where T : UnityEngine.Object
        {
            if (configs == null)
            {
                configs = Resources.Load<T>(path);
                Debug.Assert(configs != null, $"Try load default config for {typeof(T).Name} but not found [Resources/{defaultConfigsPath}]");
            }
        }

        private void Update()
        {
#if UNITY_EDITOR && PECAN_NAVIGATOR && PECAN_DEVELOPMENT
            if (Input.GetKeyDown(KeyCode.Return))
            {
                RecoveryNavigator();
            }
#endif
        }

        #region Require Function
        private Func<string, bool> isOwnedCharacterFunc = null;
        private Func<string, bool> isEquippedCharacterFunc = null;

        private Func<string, bool> isOwnedThemeFunc = null;
        private Func<string, bool> isEquippedThemeValidateFunc = null;

        private Func<int, bool> isEnoughCurrencyFunc = null;

        private Func<ShopElementData[]> getCharacterShopItemsFunc = null;
        private Func<ShopElementData[]> getThemeShopItemsFunc = null;

        private Func<int, bool> isNewHighScoreFunc = null;
        private Func<int> getHighScoreFunc = null;

        public bool IsOwned(string id, ItemType shopCategory)
        {
            switch (shopCategory)
            {
                case ItemType.Character:
                    return isOwnedCharacterFunc == null ? false : isOwnedCharacterFunc(id);
                case ItemType.Theme:
                    return isOwnedThemeFunc == null ? false : isOwnedThemeFunc(id);
                default:
                    return false;
            }
        }

        public bool IsEquipped(string id, ItemType shopCategory)
        {
            switch (shopCategory)
            {
                case ItemType.Character:
                    return isEquippedCharacterFunc == null ? false : isEquippedCharacterFunc(id);
                case ItemType.Theme:
                    return isEquippedThemeValidateFunc == null ? false : isEquippedThemeValidateFunc(id);
                default:
                    return false;
            }
        }

        public bool IsEnoughCurrency(int price)
        {
            return isEnoughCurrencyFunc == null ? false : isEnoughCurrencyFunc(price);
        }

        public ShopElementData[] GetCharacterShopItems()
        {
            return getCharacterShopItemsFunc == null ? null : getCharacterShopItemsFunc();
        }

        public ShopElementData[] GetThemeShopItems()
        {
            return getThemeShopItemsFunc == null ? null : getThemeShopItemsFunc();
        }

        public ShopElementData[] GetShopItems(ItemType category)
        {
            switch (category)
            {
                case ItemType.Character:
                    return GetCharacterShopItems();
                case ItemType.Theme:
                    return GetThemeShopItems();
                default:
                    return null;
            }
        }

        public int GetHighScore()
        {
            return getHighScoreFunc == null ? 0 : getHighScoreFunc();
        }

        public bool IsNewHighScore(int value)
        {
            return isNewHighScoreFunc == null ? false : isNewHighScoreFunc(value);
        }

        public void SetIsOwnedCharacterFunction(Func<string, bool> func)
        {
            isOwnedCharacterFunc = func;
        }

        public void SetIsEquippedCharacterFunction(Func<string, bool> func)
        {
            isEquippedCharacterFunc = func;
        }

        public void SetIsOwnedThemeFunction(Func<string, bool> func)
        {
            isOwnedThemeFunc = func;
        }

        public void SetIsEquippedThemeFunction(Func<string, bool> func)
        {
            isEquippedThemeValidateFunc = func;
        }

        public void SetIsEnoughCurrencyFunction(Func<int, bool> func)
        {
            isEnoughCurrencyFunc = func;
        }

        public void SetGetCharacterShopItemsFunction(Func<ShopElementData[]> func)
        {
            getCharacterShopItemsFunc = func;
        }

        public void SetGetThemeShopItemsFunction(Func<ShopElementData[]> func)
        {
            getThemeShopItemsFunc = func;
        }

        public void SetIsNewHighScoreFunction(Func<int, bool> func)
        {
            isNewHighScoreFunc = func;
        }

        public void SetGetHighScoreFunction(Func<int> func)
        {
            getHighScoreFunc = func;
        }
        #endregion
    }
}
