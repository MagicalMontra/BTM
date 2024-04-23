using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using Doozy.Runtime.UIManager;
using Doozy.Runtime.UIManager.Components;
using HotPlay.PecanUI;
using HotPlay.Utilities;
using System;
using TMPro;
using UnityEngine;
using Zenject;

namespace HotPlay.BoosterMath.Core
{
    public class GameplayDebugPanel : MonoBehaviour
    {
#if CHEAT_ENABLED
        [SerializeField]
        private GameObject contentObject;
        
        [SerializeField]
        private TMP_InputField coinField;
        
        [SerializeField]
        private TMP_InputField scoreField;

        [SerializeField]
        private UIButton addCoinButton;
        
        [SerializeField]
        private UIButton decreaseCoinButton;
        
        [SerializeField]
        private UIButton increaseCoinButton;
        
        [SerializeField]
        private UIButton decreaseScoreButton;
        
        [SerializeField]
        private UIButton increaseScoreButton;

        [SerializeField]
        private UIButton addScoreButton;

        [SerializeField]
        private UIButton activateSlowBooster;
        
        [SerializeField]
        private UIButton dropSlowBooster;
        
        [SerializeField]
        private UIButton deactivateSlowBooster;
        
        [SerializeField]
        private UIButton activateRewindBooster;
        
        [SerializeField]
        private UIButton dropRewindBooster;
        
        [SerializeField]
        private UIButton deactivateRewindBooster;
        
        [SerializeField]
        private UIButton activateScoreBooster;
        
        [SerializeField]
        private UIButton dropScoreBooster;
        
        [SerializeField]
        private UIButton deactivateScoreBooster;

        [SerializeField]
        private UIButton closeButton;

        [Inject]
        private PecanServices pecanServices;
        
        [Inject]
        private ItemDropController itemDropController;
        
        [Inject]
        private GameSessionController gameSessionController;
        
        private void Awake()
        {
            coinField.text = 10.ToString();
            scoreField.text = 10.ToString();
            Bind();
        }

        private void OnDestroy()
        {
            Unbind();
        }

        private void LateUpdate()
        {
            if (transform.parent.GetChild(transform.parent.childCount - 1).gameObject != gameObject)
                transform.SetAsLastSibling();
        }

        private void Bind()
        {
            closeButton.behaviours.AutoGetUnityEvent(UIBehaviour.Name.PointerClick).AddListener(Close);
            increaseCoinButton.behaviours.AutoGetUnityEvent(UIBehaviour.Name.PointerClick).AddListener(IncreaseCoinCounter);
            decreaseCoinButton.behaviours.AutoGetUnityEvent(UIBehaviour.Name.PointerClick).AddListener(DecreaseCoinCounter);
            increaseScoreButton.behaviours.AutoGetUnityEvent(UIBehaviour.Name.PointerClick).AddListener(IncreaseScoreCounter);
            decreaseScoreButton.behaviours.AutoGetUnityEvent(UIBehaviour.Name.PointerClick).AddListener(DecreaseScoreCounter);
            addCoinButton.behaviours.AutoGetUnityEvent(UIBehaviour.Name.PointerClick).AddListener(AddCoin);
            addScoreButton.behaviours.AutoGetUnityEvent(UIBehaviour.Name.PointerClick).AddListener(AddScore);
            activateSlowBooster.behaviours.AutoGetUnityEvent(UIBehaviour.Name.PointerClick).AddListener(ActivateSlowBooster);
            dropSlowBooster.behaviours.AutoGetUnityEvent(UIBehaviour.Name.PointerClick).AddListener(DropSlowBooster);
            deactivateSlowBooster.behaviours.AutoGetUnityEvent(UIBehaviour.Name.PointerClick).AddListener(DeactivateSlowBooster);
            activateRewindBooster.behaviours.AutoGetUnityEvent(UIBehaviour.Name.PointerClick).AddListener(ActivateRewindBooster);
            dropRewindBooster.behaviours.AutoGetUnityEvent(UIBehaviour.Name.PointerClick).AddListener(DropRewindBooster);
            deactivateRewindBooster.behaviours.AutoGetUnityEvent(UIBehaviour.Name.PointerClick).AddListener(DeactivateRewindBooster);
            activateScoreBooster.behaviours.AutoGetUnityEvent(UIBehaviour.Name.PointerClick).AddListener(ActivateScoreBooster);
            dropScoreBooster.behaviours.AutoGetUnityEvent(UIBehaviour.Name.PointerClick).AddListener(DropScoreBooster);
            deactivateScoreBooster.behaviours.AutoGetUnityEvent(UIBehaviour.Name.PointerClick).AddListener(DeactivateScoreBooster);
        }

