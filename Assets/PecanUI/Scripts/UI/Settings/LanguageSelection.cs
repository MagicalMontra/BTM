using Doozy.Runtime.UIManager.Components;
using HotPlay.Utilities;
using System;
using System.Linq;
using TMPro;
using UnityEngine;

namespace HotPlay.PecanUI.Settings
{
    public class LanguageSelection : MonoBehaviour
    {
        [SerializeField]
        private UIButton leftButton;

        [SerializeField]
        private UIButton rightButton;

        [SerializeField]
        private TextMeshProUGUI label;

        public event Action IndexChanged;

        public int CurrentIndex { get; private set; }

        private LanguageData[] languages;

        private void Awake()
        {
            leftButton.behaviours.AutoGetUnityEvent(Doozy.Runtime.UIManager.UIBehaviour.Name.PointerLeftClick).AddListener(Left);
            rightButton.behaviours.AutoGetUnityEvent(Doozy.Runtime.UIManager.UIBehaviour.Name.PointerLeftClick).AddListener(Right);
        }

        public void SetItems(LanguageData[] languages, string initialLanguageCode)
        {
            var initialLanguageData = languages.Where(x => x.LanguageCode == initialLanguageCode).FirstOrDefault();
            if(initialLanguageData != null)
            {
                SetItems(languages, Array.IndexOf(languages, initialLanguageData));
            }
            else
            {
                SetItems(languages, 0);
            }
        }

        public void SetItems(LanguageData[] languages, int initialIndex)
        {
            CurrentIndex = initialIndex;
            this.languages = languages;
            RefreshStates();
        }

        private void Left()
        {
            if (CurrentIndex > 0)
            {
                CurrentIndex--;
                I2.Loc.LocalizationManager.CurrentLanguageCode = languages[CurrentIndex].LanguageCode;
                RefreshStates();
                IndexChanged?.Invoke();
            }
        }

        private void Right()
        {
            if (CurrentIndex < languages.Length - 1)
            {
                CurrentIndex++;
                I2.Loc.LocalizationManager.CurrentLanguageCode = languages[CurrentIndex].LanguageCode;
                RefreshStates();
                IndexChanged?.Invoke();
            }
        }

        private void RefreshStates()
        {
            bool showArrows = languages.Length > 1;
            bool hasPrevious = CurrentIndex > 0;
            bool hasNext = CurrentIndex < languages.Length - 1;

            leftButton.gameObject.SetActive(showArrows);
            rightButton.gameObject.SetActive(showArrows);
            
            label.text = languages[CurrentIndex].LanguageName.GetLocalizedString();

            if (!showArrows)
            {
                return;
            }

#if PECAN_NAVIGATOR

            var currentSelected = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;

            if (currentSelected == leftButton.gameObject && !hasPrevious)
            {
                UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(rightButton.gameObject);
            }
            else if (currentSelected == rightButton.gameObject && !hasNext)
            {
                UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(leftButton.gameObject);
            }
#endif

            leftButton.interactable = hasPrevious;
            rightButton.interactable = hasNext;
        }
    }
}
