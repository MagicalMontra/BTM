#nullable enable

using Doozy.Runtime.Signals;
using Doozy.Runtime.UIManager.Components;
using HotPlay.DailyLogin;
using HotPlay.Utilities;
using System;
using System.Linq;
using HotPlay.goPlay.Services.Rewards;
using UnityEngine;

#if PECAN_NAVIGATOR
using UnityEngine.EventSystems;
#endif

namespace HotPlay.PecanUI.DailyLogin
{
    public class DailyLoginDialog : BaseDialog
    {
        [SerializeField]
        private UIButton closeButton = default!;

        [SerializeField]
        private Canvas canvas = default!;

        [SerializeField]
        private DailyRewardDayContainer[] dayElements = new DailyRewardDayContainer[0];
        public DailyRewardDayContainer[] DayElements => dayElements;

        private DailyLoginManager? manager;

        protected override void Awake()
        {
            base.Awake();

            for (int i = 0; i < dayElements.Length; i++)
            {
                dayElements[i].Clicked += OnClickReward;
            }
        }

        private void Start()
        {
            manager = PecanServices.Instance.DailyLoginManager;
            closeButton.behaviours.AutoGetUnityEvent(Doozy.Runtime.UIManager.UIBehaviour.Name.PointerLeftClick).AddListener(OnCloseButtonClicked);
        }

        private void OnValidate()
        {
            if(closeButton == null)
            {
                Debug.LogError($"{nameof(closeButton)} is null");
            }

            if (canvas == null)
            {
                Debug.LogError($"{nameof(canvas)} is null");
            }

            if (dayElements == null)
            {
                Debug.LogError($"{nameof(dayElements)} is null");
            }
        }

        public bool IsClaimed()
        {
            return manager?.IsClaimed ?? false;
        }

        protected override void OnShowing()
        {
            base.OnShowing();

            canvas.enabled = true;
            RefreshElements();
        }

        protected override void OnVisible()
        {
            base.OnVisible();

            RefreshElements();

            closeButton.gameObject.SetActive(IsClaimed());
#if PECAN_NAVIGATOR
            if(IsClaimed())
            {
                EventSystem.current.SetSelectedGameObject(closeButton.gameObject);
            }
#endif
        }

        private void RefreshElements()
        {
            if(manager == null)
            {
                return;
            }

            int rewardCount = manager.GetCurrentRewardCount();

            bool showLastWeek = (rewardCount == 1 && manager.LastWeekClaimedCount != 0 && manager.IsClaimed);
            if (showLastWeek)
            {
                rewardCount = manager.LastWeekClaimedCount;
            }

            bool hasUnclaimed = true;

            for (int i = 0; i < dayElements.Length; i++)
            {
                var amount = manager.GetDailyLoginData(i);
                var element = dayElements[i];
                int day = i + 1;
                element.SetData(amount, day);

                if (showLastWeek && rewardCount >= day)
                {
                    element.SetClaimed();
                }
                else if (!manager.IsClaimed && rewardCount == day)
                {
                    element.SetCurrentDay();
#if PECAN_NAVIGATOR
                    EventSystem.current.SetSelectedGameObject(element.gameObject);
#endif
                    hasUnclaimed = false;
                    view.AutoSelectTarget = element.gameObject;
                }
                else if (rewardCount <= day)
                {
                    element.SetNotClaimed();
                }
                else
                {
                    element.SetClaimed();
                }
            }

            if (hasUnclaimed)
            {
                view.AutoSelectTarget = closeButton.gameObject;
            }
        }

        private void OnClickReward(DailyRewardDayContainer container)
        {
            if(manager == null)
            {
                return;
            }

            int rewardIndex = dayElements.ToList().IndexOf(container);
            int day = rewardIndex + 1;

            int rewardCount = manager.GetCurrentRewardCount();
            if (day == rewardCount && !manager.IsClaimed)
            {
                var claimRewardData = new ClaimRewardData() { day = day, amount = manager.GetDailyLoginData(rewardIndex) };
                Signal.Send("DailyLogin", "Reward", claimRewardData);
            }
        }

        private void OnCloseButtonClicked()
        {
            PecanServices.Instance.Events.DailyLoginEventsHandler.InvokeCloseButtonClicked();
        }

        public void ResetDailyLogin()
        {
            if (manager == null)
            {
                throw new NullReferenceException(nameof(manager));
            }

            manager.ResetDailyLogIn();
            RefreshElements();
        }

        public void NextDay()
        {
            if (manager == null)
            {
                throw new NullReferenceException(nameof(manager));
            }

            manager.TimeForward(1);
            RefreshElements();
        }
    }
}
