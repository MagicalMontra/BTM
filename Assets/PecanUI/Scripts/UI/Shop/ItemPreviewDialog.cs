using Cysharp.Threading.Tasks;
using Doozy.Runtime.Reactor.Animators;
using Doozy.Runtime.UIManager;
using Doozy.Runtime.UIManager.Components;
using HotPlay.PecanUI.Skin;
using HotPlay.Utilities;
using Spine.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HotPlay.PecanUI.Shop
{
    public class ItemPreviewDialog : BaseDialog
    {
        private const string unlockItemLocalizeKey = "Shop/UNLOCK ITEM?";
        private const string congratulationLocalizeKey = "Generic/CONGRATULATIONS!";
        private const string useItemNowLocalizeKey = "Shop/USE ITEM NOW?";
        private const string itemInUseLocalizeKey = "Shop/ITEM IN-USE";
        private const string cancelLocalizeKey = "Generic/Cancel";
        private const string closeLocalizeKey = "Generic/Close";

        [SerializeField]
        private TextMeshProUGUI headerText;

        [SerializeField]
        private Image image;

        [SerializeField]
        private TextMeshProUGUI nameText;

        [SerializeField]
        private TextMeshProUGUI priceText;

        [SerializeField]
        private TextMeshProUGUI ownedText;

        [SerializeField]
        private UIButton purchaseButton;

        [SerializeField]
        private UIButton purchaseCancelButton;

        [SerializeField]
        private UIButton equipButton;

        [SerializeField]
        private GameObject equipButtonHolder;

        [SerializeField]
        private UIButton equipCancelButton;

        [SerializeField]
        private TextMeshProUGUI equipCancelButtonText;

        [SerializeField]
        private UIButton closeButton;
        
        [SerializeField]
        private SkeletonGraphic purchaseAnimation;

        [SerializeField]
        private AnimationReferenceAsset purchaseAnimationRef;

        [SerializeField]
        private GameObject purchaseItemViewButtonsPanel;

        [SerializeField]
        private GameObject unlockedItemViewButtonPanel;

        [SerializeField]
        private GameObject equipItemViewButtonPanel;

        [SerializeField]
        private SkinTMPHandler pricePositiveSkinHandler;
        
        [SerializeField]
        private SkinTMPHandler priceNegativeSkinHandler;

        private UIButton[] uiButtons;
        private ShopElementData currentItemData;

        protected override void Awake()
        {
            base.Awake();
            uiButtons = GetComponentsInChildren<UIButton>();
            purchaseButton.behaviours.AutoGetUnityEvent(UIBehaviour.Name.PointerLeftClick).AddListener(OnPurchaseButtonClicked);
            equipButton.behaviours.AutoGetUnityEvent(UIBehaviour.Name.PointerLeftClick).AddListener(OnEquipButtonClicked);
            closeButton.behaviours.AutoGetUnityEvent(UIBehaviour.Name.PointerLeftClick).AddListener(OnCloseButtonClicked);
            equipCancelButton.behaviours.AutoGetUnityEvent(UIBehaviour.Name.PointerLeftClick).AddListener(OnCancelButtonClicked);
            purchaseCancelButton.behaviours.AutoGetUnityEvent(UIBehaviour.Name.PointerLeftClick).AddListener(OnCancelButtonClicked);
        }

        private void OnDestroy()
        {
            purchaseButton.behaviours.AutoGetUnityEvent(UIBehaviour.Name.PointerLeftClick).RemoveListener(OnPurchaseButtonClicked);
            equipButton.behaviours.AutoGetUnityEvent(UIBehaviour.Name.PointerLeftClick).RemoveListener(OnEquipButtonClicked);
            closeButton.behaviours.AutoGetUnityEvent(UIBehaviour.Name.PointerLeftClick).RemoveListener(OnCloseButtonClicked);
            equipCancelButton.behaviours.AutoGetUnityEvent(UIBehaviour.Name.PointerLeftClick).RemoveListener(OnCancelButtonClicked);
            purchaseCancelButton.behaviours.AutoGetUnityEvent(UIBehaviour.Name.PointerLeftClick).RemoveListener(OnCancelButtonClicked);
        }

        public void SetUp(ShopElementData itemData)
        {
            SetButtonsInteractable(true);
            currentItemData = itemData;
            image.sprite = itemData.Sprite;
            nameText.text = (PecanServices.Instance.Configs.ShopItemCategory + itemData.Name).GetLocalizedString();

            if (!itemData.IsOwned)
            {
                SetPurchaseItemView();
            }
            else
            {
                SetEquipItemView();
            }
        }

        private void SetPurchaseItemView()
        {
            if (currentItemData == null)
            {
                return;
            }

            view.AutoSelectTarget = purchaseCancelButton.gameObject;

#if PECAN_NAVIGATOR
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(purchaseCancelButton.gameObject);
#endif
            headerText.text = unlockItemLocalizeKey.GetLocalizedString();
            bool isEnoughCurrency = IsEnoughCurrency(currentItemData.Price);
            var skinConfigs = PecanServices.Instance.SkinConfigs;
            var priceSkin = isEnoughCurrency ? pricePositiveSkinHandler : priceNegativeSkinHandler;
            purchaseButton.interactable = isEnoughCurrency;
            priceSkin.UpdateSkin(skinConfigs);

            purchaseItemViewButtonsPanel.SetActive(true);
            unlockedItemViewButtonPanel.SetActive(false);
            equipItemViewButtonPanel.SetActive(false);

            priceText.gameObject.SetActive(true);
            priceText.text = currentItemData.Price.ToString("N0");
            ownedText.gameObject.SetActive(false);
            purchaseAnimation.freeze = true;
            purchaseAnimation.gameObject.SetActive(false);
        }

        private void SetEquipItemView()
        {
            view.AutoSelectTarget = equipCancelButton.gameObject;

#if PECAN_NAVIGATOR
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(equipCancelButton.gameObject);
#endif
            if (currentItemData.IsEquipped)
            {
                headerText.text = itemInUseLocalizeKey.GetLocalizedString();
                equipCancelButtonText.text = closeLocalizeKey.GetLocalizedString();
            }
            else
            {
                headerText.text = useItemNowLocalizeKey.GetLocalizedString();
                equipCancelButtonText.text = cancelLocalizeKey.GetLocalizedString();
            }

            purchaseItemViewButtonsPanel.SetActive(false);
            unlockedItemViewButtonPanel.SetActive(false);
            equipItemViewButtonPanel.SetActive(true);
            equipButtonHolder.SetActive(!currentItemData.IsEquipped);

            priceText.gameObject.SetActive(false);
            ownedText.gameObject.SetActive(true);
            purchaseAnimation.freeze = true;
            purchaseAnimation.gameObject.SetActive(false);
        }

        private void SetUnlockItemView()
        {
            view.AutoSelectTarget = closeButton.gameObject;

#if PECAN_NAVIGATOR
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(closeButton.gameObject);
#endif
            headerText.text = congratulationLocalizeKey.GetLocalizedString();
            purchaseItemViewButtonsPanel.SetActive(false);
            unlockedItemViewButtonPanel.SetActive(true);
            equipItemViewButtonPanel.SetActive(false);

            priceText.gameObject.SetActive(false);
            ownedText.gameObject.SetActive(true);

            purchaseAnimation.AnimationState.ClearTrack(0);
            purchaseAnimation.AnimationState.SetAnimation(0, purchaseAnimationRef, true);
            purchaseAnimation.freeze = false;
            purchaseAnimation.gameObject.SetActive(true);
        }

        private bool IsEnoughCurrency(int price)
        {
            return PecanServices.Instance.IsEnoughCurrency(price);
        }

        // To prevent equip/cancel/close button spamming
        private void SetButtonsInteractable(bool interactable)
        {
            foreach (var button in uiButtons)
            {
                button.interactable = interactable;
            }
        }
        
        private void OnPurchaseButtonClicked()
        {
            PecanServices.Instance.Events.ItemPreviewEventsHandler.InvokePurchaseItemEvent(currentItemData);
            SetUnlockItemView();
        }

        private void OnEquipButtonClicked()
        {
            PecanServices.Instance.Events.ItemPreviewEventsHandler.InvokeEquipItemEvent(currentItemData);
            SetButtonsInteractable(false);
        }

        //Close button from unlocked view
        private void OnCloseButtonClicked()
        {
            SetEquipItemView();
            PecanServices.Instance.Events.ItemPreviewEventsHandler.InvokeCloseButtonClickedEvent();
        }

        private void OnCancelButtonClicked()
        {
            PecanServices.Instance.Events.ItemPreviewEventsHandler.InvokeCancelButtonClickedEvent();
            SetButtonsInteractable(false);
        }

        protected override void RecoveryNavigator()
        {
            if (closeButton.gameObject.activeInHierarchy)
            {
                UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(closeButton.gameObject);
            }
            else if (equipCancelButton.gameObject.activeInHierarchy)
            {
                UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(equipCancelButton.gameObject);
            }
            else if (purchaseCancelButton.gameObject.activeInHierarchy)
            {
                UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(purchaseCancelButton.gameObject);
            }
        }
    }
}
