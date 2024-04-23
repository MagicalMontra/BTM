using Doozy.Runtime.UIManager;
using Doozy.Runtime.UIManager.Components;
using HotPlay.Utilities;
using UnityEngine;

namespace HotPlay.PecanUI.Gameplay
{
    public class PauseDialog : BaseDialog
    {
        [SerializeField]
        private UIButton tutorialButton;

        [SerializeField]
        private UIButton homeButton;

        [SerializeField]
        private UIButton restartButton;

        [SerializeField]
        private UIButton resumeButton;

        protected override void Awake()
        {
            base.Awake();

#if PECAN_NAVIGATOR
            tutorialButton.gameObject.SetActive(true);
            tutorialButton.behaviours.AutoGetUnityEvent(UIBehaviour.Name.PointerLeftClick).AddListener(OnTutorialButtonClicked);
#else
            tutorialButton.gameObject.SetActive(false);
#endif
        }

        protected override void OnHidden()
        {
            base.OnHidden();
            restartButton.behaviours.AutoGetUnityEvent(UIBehaviour.Name.PointerLeftClick).RemoveListener(OnRestartButtonClicked);
        }

        protected override void OnVisible()
        {
            base.OnVisible();
            restartButton.behaviours.AutoGetUnityEvent(UIBehaviour.Name.PointerLeftClick).AddListener(OnRestartButtonClicked);
        }

        private void OnTutorialButtonClicked()
        {
            PecanServices.Instance.Events.PauseEventsHandler.InvokeTutorialButtonClicked();
            PecanServices.Instance.Signals.SendTutorialSignal(PecanServices.Instance.GetTutorialData());
        }
        
        private void OnRestartButtonClicked()
        {
            restartButton.behaviours.AutoGetUnityEvent(UIBehaviour.Name.PointerLeftClick).RemoveListener(OnRestartButtonClicked);
        }
    }
}
