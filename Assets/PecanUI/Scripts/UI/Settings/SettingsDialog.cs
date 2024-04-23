using Doozy.Runtime.Signals;
using Doozy.Runtime.UIManager;
using Doozy.Runtime.UIManager.Animators;
using Doozy.Runtime.UIManager.Components;
using HotPlay.Utilities;
using TMPro;
using UnityEngine;

namespace HotPlay.PecanUI.Settings
{
    public class SettingsDialog : BaseDialog
    {
        private const string signalCategory = "Settings";

        [SerializeField]
        private LanguageSelection languageSelection;

        [SerializeField]
        private UIToggle sfxToggle;

        [SerializeField]
        private UIToggle musicToggle;

        [SerializeField]
        private UIButton facebookButton;

        [SerializeField]
        private UIButton twitterButton;

        [SerializeField]
        private UIButton supportButton;

        [SerializeField]
        private UIButton privacyButton;

        [SerializeField]
        private UIButton closeButton;

        [SerializeField]
        private GameObject externalLinkPanel;

        [SerializeField]
        private UISelectableUIAnimator privacyPolicySelectableAnimator;

        [SerializeField]
        private TextMeshProUGUI playerIDLabel;

        [SerializeField]
        private TextMeshProUGUI versionLabel;

        private const string versionStringFormat = "V{0}";
        private const string playerIDStringFormat = "ID: {0}";

        private LanguageData[] languages;

        private bool prevMusicToggleValue;
        private bool prevSFXToggleValue;

        protected override void Awake()
        {
            base.Awake();

            prevMusicToggleValue = musicToggle.isOn;
            prevSFXToggleValue = sfxToggle.isOn;
        }

        private void Start()
        {
            languages = PecanServices.Instance.Configs.Languages;
            Debug.Assert(languages != null && languages.Length > 0, $"Support language missing data");

            languageSelection.IndexChanged += OnLanguageChange;

            var configs = PecanServices.Instance.Configs;

            closeButton.behaviours.AutoGetUnityEvent(UIBehaviour.Name.PointerLeftClick).AddListener(OnCloseButtonClicked);
            facebookButton.gameObject.SetActive(configs.ShowFacebookButton);
            facebookButton.behaviours.AutoGetUnityEvent(UIBehaviour.Name.PointerLeftClick).AddListener(OnFacebookButtonClicked);
            twitterButton.behaviours.AutoGetUnityEvent(UIBehaviour.Name.PointerLeftClick).AddListener(OnTwitterButtonClicked);
            twitterButton.gameObject.SetActive(configs.ShowTwitterButton);
            supportButton.behaviours.AutoGetUnityEvent(UIBehaviour.Name.PointerLeftClick).AddListener(OnSupportButtonClicked);
            supportButton.gameObject.SetActive(configs.ShowSupportButton);
            privacyButton.behaviours.AutoGetUnityEvent(UIBehaviour.Name.PointerLeftClick).AddListener(OnPrivacyButtonClicked);

            externalLinkPanel.SetActive(configs.ShowFacebookButton || configs.ShowSupportButton || configs.ShowTwitterButton);

            musicToggle.OnValueChangedCallback.AddListener(OnBGMToggleValueChanged);
            sfxToggle.OnValueChangedCallback.AddListener(OnSFXToggleValueChanged);

            versionLabel.text = string.Format(versionStringFormat, Application.version);
            PecanServices.Instance.Events.PlayerIDUpdate += OnPlayerIDUpdate;

#if !PECAN_NAVIGATOR
            privacyPolicySelectableAnimator.SetController(null);
#endif
        }

        private void OnPlayerIDUpdate(string id)
        {
            playerIDLabel.text = string.Format(playerIDStringFormat, id);
        }

        private void OnLanguageChange()
        {
        }

        protected override void OnShowing()
        {
            base.OnShowing();
            languageSelection.SetItems(languages, I2.Loc.LocalizationManager.CurrentLanguageCode);

            sfxToggle.isOn = PecanServices.Instance.PecanSoundManager.EnableSFX;
            musicToggle.isOn = PecanServices.Instance.PecanSoundManager.EnableBGM;
        }

        private void OnCloseButtonClicked()
        {
            PecanServices.Instance.Events.SettingsEventHandler.InvokeCloseButtonClicked();
        }

        private void OnBGMToggleValueChanged(bool status)
        {
            if (status != prevMusicToggleValue)
            {
                Signal.Send(signalCategory, "EnableBGM", status);
                prevMusicToggleValue = status;
            }
        }

        private void OnSFXToggleValueChanged(bool status)
        {
            if (status != prevSFXToggleValue)
            {
                Signal.Send(signalCategory, "EnableSFX", status);
                prevSFXToggleValue = status;
            }
        }

        private void OnFacebookButtonClicked()
        {
            OpenURL(PecanServices.Instance.Configs.FacebookURL);
            Signal.Send(signalCategory, "Facebook");
        }

        private void OnTwitterButtonClicked()
        {
            OpenURL(PecanServices.Instance.Configs.TwitterURL);
            Signal.Send(signalCategory, "Twitter");
        }

        private void OnSupportButtonClicked()
        {
            OpenURL(PecanServices.Instance.Configs.SupportURL);
            Signal.Send(signalCategory, "Support");
        }

        private void OnPrivacyButtonClicked()
        {
            OpenURL(PecanServices.Instance.Configs.PrivacyPolicyURL);
            Signal.Send(signalCategory, "PrivacyPolicy");
        }

        private void OpenURL(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                Application.OpenURL(url);
            }
        }
    }
}
