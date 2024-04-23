using System;
using System.Collections;
using System.Collections.Generic;
using Doozy.Runtime.Signals;
using HotPlay.PecanUI.Shop;
using UnityEngine;

namespace HotPlay.PecanUI.Events
{
    public class ShopEventsHandler : MonoBehaviour
    {
        public event Action<bool> CharacterToggleChanged;
        public event Action<bool> ThemeToggleClicked;
        public event Action<ShopElementData> PreviewItem;
        public event Action CloseButtonClicked;

        [SerializeField]
        private ShopDialog shopDialog;

        [SerializeField]
        private ItemPreviewDialog itemPreviewDialog;

        private SignalStream previewItemSignalStream;
        private SignalReceiver previewItemSignalReceiver;

        private void Start()
        {
            previewItemSignalStream = SignalStream.Get("Shop", "PreviewItem");
            previewItemSignalReceiver = new SignalReceiver().SetOnSignalCallback(OnPreviewItemSignal);
            previewItemSignalStream.ConnectReceiver(previewItemSignalReceiver);
        }

        public void InvokeCloseButtonClicked()
        {
            CloseButtonClicked?.Invoke();
        }

        public void InvokeCharacterToggleChanged(bool value)
        {
            CharacterToggleChanged?.Invoke(value);
        }

        public void InvokeThemeToggleChanged(bool value)
        {
            ThemeToggleClicked?.Invoke(value);
        }

        private void OnPreviewItemSignal(Signal signal)
        {
            ShopElementData itemData = signal.GetValueUnsafe<ShopElementData>();
            itemPreviewDialog.SetUp(itemData);
            PreviewItem?.Invoke(itemData);
        }
    }
}
