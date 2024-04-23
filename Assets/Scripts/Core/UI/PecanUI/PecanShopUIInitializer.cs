using System.Collections.Generic;
using HotPlay.BoosterMath.Core.Player;
using HotPlay.PecanUI.Shop;
using HotPlay.QuickMath.Analytics;
using UnityEngine.Assertions;
using Zenject;

namespace HotPlay.BoosterMath.Core
{
    public class PecanShopUIInitializer : PecanUIInitializerBase
    {
        [Inject]
        private ShopDatabase shopDatabase;

        [Inject]
        private ThemeSelector themeSelector;

        [Inject]
        private ShopDataController shopDataController;
        
        [Inject]
        private CurrencyDataController currencyDataController;

        private ShopElementData[] themeShopData;
        private ShopElementData[] characterShopData;

        public override void Setup()
        {
            Assert.IsNotNull(Services);
            
            themeSelector.Select(shopDataController.EquippedTheme);
            Services.Events.InitSoftCurrency(currencyDataController.PermanentCurrency);

            Services.SetIsEnoughCurrencyFunction(IsEnoughCurrency);
            
            Services.SetIsOwnedThemeFunction(shopDataController.IsOwnedTheme);
            Services.SetIsOwnedCharacterFunction(shopDataController.IsOwnedCharacter);

            Services.SetIsEquippedThemeFunction(IsThemeEquipped);
            Services.SetIsEquippedCharacterFunction(IsCharacterEquipped);
            
            Services.SetGetThemeShopItemsFunction(GetThemeShopData);
            Services.SetGetCharacterShopItemsFunction(GetCharacterShopData);
            
            Services.Events.ItemPreviewEventsHandler.EquipThemeItem += OnThemeEquipped;
            Services.Events.ItemPreviewEventsHandler.EquipCharacterItem += OnCharacterEquipped;

            Services.Events.ItemPreviewEventsHandler.PurchaseThemeItem += OnThemePurchased;
            Services.Events.ItemPreviewEventsHandler.PurchaseCharacterItem += OnCharacterPurchased;
        }

        public override void Terminate()
        {
            Services.Events.ItemPreviewEventsHandler.EquipThemeItem -= OnThemeEquipped;
            Services.Events.ItemPreviewEventsHandler.EquipCharacterItem -= OnCharacterEquipped;
            
            Services.Events.ItemPreviewEventsHandler.PurchaseThemeItem -= OnThemePurchased;
            Services.Events.ItemPreviewEventsHandler.PurchaseCharacterItem -= OnCharacterPurchased;
        }

        private ShopElementData[] GetThemeShopData()
        {
            themeShopData ??= new ShopElementData[shopDatabase.ThemeData.Length];

            for (int i = 0; i < shopDatabase.ThemeData.Length; i++)
            {
                if (themeShopData[i] != null)
                    continue;
                
                var data = shopDatabase.ThemeData[i];
                themeShopData[i] = new ShopElementData(data.id, data.id, data.price, data.icon, ItemType.Theme);
            }

            return themeShopData;
        }

        private ShopElementData[] GetCharacterShopData()
        {
            characterShopData ??= new ShopElementData[shopDatabase.CharacterData.Length];

            for (int i = 0; i < shopDatabase.CharacterData.Length; i++)
            {
                if (characterShopData[i] != null)
                    continue;
                
                var data = shopDatabase.CharacterData[i];
                characterShopData[i] = new ShopElementData(data.characterData.id, data.characterData.id, data.price, data.icon, ItemType.Character);
            }

            return characterShopData;
        }
        
        private bool IsEnoughCurrency(int price)
        {
            return currencyDataController.PermanentCurrency >= price;
        }

        private bool IsThemeEquipped(string id)
        {
            return shopDataController.EquippedTheme == id;
        }
        
        private bool IsCharacterEquipped(string id)
        {
            return shopDataController.EquippedCharacter == id;
        }

        private void OnThemeEquipped(ShopElementData data)
        {
            themeSelector.Select(data.Id);
            shopDataController.EquipTheme(data.Id);
        }
        
        private void OnCharacterEquipped(ShopElementData data)
        {
            shopDataController.EquipCharacter(data.Id);
        }
        
        private void OnThemePurchased(ShopElementData data)
        {
            currencyDataController.Remove(data.Price);
            shopDataController.UnlockTheme(data.Id);

            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { AnalyticsParams.ResourceFlowType, AnalyticsParams.ResourceSpendType },
                { AnalyticsParams.CurrencyType, AnalyticsParams.Coin },
                { AnalyticsParams.Amount, data.Price },
                { AnalyticsParams.ItemType, AnalyticsParams.Theme },
                { AnalyticsParams.ItemID, data.Id }
            };
            AnalyticsServices.Instance.LogResourceEvent(parameters);
        }
        
        private void OnCharacterPurchased(ShopElementData data)
        {
            currencyDataController.Remove(data.Price);
            shopDataController.UnlockCharacter(data.Id);

            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { AnalyticsParams.ResourceFlowType, AnalyticsParams.ResourceSpendType },
                { AnalyticsParams.CurrencyType, AnalyticsParams.Coin },
                { AnalyticsParams.Amount, data.Price },
                { AnalyticsParams.ItemType, AnalyticsParams.Character },
                { AnalyticsParams.ItemID, data.Id }
            };
            AnalyticsServices.Instance.LogResourceEvent(parameters);
        }
    }
}