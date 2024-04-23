using Doozy.Runtime.UIManager;
using Doozy.Runtime.UIManager.Components;
using HotPlay.Utilities;
using UnityEngine;

namespace HotPlay.PecanUI.Gameplay
{
    public class GameplayDialog : BaseDialog
    {
        [SerializeField]
        private UIButton tutorialButton;
        public UIButton TutorialButton => tutorialButton;

        [SerializeField]
        private UIButton pauseButton;
        public UIButton PauseButton => pauseButton;

        [SerializeField]
        private Transform customGameplayPanelHolder;

        protected override void Awake()
        {
            base.Awake();
            PecanServices.Instance.CreateCustomGameplayPanel(customGameplayPanelHolder);
        }
        
        private void Start()
        {
            pauseButton.behaviours.AutoGetUnityEvent(UIBehaviour.Name.PointerLeftClick).AddListener(OnPauseButtonClicked);

#if PECAN_NAVIGATOR
            tutorialButton.gameObject.SetActive(false);
            pauseButton.gameObject.SetActive(false);
#else
            tutorialButton.gameObject.SetActive(true);
            tutorialButton.behaviours.AutoGetUnityEvent(UIBehaviour.Name.PointerLeftClick).AddListener(OnTutorialButtonClicked);
            pauseButton.gameObject.SetActive(true);
#endif
        }

        protected override void OnShowing()
        {
            base.OnShowing();

            if (PecanServices.Instance.CustomGameplayPanel != null)
            {
                PecanServices.Instance.CustomGameplayPanel.OnShowing();
            }
        }

        protected override void OnVisible()
        {
            base.OnVisible();

            if (PecanServices.Instance.CustomGameplayPanel != null)
            {
                PecanServices.Instance.CustomGameplayPanel.OnVisible();
            }
        }

        protected override void OnHide()
        {
            base.OnHide();

            if (PecanServices.Instance.CustomGameplayPanel != null)
            {
                PecanServices.Instance.CustomGameplayPanel.OnHide();
            }
        }

        protected override void OnHidden()
        {
            base.OnHidden();

            if (PecanServices.Instance.CustomGameplayPanel != null)
            {
                PecanServices.Instance.CustomGameplayPanel.OnHidden();
            }
        }

        private void OnTutorialButtonClicked()
        {
            PecanServices.Instance.Events.GameplayEventsHandler.InvokeTutorialButtonClicked();
            PecanServices.Instance.Signals.SendTutorialSignal(PecanServices.Instance.GetTutorialData());
        }

        private void OnPauseButtonClicked()
        {
            PecanServices.Instance.Signals.SendPauseSignal();
        }
    }
}
