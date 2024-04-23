using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace HotPlay.PecanUI
{
    //Reference: https://gist.github.com/llamacademy/e651518eebc09650cc7b66492aefe58e

    public class CurrencyBar : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI text;

        [SerializeField]
        private int countFPS = 30;

        [SerializeField]
        private float duration = 1f;

        [SerializeField]
        private string numberFormat = "N0";

        private int _value;

        //TODO: implement on currency change event and set Value on invoked
        public int Value
        {
            get
            {
                return _value;
            }
            set
            {
                UpdateText(value);
                _value = value;
            }
        }

        private Coroutine CountingCoroutine;

        protected void OnCurrencyUpdate(int currency, bool shouldPlayAnim)
        {
            if(gameObject.activeInHierarchy && shouldPlayAnim)
            {
                Value = currency;
            }
            else
            {
                _value = currency;
                text.SetText(currency.ToString(numberFormat));
            }
        }

        private void UpdateText(int newValue)
        {
            if (CountingCoroutine != null)
            {
                StopCoroutine(CountingCoroutine);
            }

            CountingCoroutine = StartCoroutine(CountText(newValue));
        }

        private IEnumerator CountText(int newValue)
        {
            WaitForSeconds Wait = new WaitForSeconds(1f / countFPS);
            int previousValue = _value;
            int stepAmount;

            if (newValue - previousValue < 0)
            {
                stepAmount = Mathf.FloorToInt((newValue - previousValue) / (countFPS * duration)); // newValue = -20, previousValue = 0. CountFPS = 30, and Duration = 1; (-20- 0) / (30*1) // -0.66667 (ceiltoint)-> 0
            }
            else
            {
                stepAmount = Mathf.CeilToInt((newValue - previousValue) / (countFPS * duration)); // newValue = 20, previousValue = 0. CountFPS = 30, and Duration = 1; (20- 0) / (30*1) // 0.66667 (floortoint)-> 0
            }

            if (previousValue < newValue)
            {
                while(previousValue < newValue)
                {
                    previousValue += stepAmount;
                    if (previousValue > newValue)
                    {
                        previousValue = newValue;
                    }

                    text.SetText(previousValue.ToString(numberFormat));

                    yield return Wait;
                }
            }
            else
            {
                while (previousValue > newValue)
                {
                    previousValue += stepAmount; // (-20 - 0) / (30 * 1) = -0.66667 -> -1              0 + -1 = -1
                    if (previousValue < newValue)
                    {
                        previousValue = newValue;
                    }

                    text.SetText(previousValue.ToString(numberFormat));

                    yield return Wait;
                }
            }
        }

        protected virtual void OnDisable()
        {
            text.SetText(Value.ToString(numberFormat));
        }
    }
}
