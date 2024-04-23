using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HotPlay.PecanUI
{
    [Serializable]
    public class ItemPerRowData
    {
        [SerializeField]
        private int horizontalItemAmount;

        [SerializeField]
        private int verticalItemAmount;

        public int GetItemPerRow()
        {
            float ratio = (float)Screen.width / (float)Screen.height;
            return ratio >= 1 ? horizontalItemAmount : verticalItemAmount;
        }
    }
}
