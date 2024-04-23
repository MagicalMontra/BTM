using Doozy.Runtime.Signals;
using Doozy.Runtime.UIManager;
using Doozy.Runtime.UIManager.Components;
using HotPlay.PecanUI.Skin;
using HotPlay.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#if PECAN_NAVIGATOR
using static EnhancedUI.EnhancedScroller.EnhancedScroller;
#endif

namespace HotPlay.PecanUI.Shop
{
    public class ShopElement : MonoBehaviour
    {
        public RectTransform Rect => rect;

        [SerializeField]
        private Image itemImage;

        [SerializeField]
        private TextMeshProUGUI nameText;

        [SerializeField]
        private TextMeshProUGUI ownedText;

        [SerializeField]
        private TextMeshProUGUI priceText;

        [SerializeField]
        private Image checkmark;

        [SerializeField]
        private UIButton button;

        [SerializeField]
        private RectTransform rect;

        [SerializeField]
        private BaseSkinHandler[] skinHandler;

        private ShopElementData itemData;
        private bool isOwned;
        private bool isEquipped;
        private ShopPanel shopPanel;
        private ShopCellView shopCellView;

        public void Init(ShopElementData data, ShopPanel shopPanel, ShopCellView shopCellView)
        {
            this.itemData = data;
            this.isOwned = data.IsOwned;
            this.isEquipped = data.IsEquipped;
            this.shopPanel = shopPanel;
            this.shopCellView = shopCellView;

            nameText.text = (PecanServices.Instance.Configs.ShopItemCategory + data.Name).GetLocalizedString();
            priceText.text = data.Price.ToString("N0");
            itemImage.sprite = data.Sprite;
            itemImage.gameObject.SetActive(itemImage.sprite != null);

            ownedText.gameObject.SetActive(isOwned);
            priceText.gameObject.SetActive(!isOwned);
            checkmark.gameObject.SetActive(isEquipped);
        }

        private void Start()
        {
            foreach (var skin in skinHandler)
            {
                skin.UpdateSkin(PecanServices.Instance.SkinConfigs);
            }
            
            itemImage.gameObject.SetActive(itemImage.sprite != null);
            button.behaviours.AutoGetUnityEvent(UIBehaviour.Name.PointerLeftClick).AddListener(OnClicked);
            button.selectedState.stateEvent.Event.AddListener(OnSelected);
        }

        private void OnClicked()
        {
            //Send signal to open item preview
            Signal.Send("Shop", "PreviewItem", itemData, "message");
        }

        private void OnSelected()
        {
#if PECAN_NAVIGATOR
            if(shopPanel != null && shopCellView != null)
            {
                shopPanel.Scroller.JumpToDataIndex(shopCellView.dataIndex, scrollerOffset:0.1f, tweenType:TweenType.linear, tweenTime: 0.2f);
            }
#endif
        }
    }
}
