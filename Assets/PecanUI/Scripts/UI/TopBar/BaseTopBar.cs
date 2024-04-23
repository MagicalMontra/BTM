using Doozy.Runtime.UIManager.Containers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HotPlay.PecanUI
{
    public abstract class BaseTopBar : MonoBehaviour
    {
        [SerializeField]
        private UIContainer uiContainer;
        public UIContainer UIContainer => uiContainer;
    }
}
