using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace HotPlay.BoosterMath.Core
{
    public class HeartPanel : MonoBehaviour
    {
        [Inject]
        private HeartIcon.Pool pool;
        
        [SerializeField]
        private Transform parent;
        
        private int focusIndex;
        private int currentHeart;
        private int maxHeartCount;

        private List<HeartIcon> hearts = new List<HeartIcon>();

        public void Initialize(int maxHeartCount, int initialHeart)
        {
            this.maxHeartCount = maxHeartCount;
            PrepareHeartIcons(maxHeartCount);

            for (int i = 0; i < maxHeartCount; i++)
            {
                hearts[i].gameObject.SetActive(true);
                hearts[i].Enable(true);
                hearts[i].Reinitialize();
            }

            currentHeart = initialHeart;
            focusIndex = currentHeart - 1;
        }

        public void Dispose()
        {
            foreach(HeartIcon heart in hearts)
            {
                pool.Despawn(heart);
            }
            
            hearts.Clear();
        }

        public void UpdateHeart(int value)
        {
            Debug.Assert(value <= maxHeartCount, "The value is more than max heart count should not happen");

            if (value > currentHeart)
            {
                IncreaseHeart(value - currentHeart);
            }
            else if (value < currentHeart)
            {
                ReduceHeart(currentHeart - value);
            }
        }

        private void ReduceHeart(int amount)
        {
            if(amount == 0)
            {
                return;
            }

            for (int i = 0; i < amount; i++)
            {
                hearts[focusIndex].OnHeartLost();
                int tempHeartAmount = currentHeart - 1;
                currentHeart = tempHeartAmount < 0 ? 0 : tempHeartAmount;
                int temp = focusIndex - 1;
                focusIndex = temp < 0 ? 0 : temp;
            }
        }

        private void IncreaseHeart(int amount)
        {
            if(amount == 0)
            {
                return;
            }

            int tempFocusIndex = focusIndex + 1;
            focusIndex = tempFocusIndex > maxHeartCount - 1 ? maxHeartCount - 1 : tempFocusIndex;

            hearts[focusIndex].OnHeartGained();

            int tempHeartAmount = currentHeart + 1;
            currentHeart = tempHeartAmount > maxHeartCount ? maxHeartCount : tempHeartAmount;

            ReduceHeart(amount - 1);
        }

        private void PrepareHeartIcons(int maxHeartCount)
        {
            int instantiateAmount = maxHeartCount - hearts.Count;

            if (instantiateAmount > 0)
            {
                for (int i = 0; i < instantiateAmount; i++)
                {
                    hearts.Add(pool.Spawn(parent));
                }
            }

            foreach(HeartIcon heart in hearts)
            {
                heart.gameObject.SetActive(false);
            }
        }
    }
}