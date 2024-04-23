#nullable enable
using System;
using Doozy.Runtime.UIManager;
using Doozy.Runtime.UIManager.Animators;
using Doozy.Runtime.UIManager.Components;
using HotPlay.BoosterMath.Core.UI;
using HotPlay.PecanUI;
using HotPlay.Utilities;
using UnityEngine;
using Zenject;

namespace HotPlay.BoosterMath.Core
{
    public class SelectDifficultyButton : MonoBehaviour
    {
        public bool IsPointerEntered => isPointerEntered;
        
        [SerializeField]
        private GameModeEnum difficulty;

        [SerializeField]
        private UIButton? button;

        [SerializeField]
        private BaseUISelectableAnimator[] animators;
        
        [Inject]
        private SoundData soundData;
        
        [Inject]
        private PecanServices pecanServices;
        
        [Inject]
        private GameModeController gameMode;

        private bool isPointerEntered;
        private Action? action;
        private GameModeSelectionUI gameModeSelectionUI;

        public void Initialize(Action? action, GameModeSelectionUI gameModeSelectionUI)
        {
            this.gameModeSelectionUI ??= gameModeSelectionUI;
            
            if (button == null || action == null)
                return;

            this.action = action;
            button.behaviours.AutoGetUnityEvent(UIBehaviour.Name.PointerClick).AddListener(OnClicked);
            button.behaviours.AutoGetUnityEvent(UIBehaviour.Name.PointerExit).AddListener(OnPointerExit);
            button.behaviours.AutoGetUnityEvent(UIBehaviour.Name.PointerEnter).AddListener(OnPointerEntered);
            button.behaviours.AutoGetUnityEvent(UIBehaviour.Name.PointerEnter).AddListener(action.Invoke);
        }
        
        public void Dispose()
        {
            if (button == null)
                return;
            
            button.behaviours.AutoGetUnityEvent(UIBehaviour.Name.PointerClick).RemoveListener(OnClicked);
            button.behaviours.AutoGetUnityEvent(UIBehaviour.Name.PointerExit).RemoveListener(OnPointerExit);
            button.behaviours.AutoGetUnityEvent(UIBehaviour.Name.PointerEnter).RemoveListener(OnPointerEntered);
            
            if (action == null)
                return;
            
            button.behaviours.AutoGetUnityEvent(UIBehaviour.Name.PointerEnter).RemoveListener(action.Invoke);
        }
        
        public void OnSelected()
        {
            foreach (var animator in animators)
            {
                animator.StopAllReactions();
                animator.Play(UISelectionState.Selected);
            }
        }

        private void OnPointerEntered()
        {
            isPointerEntered = true;
            
            if (gameModeSelectionUI == null)
                return;
            
            gameModeSelectionUI.IsPointerEntered = true;
        }
        
        private void OnPointerExit()
        {
            isPointerEntered = false;
            
            if (gameModeSelectionUI == null)
                return;

            gameModeSelectionUI.IsPointerEntered = false;
        }
        
        public void OnDeselected()
        {
            foreach (var animator in animators)
            {
                animator.StopAllReactions();
                animator.Play(UISelectionState.Normal);
            }
        }
        
        public void OnClicked()
        {
            pecanServices.SoundManager.SoundPlayer.Play(soundData.ButtonClick, false);
            gameMode.ChangeMode(difficulty);
        }
    }
}