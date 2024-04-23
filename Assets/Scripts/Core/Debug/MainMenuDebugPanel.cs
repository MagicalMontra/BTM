using Doozy.Runtime.UIManager;
using Doozy.Runtime.UIManager.Components;
using HotPlay.PecanUI;
using HotPlay.Utilities;
using TMPro;
using UnityEngine;
using Zenject;

namespace HotPlay.BoosterMath.Core
{
    public class MainMenuDebugPanel : MonoBehaviour
    {
#if CHEAT_ENABLED
        [SerializeField]
        private GameObject contentObject;
        
        [SerializeField]
        private TMP_InputField coinField;
        
        [SerializeField]
        private TMP_InputField dayField;

        [SerializeField]
        private UIButton addCoinButton;
        
        [SerializeField]
        private UIButton addDayButton;
        
        [SerializeField]
        private UIButton decreaseCoinButton;
        
        [SerializeField]
        private UIButton increaseCoinButton;
        
        [SerializeField]
        private UIButton decreaseDayButton;
        
        [SerializeField]
        private UIButton increaseDayButton;
        
        [SerializeField]
        private UIButton resetShopButton;
        
        [SerializeField]
        private UIButton resetHighScoreButton;
        
        [SerializeField]
        private UIButton resetTutorialButton;
        
        [SerializeField]
        private UIButton closeButton;
        
        [SerializeField]
        private UIButton resetAllButton;

        [Inject]
        private PecanServices pecanServices;
        
        [Inject]
        private TutorialController tutorialController;
        
        [Inject]
        private ShopDataController shopDataController;

        [Inject]
        private CurrencyDataController currencyDataController;
        
        private const string highScoreKey = "HighScore";
        
        private void Awake()
        {
            coinField.text = 1000.ToString();
            dayField.text = 1.ToString();
            Bind();
        }

        private void OnDestroy()
        {
            Unbind();
        }

        private void Bind()
        {
            closeButton.behaviours.AutoGetUnityEvent(UIBehaviour.Name.PointerClick).AddListener(Close);
            increaseCoinButton.behaviours.AutoGetUnityEvent(UIBehaviour.Name.PointerClick).AddListener(IncreaseCoinCounter);
            decreaseCoinButton.behaviours.AutoGetUnityEvent(UIBehaviour.Name.PointerClick).AddListener(DecreaseCoinCounter);
            increaseDayButton.behaviours.AutoGetUnityEvent(UIBehaviour.Name.PointerClick).AddListener(IncreaseDayCounter);
            decreaseDayButton.behaviours.AutoGetUnityEvent(UIBehaviour.Name.PointerClick).AddListener(DecreaseDayCounter);
            addCoinButton.behaviours.AutoGetUnityEvent(UIBehaviour.Name.PointerClick).AddListener(AddCoin);
            addDayButton.behaviours.AutoGetUnityEvent(UIBehaviour.Name.PointerClick).AddListener(AddDay);
            resetShopButton.behaviours.AutoGetUnityEvent(UIBehaviour.Name.PointerClick).AddListener(ResetShop);
            resetHighScoreButton.behaviours.AutoGetUnityEvent(UIBehaviour.Name.PointerClick).AddListener(ResetHighScore);
            resetTutorialButton.behaviours.AutoGetUnityEvent(UIBehaviour.Name.PointerClick).AddListener(ResetTutorial);
            resetAllButton.behaviours.AutoGetUnityEvent(UIBehaviour.Name.PointerClick).AddListener(ResetAll);
        }

        private void Unbind()
        {
            closeButton.behaviours.AutoGetUnityEvent(UIBehaviour.Name.PointerClick).RemoveListener(Close);
            increaseCoinButton.behaviours.AutoGetUnityEvent(UIBehaviour.Name.PointerClick).RemoveListener(IncreaseCoinCounter);
            decreaseCoinButton.behaviours.AutoGetUnityEvent(UIBehaviour.Name.PointerClick).RemoveListener(DecreaseCoinCounter);
            increaseDayButton.behaviours.AutoGetUnityEvent(UIBehaviour.Name.PointerClick).RemoveListener(IncreaseDayCounter);
            decreaseDayButton.behaviours.AutoGetUnityEvent(UIBehaviour.Name.PointerClick).RemoveListener(DecreaseDayCounter);
            addCoinButton.behaviours.AutoGetUnityEvent(UIBehaviour.Name.PointerClick).RemoveListener(AddCoin);
            addDayButton.behaviours.AutoGetUnityEvent(UIBehaviour.Name.PointerClick).RemoveListener(AddDay);
            resetShopButton.behaviours.AutoGetUnityEvent(UIBehaviour.Name.PointerClick).RemoveListener(ResetShop);
            resetHighScoreButton.behaviours.AutoGetUnityEvent(UIBehaviour.Name.PointerClick).RemoveListener(ResetHighScore);
            resetTutorialButton.behaviours.AutoGetUnityEvent(UIBehaviour.Name.PointerClick).RemoveListener(ResetTutorial);
            resetAllButton.behaviours.AutoGetUnityEvent(UIBehaviour.Name.PointerClick).RemoveListener(ResetAll);
        }

        private void Close()
        {
            contentObject.SetActive(false);
        }
        
        private void DecreaseCoinCounter()
        {
            var count = 0;
            int.TryParse(coinField.text, out count);
            count = Mathf.Clamp(count - 1000, 1000, int.MaxValue);
            coinField.text = count.ToString();
        }

        private void IncreaseCoinCounter()
        {
            var count = 0;
            int.TryParse(coinField.text, out count);
            count += 1000;
            coinField.text = count.ToString();
        }
        
        private void DecreaseDayCounter()
        {
            var count = 0;
            int.TryParse(dayField.text, out count);
            count = Mathf.Clamp(count - 1, 1, int.MaxValue);
            dayField.text = count.ToString();
        }

        private void IncreaseDayCounter()
        {
            var count = 0;
            int.TryParse(dayField.text, out count);
            count++;
            dayField.text = count.ToString();
        }

        private void AddCoin()
        {
            var count = 0;
            int.TryParse(coinField.text, out count);
            currencyDataController.Add(count);
            Close();
        }
        
        private void AddDay()
        {
            var count = 0;
            int.TryParse(dayField.text, out count);
            pecanServices.DailyLoginManager.TimeForward(count);
            Close();
            pecanServices.Signals.SendDailyLoginSignal();
        }

        private void ResetShop()
        {
            shopDataController.Reset();
            Close();
        }

        private void ResetHighScore()
        {
            PlayerPrefs.SetInt(highScoreKey, 0);
            PlayerPrefs.Save();
            Close();
        }

        private void ResetTutorial()
        {
            tutorialController.Reset();
            Close();
        }
        
        private void ResetAll()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
            ResetShop();
            ResetTutorial();
            Close();
        }
        #endif
    }
}