using Doozy.Runtime.UIManager.Containers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HotPlay.PecanUI
{
    public enum TopBarType
    {
        None = 0,
        Common = 1,
        Gameplay = 2
    }

    public class TopBar : MonoBehaviour
    {
        [SerializeField]
        private CommonTopBar commonTopBar;

        [SerializeField]
        private GameplayTopBar gameplayTopBar;

        public TopBarType CurrentTopBar { get; private set; }

        private Dictionary<TopBarType, BaseTopBar> topBarLookUp;

        private void Awake()
        {
            CurrentTopBar = TopBarType.None;

            topBarLookUp = new Dictionary<TopBarType, BaseTopBar>()
            {
                { TopBarType.Common, commonTopBar },
                { TopBarType.Gameplay, gameplayTopBar }
            };

            foreach (var bar in topBarLookUp)
            {
                bar.Value.UIContainer.InstantHide();
            }

            PecanServices.Instance.Events.TopBarChange += OnTopBarChange;
        }

        public T GetTopBar<T>() where T:BaseTopBar
        {
            foreach (var topBar in topBarLookUp.Values)
            {
                if (topBar is T)
                {
                    return topBar as T;
                }
            }

            Debug.LogAssertion($"Top bar {typeof(T)} not found");
            return null;
        }

        private void OnTopBarChange(TopBarType type, bool transition)
        {
            if (CurrentTopBar == type)
            {
                return;
            }

            CurrentTopBar = type;

            foreach (var bar in topBarLookUp)
            {
                if (bar.Key == type)
                {
                    if (transition)
                    {
                        bar.Value.UIContainer.Show();
                    }
                    else
                    {
                        bar.Value.UIContainer.InstantShow();
                    }
                }
                else
                {
                    if (transition)
                    {
                        bar.Value.UIContainer.Hide();
                    }
                    else
                    {
                        bar.Value.UIContainer.InstantHide();
                    }
                }
            }
        }

        private void OnDestroy()
        {
            if (PecanServices.HasInstance)
            {
                PecanServices.Instance.Events.TopBarChange -= OnTopBarChange;
            }
        }
    }
}
