using Doozy.Runtime.UIManager;
using Doozy.Runtime.UIManager.Components;
using Doozy.Runtime.UIManager.Containers;
using HotPlay.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace HotPlay.PecanUI.Shop
{
    [Serializable]
    public class ShopElementData
    {
        public string Name => name;
        public string Id => id;
        public int Price => price;
        public Sprite Sprite => sprite;
        public bool IsOwned { get; private set; }
        public bool IsEquipped { get; private set; }

        public ItemType ItemType { get; private set; }

        [SerializeField]
        private string name;

        [SerializeField]
        private string id;

        [SerializeField]
        private int price;

        [SerializeField]
        private Sprite sprite;

        public ShopElementData(string id, string name, int price, Sprite sprite, ItemType itemType)
        {
            this.id = id;
            this.name = name;
            this.price = price;
            this.sprite = sprite;
            this.ItemType = itemType;
        }

        public void SetIsOwned(bool status)
        {
            IsOwned = status;
        }

        public void SetIsEquipped(bool status)
        {
            IsEquipped = status;
        }
    }

    public class ShopDialog : BaseDialog
    {
        [SerializeField]
        private ShopPanelData[] shopPanelData;

        [SerializeField]
        private RectTransform shopContent;

        [SerializeField]
        private float horizontalPadding;

        [SerializeField]
        private UIToggleGroup panelToggleGroup;

        [SerializeField]
        private UIButton closeButton;

        [SerializeField]
        private UIToggle characterToggle;

        [SerializeField]
        private UIToggle themeToggle;

        [SerializeField]
        private BackEventHandler backEventHandler;

        [Serializable]
        public class ShopPanelData
        {
            [FormerlySerializedAs("toggle")]
            public UIToggle Toggle;

            [FormerlySerializedAs("panel")]
            public ShopPanel Panel;

            public Image ToggleImage;
            public TextMeshProUGUI ToggleLabel;
        }

        private UIToggle currentToggle;
        private UIToggle defaultToggle;
        private UIContainer defaultPanel;
        private bool isClosing = false;

        private void Start()
        {
            var configs = PecanServices.Instance.Configs;
            closeButton.behaviours.AutoGetUnityEvent(UIBehaviour.Name.PointerLeftClick).AddListener(OnCloseButtonClicked);

            SetToggleState(characterToggle, configs.IsEnableCharacterTab, OnCharacterToggleValueChanged);
            SetToggleState(themeToggle, configs.IsEnableThemeTab, OnThemeToggleValueChanged);

            foreach (ShopPanelData data in shopPanelData)
            {
                var overrideSprite = data.Panel.Category switch
                {
                    ItemType.Character => configs.ShopTabCharacterImage,
                    ItemType.Theme => configs.ShopTabThemeImage,
                    _ => throw new NotImplementedException()
                };
                var overrideText = data.Panel.Category switch
                {
                    ItemType.Character => configs.ShopTabCharacterLabel.GetLocalizedString(),
                    ItemType.Theme => configs.ShopTabThemeLabel.GetLocalizedString(),
                    _ => throw new NotImplementedException()
                };

                if (overrideSprite != null)
                {
                    data.ToggleImage.sprite = overrideSprite;
                }

                if (!string.IsNullOrEmpty(overrideText))
                {
                    data.ToggleLabel.text = overrideText;
                }

                // Initialize shop category
                if (data.Panel.Category == configs.DefaultShopCategory)
                {
                    panelToggleGroup.FirstToggle = data.Toggle;
                    currentToggle = data.Toggle;
                    defaultToggle = currentToggle;
                    defaultPanel = data.Panel.GetComponent<UIContainer>();
#if PECAN_NAVIGATOR
                    view.AutoSelectTarget = data.Toggle.gameObject;
#endif
                }
            }

            UpdateContentWidth();

            static void SetToggleState(UIToggle toggle, bool isEnable, UnityAction<bool> valueChangedCallback)
            {
                toggle.gameObject.SetActive(isEnable);
                if (isEnable)
                {
                    toggle.OnValueChangedCallback.AddListener(valueChangedCallback);
                }
            }
        }

        private void OnCloseButtonClicked()
        {
            PecanServices.Instance.Events.ShopEventHandler.InvokeCloseButtonClicked();
            currentToggle = defaultToggle;
        }

        private void OnCharacterToggleValueChanged(bool value)
        {
            PecanServices.Instance.Events.ShopEventHandler.InvokeCharacterToggleChanged(value);
            if (value)
            {
                currentToggle = characterToggle;
            }
        }

        private void OnThemeToggleValueChanged(bool value)
        {
            PecanServices.Instance.Events.ShopEventHandler.InvokeThemeToggleChanged(value);
            if (value)
            {
                currentToggle = themeToggle;
            }
        }

        protected override void OnShowing()
        {
            backEventHandler.Subscribe("Shop", OnShopBackNotified);
            UpdateContentWidth();
            StartCoroutine(UpdateScroller());

            if (defaultPanel != null && currentToggle == defaultToggle)
            {
                defaultPanel.Show();
            }
        }

        private IEnumerator UpdateScroller()
        {
            yield return null;
            foreach (ShopPanelData data in shopPanelData)
            {
                data.Panel.RestoreScrollPosition();
            }
        }

        protected override void OnHidden()
        {
            base.OnHidden();
            backEventHandler.Unsubscribe("Shop", OnShopBackNotified);

            if (isClosing)
            {
                panelToggleGroup.FirstToggle = defaultToggle;
                defaultToggle.SetIsOn(true, false);
            }

            foreach (ShopPanelData data in shopPanelData)
            {
                if (isClosing)
                {
                    data.Panel.ResetScrollPosition();
                    if (data.Toggle != currentToggle)
                    {
                        data.Panel.GetComponent<UIContainer>().InstantHide();
                    }
                }
                else
                {
                    data.Panel.StoreScrollPosition();
                }
            }

            isClosing = false;
        }

        private void OnShopBackNotified()
        {
            isClosing = true;
        }

        private void UpdateContentWidth()
        {
            List<float> contentsWidth = new List<float>();
            foreach (ShopPanelData data in shopPanelData)
            {
                ShopPanel panel = data.Panel;
                panel.Init();
                contentsWidth.Add(panel.GetCellViewWidth());
            }

            float maxValue = contentsWidth.Max();
            shopContent.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, maxValue + (horizontalPadding * 2));
        }
    }
}
