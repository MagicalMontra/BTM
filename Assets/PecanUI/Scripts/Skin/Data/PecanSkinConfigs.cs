using HotPlay.Utilities;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace HotPlay.PecanUI.Skin
{
    [CreateAssetMenu(menuName = "Pecan UI/Create PecanSkinConfigs", fileName = "PecanSkinConfigs", order = 0)]
    public class PecanSkinConfigs : ScriptableObject
    {
        public PecanServices PecanServices => pecanServices;
        public ImageSkinConfigs ImageSkin => imageSkin;
        public TMPSkinConfigs TMPSkin => tmpSkin;

        public string selectedKey;
        public string searchKeyword;
        public string availableKeySearch;
        
        public TMPSkinData newTMPSkin;
        public ImageSkinData newImageSkin;
        
        [SerializeField]
        private TMPSkinConfigs tmpSkin;
        [SerializeField]
        private ImageSkinConfigs imageSkin;

        [SerializeField]
        private PecanServices pecanServices;

        [Serializable]
        public class TMPSkinConfigs : ReferenceClassTree<TMPSkinData>
        {
            
        }

        [Serializable]
        public class ImageSkinConfigs : ReferenceClassTree<ImageSkinData>
        {
            
        }
    }
}