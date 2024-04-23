using System;
using System.Collections;
using System.Collections.Generic;
using HotPlay.PecanUI.Analytic;
using HotPlay.PecanUI.Shop;
using UnityEngine;
using ItemType = HotPlay.PecanUI.Shop.ItemType;

namespace HotPlay.PecanUI.Events
{
    public class ItemPreviewEventsHandler : MonoBehaviour
    {
        public event Action<ShopElementData> EquipCharacterItem;
        public event Action<ShopElementData> PurchaseCharacterItem;
        public event Action<ShopElementData> EquipThemeItem;
        public event Action<ShopElementData> PurchaseThemeItem;
        public event Action CancelButtonClicked;

        //Close button from unlocked view
        public event Action CloseButtonClicked;

        private bool isJustPurchased;

        private IAnalyticEvent<DesignEventData<int>, int> equipEvent;
        private IAnalyticEvent<ResourceEventData, ShopElementData> purchaseEvent;

        public void InvokeEquipItemEvent(ShopElementData data)
        {
            if (data.ItemType == ItemType.Character)
            {
                EquipCharacterItem?.Invoke(data);
            }
            else
            {
                EquipThemeItem?.Invoke(data);
            }
            
            equipEvent ??= new IntAnalyticDesignEvent($"setting:{data.ItemType}:{data.Id}");
            var value = isJustPurchased ? 1 : 0;
            PecanServices.Instance.Analytic.TryLog(value, equipEvent);
        }

        public void InvokePurchaseItemEvent(ShopElementData data)
        {
            if (data.ItemType == ItemType.Character)
            {
                PurchaseCharacterItem?.Invoke(data);
            }
            else
            {
                PurchaseThemeItem?.Invoke(data);
            }

            isJustPurchased = true;
            purchaseEvent ??= new ShopItemPurchaseAnalyticEvent();
            PecanServices.Instance.Analytic.TryLog(data, purchaseEvent);
        }

        public void InvokeCancelButtonClickedEvent()
        {
            isJustPurchased = false;
            CancelButtonClicked?.Invoke();
        }

        public void InvokeCloseButtonClickedEvent()
        {
            isJustPurchased = false;
            CloseButtonClicked?.Invoke();
        }
    }
}
