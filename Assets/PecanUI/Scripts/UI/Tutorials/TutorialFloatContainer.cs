using Doozy.Runtime.UIManager.Components;
using HotPlay.PecanUI.Analytic;
using HotPlay.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HotPlay.PecanUI
{
    public class TutorialFloatContainer : TutorialContainer
    {
        public override TutorialContainerType ContainerType => TutorialContainerType.Float;

        [SerializeField]
        private Image tutorialImage;

        [SerializeField]
        private TextMeshProUGUI tutorialText;

        [SerializeField]
        private UIButton closeButton;

        private TutorialData tutorialData;

        private IAnalyticEvent<DesignEventData<string>, string> tutorialEvent;

        protected override void Awake()
        {
            base.Awake();

            closeButton.behaviours.AutoGetUnityEvent(Doozy.Runtime.UIManager.UIBehaviour.Name.PointerLeftClick).AddListener(OnTutorialClosed);
            closeButton.behaviours.AutoGetUnityEvent(Doozy.Runtime.UIManager.UIBehaviour.Name.PointerLeftClick).AddListener(OnCloseButtonClicked);
        }

        public void Setup(TutorialData tutorialData)
        {
            this.tutorialData = tutorialData;

            tutorialImage.sprite = tutorialData.TutorialSprite;
            tutorialEvent = new StringAnalyticDesignEvent("tutorial");
            PecanServices.Instance.Analytic.TryLog($"{tutorialData.TutorialKey}:start", tutorialEvent);
            tutorialText.text = (PecanServices.Instance.Configs.TutorialCategory + tutorialData.TutorialKey).GetLocalizedString();
        }

        private void OnTutorialClosed()
        {
            PecanServices.Instance.Analytic.TryLog($"{tutorialData.TutorialKey}:complete", tutorialEvent);
        }

        public override void RecoveryNavigator()
        {
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(closeButton.gameObject);
        }
    }
}
