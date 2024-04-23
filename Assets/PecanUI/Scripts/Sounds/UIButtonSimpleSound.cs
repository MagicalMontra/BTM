#nullable enable

using Cysharp.Threading.Tasks;
using Doozy.Runtime.UIManager;
using Doozy.Runtime.UIManager.Components;
using HotPlay.Utilities;
using UnityEngine;
using UnityEngine.Events;

namespace HotPlay.PecanUI.Sounds
{
    [RequireComponent(typeof(UIButton))]
    public class UIButtonSimpleSound : MonoBehaviour
    {
        [SerializeField]
        private string soundConfigPath = "";

        private async void OnEnable()
        {
            await UniTask.Yield(PlayerLoopTiming.Initialization);
            GetButtonEvent()?.AddListener(OnButtonClicked);
        }

        private void OnDisable()
        {
            GetButtonEvent()?.RemoveListener(OnButtonClicked);
        }

        private UnityEvent? GetButtonEvent()
        {
            if(!TryGetComponent<UIButton>(out var button))
            {
                return null;
            }

            return button.behaviours.AutoGetUnityEvent(UIBehaviour.Name.PointerLeftClick);
        }

        private void OnButtonClicked()
        {
            PecanServices.Instance.PecanSoundManager.PlayOnce(soundConfigPath);
        }

        private void OnValidate()
        {
            if (soundConfigPath == "")
            {
                soundConfigPath = "UIButton";
            }
        }
    }
}