using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace HotPlay.BoosterMath.Core
{
    public class BoosterPanel : MonoBehaviour
    {
        [SerializeField]
        private BoosterIcon[] boosterIcons;

        private Dictionary<ItemDropTypeEnum, BoosterIcon> icons = new Dictionary<ItemDropTypeEnum, BoosterIcon>();

        private void Awake()
        {
            foreach (BoosterIcon icon in boosterIcons)
            {
                icon.gameObject.SetActive(false);
                icons.Add(icon.Type, icon);
            }
        }

        public void OnBoosterUpdate(ItemDropTypeEnum booster)
        {
            if (!icons[booster].gameObject.activeInHierarchy)
                icons[booster].gameObject.SetActive(true);
        }

        public void OnBoosterTicked(ItemDropTypeEnum booster, ITimer timer)
        {
            icons[booster].OnTimerTick(timer);
        }

        public void OnBoosterActivate(ItemDropTypeEnum booster)
        {
            if (!icons[booster].gameObject.activeInHierarchy)
                icons[booster].gameObject.SetActive(true);
            
            icons[booster].PlayActivateAnimation();
        }

        public void OnBoosterDeactivate(ItemDropTypeEnum booster)
        {
            icons[booster].PlayDisappearAnimation().Forget();
        }
        
        public void OnBoosterEffectApplied(ItemDropTypeEnum booster)
        {
            icons[booster].PlayEffectAnimation();
        }
    }
}