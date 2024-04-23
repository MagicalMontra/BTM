using System;
using HotPlay.BoosterMath.Core.Player;
using HotPlay.BoosterMath.Core.UI;
using UnityEngine;

namespace HotPlay.BoosterMath.Core
{
    [CreateAssetMenu(menuName = "HotPlay/BoosterMath/Data/Create ShopDatabase", fileName = "ShopDatabase", order = 0)]
    public class ShopDatabase : ScriptableObject
    {
        public ThemeData[] ThemeData => themeData;
        
        public CharacterShopData[] CharacterData => characterData; 
        
        [SerializeField]
        private ThemeData[] themeData;
        
        [SerializeField]
        private CharacterShopData[] characterData;
    }

    [Serializable]
    public class ThemeData
    {
        public string id;
        public string name;
        public int price;
        public Sprite icon;
        public AdBanner adBanner;
        public AnswerPanel answerPanel;
        public CharacterData[] enemies;
    }
}