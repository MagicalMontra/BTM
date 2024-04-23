using Doozy.Runtime.UIManager.Containers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HotPlay.PecanUI
{
    public abstract class TutorialContainer : MonoBehaviour
    {
        [SerializeField]
        private UIContainer container;
        public UIContainer Container => container;

        [SerializeField]
        private CanvasGroup canvasGroup;

        public abstract TutorialContainerType ContainerType { get; }

        protected bool hasShow;

        protected virtual void Awake()
        {
#if !PECAN_NAVIGATOR
            container.AutoSelectAfterShow = false;
#endif
        }

        public virtual void RecoveryNavigator()
        {
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(container.AutoSelectTarget);
        }

        /// <summary>
        /// Called when start playing show dialog animation
        /// </summary>
        public virtual void OnShowing()
        {
            hasShow = true;
        }

        /// <summary>
        /// Called when show animation finished
        /// </summary>
        public virtual void OnVisible()
        {
        }

        /// <summary>
        /// Called when start playing hide animation
        /// </summary>
        public virtual void OnHide()
        {
        }

        /// <summary>
        /// Called when hide animation finished
        /// </summary>
        public virtual void OnHidden()
        {
            hasShow = false;
        }

        protected void OnCloseButtonClicked()
        {
            PecanServices.Instance.Events.TutorialEventsHandler.InvokeCloseButtonClicked();
        }
    }
}
