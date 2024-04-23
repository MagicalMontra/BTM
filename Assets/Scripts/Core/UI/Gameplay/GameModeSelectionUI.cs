#nullable enable
using System;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using HotPlay.BoosterMath.Core.UI;
using UnityEngine;
using Zenject;

namespace HotPlay.BoosterMath.Core
{
    public class GameModeSelectionUI : MonoBehaviour
    {
        public bool IsPointerEntered;
        
        [SerializeField]
        private Transform dialogTransform;
        
        [SerializeField]
        private SelectDifficultyButton[] modeButtons;

        private int currentButtonIndex = -1;
        
        private RewirdInputController inputController;

        [Inject]
        public void Construct(RewirdInputController inputController)
        {
            this.inputController = inputController;
            dialogTransform.DOScale(Vector3.zero, 0.1f);
            gameObject.SetActive(false);
        }

        private void Update()
        {
            if (!gameObject.activeInHierarchy)
                return;
            
            if (IsPointerEntered)
            {
                for (int i = 0; i < modeButtons.Length; i++)
                {
                    if (modeButtons[i].IsPointerEntered)
                    {
                        currentButtonIndex = i;
                        modeButtons[i].OnSelected();
                        continue;
                    }

                    modeButtons[i].OnDeselected();
                }
            }
            else
            {
                if (currentButtonIndex < 0 || currentButtonIndex >= modeButtons.Length)
                    return;
                
                modeButtons[currentButtonIndex].OnSelected();
            }
        }

        public void Show()
        {
            BindButtons();
            currentButtonIndex = currentButtonIndex < 0 ? 0 : currentButtonIndex;
            modeButtons[currentButtonIndex].OnSelected();
            gameObject.SetActive(true);
            dialogTransform.DOScale(Vector3.one, 0.4f);
        }
        
        public void Hide()
        {
            UnbindButtons();
            dialogTransform.DOScale(Vector3.zero, 0.4f);
            gameObject.SetActive(false);
        }
        
        public async UniTaskVoid Hide(CancellationToken cancellationToken)
        {
            UnbindButtons();
            await dialogTransform.DOScale(Vector3.zero, 0.4f).WithCancellation(cancellationToken);
            gameObject.SetActive(false);
        }

        private void OnSubmitPressed()
        {
            modeButtons[currentButtonIndex].OnDeselected();
            modeButtons[currentButtonIndex].OnClicked();
        }
        
        private void OnLeftButtonPressed()
        {
            currentButtonIndex--;
            
            if (currentButtonIndex < 0)
                currentButtonIndex = modeButtons.Length - 1;
            
            AnimateButtons();
        }
        
        private void OnRightButtonPressed()
        {
            currentButtonIndex++;

            if (currentButtonIndex > modeButtons.Length - 1)
                currentButtonIndex = 0;
            
            AnimateButtons();
        }

        private void BindButtons()
        {
            inputController.LeftKeyDown += OnLeftButtonPressed;
            inputController.RightKeyDown += OnRightButtonPressed;
            inputController.SubmitKeyDown += OnSubmitPressed;
            
            for (int i = 0; i < modeButtons.Length; i++)
                modeButtons[i].Initialize(DeselectButtons, this);
        }
        
        private void UnbindButtons()
        {
            inputController.LeftKeyDown -= OnLeftButtonPressed;
            inputController.RightKeyDown -= OnRightButtonPressed;
            inputController.SubmitKeyDown -= OnSubmitPressed;
            
            for (int i = 0; i < modeButtons.Length; i++)
                modeButtons[i].Dispose();
        }

        private void DeselectButtons()
        {
            for (int i = 0; i < modeButtons.Length; i++)
            {
                modeButtons[i].OnDeselected();
            }
        }
        
        private void AnimateButtons()
        {
            for (int i = 0; i < modeButtons.Length; i++)
            {
                if (i == currentButtonIndex)
                {
                    modeButtons[i].OnSelected();
                    continue;
                }
                
                modeButtons[i].OnDeselected();
            }
        }

        public class Factory : PlaceholderFactory<GameModeSelectionUI, Transform, GameModeSelectionUI>
        {
            
        }
    }
}