using System;
using UnityEngine;

namespace HotPlay.BoosterMath.Core.Player
{
    [Serializable]
    public class CharacterData
    {
        public string id;
        public GameObject prefab;
    }

    [Serializable]
    public class CharacterShopData
    {
        public int price;
        public string name;
        public Sprite icon;
        public CharacterData characterData;
    }
}