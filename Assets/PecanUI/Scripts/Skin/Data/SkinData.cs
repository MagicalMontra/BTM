using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace HotPlay.PecanUI.Skin
{
    [Serializable]
    public class SkinData
    {
    }

    [Serializable]
    public class ImageSkinData : SkinData
    {
        public Color Color;
    }

    [Serializable]
    public class PanelSkinData : SkinData
    {
        public Color BackgroundColor;
        public Color OutlineColor;
    }

    [Serializable]
    public class TMPSkinData : SkinData
    {
        public Color FontColor;
        public TMP_FontAsset FontAsset;
        [ShowIf("FontAsset")]
        [AssetSelector(Filter = "SDF")]
        [ValidateInput("ValidateMaterial", "Material is missing or name not machted with font asset. Fallback to use default if not support.", messageType:InfoMessageType.Warning, ContinuousValidationCheck = true)]
        public Material PresetMaterial;

#if UNITY_EDITOR
        /// <summary>
        /// Editor: Use to validate material name with font asset
        /// </summary>
        private bool ValidateMaterial()
        {
            if (FontAsset == null)
            {
                PresetMaterial = null;
            }
            return FontAsset == null || (FontAsset != null && PresetMaterial != null && PresetMaterial.name.StartsWith(FontAsset.name));
        }
#endif
    }

    [Serializable]
    public class ButtonSkinData : SkinData
    {
        public Color StrokeColor;
        public Color BottomColor;
        public Color TopColor;
        public Color HighlightColor;
        public Color NavigatorColor;
        public Color IconColor;
    }

    [Serializable]
    public class ShopTabSkinData : SkinData
    {
        public Color StrokeColor;
        public Color BottomColor;
        public Color TopColor;
        public Color NavigatorColor;
        public Color IconColor;
        public Color DepthColor;
        public Color TabBorderColor;
        public Color TabActiveBorderColor;

        [HideLabel]
        public TMPSkinData TabLabel;
    }

    [Serializable]
    public class ShopElementSkinData : SkinData
    {
        public Color StrokeColor;
        public Color BottomColor;
        public Color TopColor;
        public Color DepthColor;
        public Color NavigatorColor;
        public Color NameBackground;
        public Color PriceBackground;
    }

    [Serializable]
    public class LeaderboardTagSkinData : SkinData
    {
        public Color OutlineColor;
        public Color BottomColor;
        public Color TopColor;
        public Color DepthColor;

        [HideLabel]
        public TMPSkinData Label;
    }

    [Serializable]
    public class LoginElementSkinData : SkinData
    {
        public Color StrokeColor;
        public Color BottomColor;
        public Color TopColor;
        public Color DepthColor;
        public Color NavigatorColor;
        public Color DayBarColor;
    }
}
