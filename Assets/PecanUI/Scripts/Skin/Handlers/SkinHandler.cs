using Sirenix.OdinInspector;
using Doozy.Runtime.Reactor;
using Doozy.Runtime.Reactor.Reactions;
using Doozy.Runtime.UIManager.Animators;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.UI;

namespace HotPlay.PecanUI.Skin
{
    public abstract class BaseSkinHandler : MonoBehaviour
    {
        [SerializeField]
        protected string skinKey;
        public string SkinKey => skinKey;
        
        #if UNITY_EDITOR
        [Button("GoToConfig")]
        private void GoToConfig()
        {
            var config = PecanServices.Instance.SkinConfigs;
            config.searchKeyword = skinKey;
            config.availableKeySearch = skinKey;
            Selection.activeObject = config;
        }
        #endif

        public virtual void UpdateSkin(PecanSkinConfigs configs)
        {
            
        }
        
        public virtual void UpdateSkin(SkinConfigs configs)
        {
        }

        public virtual void LoadSkin(SkinData skinData)
        {
        }
    }

    public abstract class SkinHandler<T> : BaseSkinHandler where T : SkinData
    {
        public override void UpdateSkin(SkinConfigs configs)
        {
            if (string.IsNullOrEmpty(skinKey))
            {
                return;
            }

            var skinData = configs.GetSkinData<T>(skinKey);

            if (skinData != null)
            {
                UpdateSkin(skinData);
            }
        }

        public override void LoadSkin(SkinData skinData)
        {
            T data = skinData as T;
            Debug.Assert(data != null, $"Skin data [{skinKey}] of [{gameObject.name} is incorrect]");
            LoadSkin(skinData as T);
        }

        protected static void ApplyColor(Image image, Color color)
        {
            image.color = color;
            if (image.TryGetComponent<UISelectableColorAnimator>(out var colorAnimator))
            {
                SetAnimateColor(colorAnimator, color);
            }
        }

        protected void ApplyColor(TextMeshProUGUI text, Color color)
        {
            text.color = color;
            if (text.TryGetComponent<UISelectableColorAnimator>(out var colorAnimator))
            {
                SetAnimateColor(colorAnimator, color);
            }
        }

        protected static void SetAnimateColor(UISelectableColorAnimator colorAnimator, Color color)
        {
            SetColor(colorAnimator.normalAnimation.animation, color);
            SetColor(colorAnimator.pressedAnimation.animation, color);
            SetColor(colorAnimator.disabledAnimation.animation, color);
            SetColor(colorAnimator.highlightedAnimation.animation, color);
            SetColor(colorAnimator.selectedAnimation.animation, color);

            static void SetColor(ColorTargetReaction target, Color color)
            {
                var fromColor = target.GetValue(
                    ReferenceValue.CurrentValue,
                    color,
                    color,
                    Color.black,
                    target.fromHueOffset,
                    target.fromSaturationOffset,
                    target.fromLightnessOffset,
                    target.fromAlphaOffset
                );
                target.fromReferenceValue = ReferenceValue.CustomValue;
                target.fromCustomValue = fromColor;

                var toColor = target.GetValue(
                    ReferenceValue.CurrentValue,
                    color,
                    color,
                    Color.black,
                    target.toHueOffset,
                    target.toSaturationOffset,
                    target.toLightnessOffset,
                    target.toAlphaOffset
                );
                target.toReferenceValue = ReferenceValue.CustomValue;
                target.toCustomValue = toColor;
            }
        }

        public abstract void UpdateSkin(T skinData);

        protected abstract void LoadSkin(T skinData);
    }
}
