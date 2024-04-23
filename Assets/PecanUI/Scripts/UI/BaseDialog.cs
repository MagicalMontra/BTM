using System;
using Doozy.Runtime.Signals;
using Doozy.Runtime.UIManager;
using Doozy.Runtime.UIManager.Containers;
using UnityEngine;

namespace HotPlay.PecanUI
{
    public abstract class BaseDialog : MonoBehaviour
    {
        public VisibilityState State => view.visibilityState;
        
        [SerializeField]
        private string dialogName;
        
        [SerializeField]
        private bool useTopBar;

        [SerializeField]
        private string hidePanelSoundName = "UIPanel/Hide";
        
        [SerializeField]
        private string showPanelSoundName = "UIPanel/Show";

        [SerializeField]
        private TopBarType topBar;

        [SerializeField]
        protected UIView view;

        [SerializeField]
        private CanvasGroup canvasGroup;

        public event Action OnStateChanged;
        public event Action<BaseDialog> Visible;
        public event Action<BaseDialog> Hide;
        public event Action<BaseDialog> Showing;
        public event Action<BaseDialog> Hidden;

        protected bool hasShow;

        protected virtual void Awake()
        {
            if (view == null)
            {
                view = GetComponent<UIView>();
            }

            if (canvasGroup == null)
            {
                canvasGroup = GetComponent<CanvasGroup>();
            }

            Debug.Assert(view != null, "UIView component could not be found");

            view.OnVisibleCallback.Event.AddListener(OnVisible);
            view.OnHideCallback.Event.AddListener(OnHide);
            view.OnHiddenCallback.Event.AddListener(OnHidden);
            view.OnVisibilityChangedCallback.AddListener(OnVisibilityChanged);

#if !PECAN_NAVIGATOR
            view.AutoSelectAfterShow = false;
#endif
        }

        public void SetCanvasGroupInteractable(bool isOn)
        {
            if (canvasGroup != null)
            {
                canvasGroup.interactable = isOn;
            }
        }

        /// <summary>
        /// Recovery selection to the auto select target if already set.
        /// </summary>
        public void TryRecoveryNavigator()
        {
#if PECAN_NAVIGATOR
            var currentSelected = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;

            // Mark as lost if selecting is null or selecting object is inactive
            bool isLost = currentSelected == null || (currentSelected != null && !currentSelected.activeInHierarchy);

            if (view.AutoSelectAfterShow && isLost)
            {
                RecoveryNavigator();
            }
#endif
        }

        protected virtual void RecoveryNavigator()
        {
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(view.AutoSelectTarget);
        }

        /// <summary>
        /// Called when start playing show dialog animation
        /// </summary>
        protected virtual void OnShowing()
        {
            hasShow = true;
            PecanServices.Instance.SignalProcessor.OnResponse(dialogName);

            if (useTopBar)
            {
                PecanServices.Instance.Events.ChangeTopBar(topBar, true);
            }
            Showing?.Invoke(this);
        }

        /// <summary>
        /// Called when show animation finished
        /// </summary>
        protected virtual void OnVisible()
        {
            Visible?.Invoke(this);
            PecanServices.Instance.SignalProcessor.OnResponse(dialogName);
            canvasGroup.alpha = 1;
            
            if (useTopBar)
            {
                PecanServices.Instance.Events.ChangeTopBar(topBar, false);
            }
        }

        /// <summary>
        /// Called when start playing hide animation
        /// </summary>
        protected virtual void OnHide()
        {
            Hide?.Invoke(this);
        }

        /// <summary>
        /// Called when hide animation finished
        /// </summary>
        protected virtual void OnHidden()
        {
            hasShow = false;
            Hidden?.Invoke(this);
        }

        protected virtual void OnVisibilityChanged(VisibilityState state)
        {
            switch (state)
            {
                case VisibilityState.IsShowing:
                    OnShowing();
                    break;
                case VisibilityState.Visible:
                    OnVisible();
                    break;
                case VisibilityState.IsHiding:
                    OnHide();
                    break;
                case VisibilityState.Hidden:
                    OnHidden();
                    break;
            }
            
            OnStateChanged?.Invoke();
        }
    }
}
