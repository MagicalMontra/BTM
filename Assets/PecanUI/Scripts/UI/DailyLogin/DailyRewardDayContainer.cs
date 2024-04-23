using System;
using System.Collections;
using System.Collections.Generic;
using Doozy.Runtime.Reactor.Animators;
using Doozy.Runtime.UIManager.Components;
using HotPlay.Utilities;
using I2.Loc;
using Spine.Unity;
using TMPro;
using UnityEngine;

namespace HotPlay.PecanUI
{
    public class DailyRewardDayContainer : MonoBehaviour
    {
        [SerializeField]
        private UIButton claimButton;
        
        [SerializeField]
        private UIAnimator highlightVfxAnimator;

        [SerializeField]
        private GameObject highlightVfx;

        [SerializeField]
        private UIAnimator checkMark;

        [SerializeField]
        private TextMeshProUGUI dayLabel;

        [SerializeField]
        private TextMeshProUGUI rewardAmountLabel;
        
        [SerializeField]
        private LocalizationParamsManager amountParamsManager; 

        [SerializeField]
        private LocalizationParamsManager dayParamsManager;
        
        public event Action<DailyRewardDayContainer> Clicked;

        public int RewardAmount { get; private set; }
        public int Day { get; private set; }

        private void Awake()
        {
            claimButton.behaviours.AutoGetUnityEvent(Doozy.Runtime.UIManager.UIBehaviour.Name.PointerLeftClick).AddListener(OnClickClaim);
        }

        public void SetData(int reward, int day)
        {
            RewardAmount = reward;
            Day = day;
            amountParamsManager.SetParameterValue("AMOUNT", reward.ToString(), true);
            dayParamsManager.SetParameterValue("DAY", day.ToString(), true);
        }

        public void SetCurrentDay()
        {
            highlightVfx.SetActive(true);
            highlightVfxAnimator.Play();
            checkMark.gameObject.SetActive(false);
        }

        public void SetClaimed()
        {
            highlightVfx.SetActive(false);
            checkMark.gameObject.SetActive(true);
            checkMark.Play();
        }

        public void SetNotClaimed()
        {
            highlightVfx.SetActive(false);
            checkMark.gameObject.SetActive(false);
        }

        private void OnClickClaim()
        {
            Clicked?.Invoke(this);
            PecanServices.Instance.Events.DailyLoginEventsHandler.InvokeElementClicked(this);
        }
    }
}
