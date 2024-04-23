#nullable enable

using Cysharp.Threading.Tasks;
using Doozy.Runtime.Signals;
using Doozy.Runtime.UIManager.Components;
using UnityEngine;
using UnityEngine.Events;

namespace HotPlay.PecanUI.Sounds
{
    [RequireComponent(typeof(UIToggle))]
    public class UIToggleSimpleSound : MonoBehaviour
    {
        [SerializeField]
        private string soundConfigPath = "";

        private async void OnEnable()
        {
            await UniTask.Yield(PlayerLoopTiming.Initialization);
            GetToggleEvent()?.AddListener(OnToggleClicked);
        }

        private void OnDisable()
        {
            GetToggleEvent()?.RemoveListener(OnToggleClicked);
        }

        private UnityEvent? GetToggleEvent()
        {
            if (!TryGetComponent<UIToggle>(out var toggle))
            {
                return null;
            }

            var behaviour = toggle
                .behaviours
                .AddBehaviour(Doozy.Runtime.UIManager.UIBehaviour.Name.PointerLeftClick);

            var ev = behaviour.Event;

            if (ev == null)
            {
                behaviour.Event = new UnityEvent();
            }
            return behaviour.Event;
        }

        private void OnToggleClicked()
        {
            PecanServices.Instance.PecanSoundManager.PlayOnce(soundConfigPath);
        }

        private void OnValidate()
        {
            if (soundConfigPath == "")
            {
                soundConfigPath = "UIToggle";
            }
        }
    }
}