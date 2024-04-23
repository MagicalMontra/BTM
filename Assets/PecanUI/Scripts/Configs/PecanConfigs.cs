using System.Collections.Generic;
using System.Linq;
using HotPlay.DailyLogin;
using HotPlay.goPlay.Internal.Entity.Enum;
using HotPlay.PecanUI.Gameplay;
using HotPlay.PecanUI.Leaderboard;
using HotPlay.PecanUI.SceneLoader;
using HotPlay.PecanUI.Shop;
using UnityEngine;

namespace HotPlay.PecanUI.Configs
{
    [CreateAssetMenu(fileName = "PecanConfigs", menuName = "Pecan UI/Pecan Configs")]
    public class PecanConfigs : ScriptableObject
    {
        [Header("Scene Loader")]
        [SerializeField]
        private LoadingProgressor sceneLoadAnimation;

        public LoadingProgressor SceneLoadAnimation => sceneLoadAnimation;
        
        [Header("Daily Login")]
        [SerializeField]
        private DailyLoginRewardData dailyLogin = null!;
        public DailyLoginRewardData DailyLogin => dailyLogin;

        [Header("Advertisements")]
        [SerializeField]
        private DefaultImpressionTexture[] defaultImpressionTextures;

        [Header("Shop")]
        [SerializeField]
        private bool isEnableCharacterTab = true;
        public bool IsEnableCharacterTab => isEnableCharacterTab;

        [SerializeField]
        private bool isEnableThemeTab = true;
        public bool IsEnableThemeTab => isEnableThemeTab;

        [SerializeField]
        private ItemType defaultShopCategory = ItemType.Character;
        public ItemType DefaultShopCategory => defaultShopCategory;

        [SerializeField]
        private Sprite shopTabCharacterImage = null!;
        public Sprite ShopTabCharacterImage => shopTabCharacterImage;

        [SerializeField]
        private Sprite shopTabThemeImage = null!;
        public Sprite ShopTabThemeImage => shopTabThemeImage;

        [SerializeField, Tooltip("Assign localize key for localization")]
        private string shopTabThemeLabel;
        public string ShopTabThemeLabel => shopTabThemeLabel;

        [SerializeField, Tooltip("Assign localize key for localization")]
        private string shopTabCharacterLabel;
        public string ShopTabCharacterLabel => shopTabCharacterLabel;

        [Header("External Links")]
        [SerializeField]
        private bool showFacebookButton;
        public bool ShowFacebookButton => showFacebookButton;

        [SerializeField]
        private string facebookURL;
        public string FacebookURL => facebookURL;

        [SerializeField]
        private bool showTwitterButton;
        public bool ShowTwitterButton => showTwitterButton;

        [SerializeField]
        private string twitterURL;
        public string TwitterURL => twitterURL;

        [SerializeField]
        private bool showSupportButton;
        public bool ShowSupportButton => showSupportButton;

        [SerializeField]
        private string supportURL;
        public string SupportURL => supportURL;

        [SerializeField]
        private string privacyPolicyURL;
        public string PrivacyPolicyURL => privacyPolicyURL;

        [Header("Tutorials")]
        [SerializeField]
        private TutorialPageData[] tutorialData;
        public TutorialPageData[] Tutorials => tutorialData;

        [Header("Localization")]
        [SerializeField]
        private LanguageData[] languages;
        public LanguageData[] Languages => languages;
        
        [SerializeField]
        private string tutorialCategory = "Tutorials/";
        public string TutorialCategory => tutorialCategory;
        
        [SerializeField]
        private string shopItemCategory = "ShopItems/";
        public string ShopItemCategory => shopItemCategory;

        [Header("Leaderboard")]
        [SerializeField]
        private ResultLeaderboardType resultLeaderboardType;
        public ResultLeaderboardType ResultLeaderboardType => resultLeaderboardType;

        [SerializeField]
        private LeaderboardDataProvider leaderboardDataProvider;
        public LeaderboardDataProvider LeaderboardDataProvider => leaderboardDataProvider;


        [SerializeField]
        private BaseCustomGameplayPanel gameplayUIPrefab;
        public BaseCustomGameplayPanel GameplayUIPrefab => gameplayUIPrefab;

        [SerializeField]
        private GameObject gameTitlePrefab;
        public GameObject GameTitlePrefab => gameTitlePrefab;

        [Header("UI Config")]
        [SerializeField]
        private float scrollSensitivity = 100f;
        public float ScrollSensitivity => scrollSensitivity;

        private Dictionary<AdRatio, Texture2D> defaultImpresionLookUp;

        public Texture2D GetDefaultImpression(AdRatio ratio)
        {
            if (defaultImpresionLookUp == null)
            {
                defaultImpresionLookUp = defaultImpressionTextures.ToDictionary((data) => data.Ratio, (data) => data.DefaultTexture);
            }

            return defaultImpresionLookUp.ContainsKey(ratio) ? defaultImpresionLookUp[ratio] : null;
        }
    }
}
