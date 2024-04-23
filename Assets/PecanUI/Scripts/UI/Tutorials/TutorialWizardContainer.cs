using Doozy.Runtime.UIManager.Components;
using HotPlay.PecanUI.Analytic;
using HotPlay.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HotPlay.PecanUI
{
    public class TutorialWizardContainer : TutorialContainer
    {
        public override TutorialContainerType ContainerType => TutorialContainerType.Wizard;

        [SerializeField]
        private Image tutorialImage;

        [SerializeField]
        private TextMeshProUGUI tutorialText;

        [SerializeField]
        private TextMeshProUGUI indicatorText;

        [SerializeField]
        private UIButton nextButton;

        [SerializeField]
        private UIButton prevButton;

        [SerializeField]
        private UIButton closeButton;

        private bool[] flags;
        private TutorialData[] tutorialData;
        private IAnalyticEvent<DesignEventData<string>, string>[] tutorialEvent;

        private int currentIndex;

        protected override void Awake()
        {
            base.Awake();

            nextButton.behaviours.AutoGetUnityEvent(Doozy.Runtime.UIManager.UIBehaviour.Name.PointerLeftClick).AddListener(OnNextButtonClicked);
            prevButton.behaviours.AutoGetUnityEvent(Doozy.Runtime.UIManager.UIBehaviour.Name.PointerLeftClick).AddListener(OnPrevButtonClicked);
            closeButton.behaviours.AutoGetUnityEvent(Doozy.Runtime.UIManager.UIBehaviour.Name.PointerLeftClick).AddListener(OnTutorialClosed);
            closeButton.behaviours.AutoGetUnityEvent(Doozy.Runtime.UIManager.UIBehaviour.Name.PointerLeftClick).AddListener(OnCloseButtonClicked);
        }
        
        private void OnTutorialClosed()
        {
            for (int i = 0; i < tutorialEvent.Length; i++)
            {
                if (flags[i])
                    continue;
                
                PecanServices.Instance.Analytic.TryLog($"{tutorialData[i].TutorialKey}:skip", tutorialEvent[i]);
            }
        }

        public void Setup(TutorialData[] tutorialData)
        {
            this.tutorialData = tutorialData;
            flags = new bool[this.tutorialData.Length];
            tutorialEvent = new IAnalyticEvent<DesignEventData<string>, string>[tutorialData.Length];

            for (int i = 0; i < tutorialEvent.Length; i++)
            {
                tutorialEvent[i] = new StringAnalyticDesignEvent("tutorial");
                PecanServices.Instance.Analytic.TryLog($"{tutorialData[i].TutorialKey}:start", tutorialEvent[i]);
            }

            if (tutorialData != null && tutorialData.Length > 0)
            {
                UpdateTutorial(0);
            }
        }

        public override void RecoveryNavigator()
        {
            if (nextButton.interactable)
            {
                UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(nextButton.gameObject);
            }
            else
            {
                UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(closeButton.gameObject);
            }
        }

        public override void OnShowing()
        {
            base.OnShowing();

            currentIndex = 0;

            if (tutorialData != null && tutorialData.Length > 0)
            {
                UpdateTutorial(0);
                
                if (flags[0])
                    return;
                
                flags[0] = true;
                tutorialEvent[0] = new StringAnalyticDesignEvent("tutorial");
                PecanServices.Instance.Analytic.TryLog($"{tutorialData[0].TutorialKey}:complete", tutorialEvent[0]);
            }
        }

        public override void OnHidden()
        {
            base.OnHidden();
        }

        private void OnNextButtonClicked()
        {
            PecanServices.Instance.Events.TutorialEventsHandler.InvokeNextButtonClicked();

            if (tutorialData != null && tutorialData.Length > 0)
            {
                int lastIndex = tutorialData.Length - 1;
                currentIndex = currentIndex == lastIndex ? lastIndex : currentIndex + 1;
                UpdateTutorial(currentIndex);
                
                if (flags[currentIndex])
                    return;
                
                flags[currentIndex] = true;
                tutorialEvent[currentIndex] = new StringAnalyticDesignEvent("tutorial");
                PecanServices.Instance.Analytic.TryLog($"{tutorialData[currentIndex].TutorialKey}:complete", tutorialEvent[currentIndex]);
            }
        }

        private void OnPrevButtonClicked()
        {
            PecanServices.Instance.Events.TutorialEventsHandler.InvokePreviousButtonClicked();

            if (tutorialData != null && tutorialData.Length > 0)
            {
                currentIndex = currentIndex == 0 ? 0 : currentIndex - 1;
                UpdateTutorial(currentIndex);
            }
        }

        private void UpdateTutorial(int index)
        {
            if (tutorialData != null && index >= 0 && index < tutorialData.Length)
            {
                tutorialImage.sprite = tutorialData[index].TutorialSprite;
                tutorialText.text = (PecanServices.Instance.Configs.TutorialCategory + tutorialData[index].TutorialKey).GetLocalizedString();

                indicatorText.text = $"{index + 1}/{tutorialData.Length}";

                bool hasPrevious = index > 0 && index < tutorialData.Length;
                bool hasNext = index >= 0 && index < tutorialData.Length - 1;

#if PECAN_NAVIGATOR

                var currentSelected = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;

                if (!hasPrevious && !hasNext)
                {
                    UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(closeButton.gameObject);
                }
                else if (currentSelected == prevButton.gameObject && !hasPrevious)
                {
                    UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(nextButton.gameObject);
                }
                else if (currentSelected == nextButton.gameObject && !hasNext)
                {
                    UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(closeButton.gameObject);
                }
#endif

                prevButton.enabled = hasPrevious;
                nextButton.enabled = hasNext;
                prevButton.interactable = hasPrevious;
                nextButton.interactable = hasNext;
            }
        }
    }
}