        private void Unbind()
        {
            closeButton.behaviours.AutoGetUnityEvent(UIBehaviour.Name.PointerClick).RemoveListener(Close);
            increaseCoinButton.behaviours.AutoGetUnityEvent(UIBehaviour.Name.PointerClick).RemoveListener(IncreaseCoinCounter);
            decreaseCoinButton.behaviours.AutoGetUnityEvent(UIBehaviour.Name.PointerClick).RemoveListener(DecreaseCoinCounter);
            increaseScoreButton.behaviours.AutoGetUnityEvent(UIBehaviour.Name.PointerClick).RemoveListener(IncreaseScoreCounter);
            decreaseScoreButton.behaviours.AutoGetUnityEvent(UIBehaviour.Name.PointerClick).RemoveListener(DecreaseScoreCounter);
            addCoinButton.behaviours.AutoGetUnityEvent(UIBehaviour.Name.PointerClick).RemoveListener(AddCoin);
            addScoreButton.behaviours.AutoGetUnityEvent(UIBehaviour.Name.PointerClick).RemoveListener(AddScore);
            activateSlowBooster.behaviours.AutoGetUnityEvent(UIBehaviour.Name.PointerClick).RemoveListener(ActivateSlowBooster);
            dropSlowBooster.behaviours.AutoGetUnityEvent(UIBehaviour.Name.PointerClick).RemoveListener(DropSlowBooster);
            deactivateSlowBooster.behaviours.AutoGetUnityEvent(UIBehaviour.Name.PointerClick).RemoveListener(DeactivateSlowBooster);
            activateRewindBooster.behaviours.AutoGetUnityEvent(UIBehaviour.Name.PointerClick).RemoveListener(ActivateRewindBooster);
            dropRewindBooster.behaviours.AutoGetUnityEvent(UIBehaviour.Name.PointerClick).RemoveListener(DropRewindBooster);
            deactivateRewindBooster.behaviours.AutoGetUnityEvent(UIBehaviour.Name.PointerClick).RemoveListener(DeactivateRewindBooster);
            activateScoreBooster.behaviours.AutoGetUnityEvent(UIBehaviour.Name.PointerClick).RemoveListener(ActivateScoreBooster);
            dropScoreBooster.behaviours.AutoGetUnityEvent(UIBehaviour.Name.PointerClick).RemoveListener(DropScoreBooster);
            deactivateScoreBooster.behaviours.AutoGetUnityEvent(UIBehaviour.Name.PointerClick).RemoveListener(DeactivateScoreBooster);
        }

        private void Close()
        {
            contentObject.SetActive(false);
        }
        
        private void DecreaseCoinCounter()
        {
            var count = 0;
            int.TryParse(coinField.text, out count);
            count = Mathf.Clamp(count - 1, 0, int.MaxValue);
            coinField.text = count.ToString();
        }

        private void IncreaseCoinCounter()
        {
            var count = 0;
            int.TryParse(coinField.text, out count);
            count++;
            coinField.text = count.ToString();
        }
        
        private void DecreaseScoreCounter()
        {
            var count = 0;
            int.TryParse(scoreField.text, out count);
            count = Mathf.Clamp(count - 1, 0, int.MaxValue);
            scoreField.text = count.ToString();
        }

        private void IncreaseScoreCounter()
        {
            var count = 0;
            int.TryParse(scoreField.text, out count);
            count++;
            scoreField.text = count.ToString();
        }

        private void AddCoin()
        {
            var count = 0;
            int.TryParse(coinField.text, out count);
            
            for (int i = 0; i < count; i++)
                gameSessionController.IncreaseCoin();
            
            pecanServices.Events.UpdateGameplayCurrency(gameSessionController.CurrentCoin);
            Close();
        }
        
        private void AddScore()
        {
            var count = 0;
            int.TryParse(scoreField.text, out count);
            gameSessionController.DebugIncreaseScore(count);
            Close();
        }

        private void ActivateSlowBooster()
        {
            itemDropController.DebugCollect(ItemDropTypeEnum.Slow).Forget();
            Close();
        }
        
        private void DropSlowBooster()
        {
            itemDropController.DebugDrop(ItemDropTypeEnum.Slow);
            Close();
        }
        
        private void DeactivateSlowBooster()
        {
            itemDropController.DebugForceDeactivate(ItemDropTypeEnum.Slow);
            Close();
        }
        
        private void ActivateRewindBooster()
        {
            itemDropController.DebugCollect(ItemDropTypeEnum.Rewind).Forget();
            Close();
        }
        
        private void DropRewindBooster()
        {
            itemDropController.DebugDrop(ItemDropTypeEnum.Rewind);
            Close();
        }
        
        private void DeactivateRewindBooster()
        {
            itemDropController.DebugForceDeactivate(ItemDropTypeEnum.Rewind);
            Close();
        }
        
        private void ActivateScoreBooster()
        {
            itemDropController.DebugCollect(ItemDropTypeEnum.ScoreBoost).Forget();
            Close();
        }
        
        private void DropScoreBooster()
        {
            itemDropController.DebugDrop(ItemDropTypeEnum.ScoreBoost);
            Close();
        }
        
        private void DeactivateScoreBooster()
        {
            itemDropController.DebugForceDeactivate(ItemDropTypeEnum.ScoreBoost);
            Close();
        }
        #endif
    }
}
