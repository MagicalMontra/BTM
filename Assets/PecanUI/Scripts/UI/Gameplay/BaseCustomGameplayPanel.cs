using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HotPlay.PecanUI.Gameplay
{
    public abstract class BaseCustomGameplayPanel : MonoBehaviour
    {
        public virtual void OnShowing()
        {
            // Do nothing
        }

        public virtual void OnVisible()
        {
            // Do nothing
        }

        public virtual void OnHide()
        {
            // Do nothing
        }

        public virtual void OnHidden()
        {
            // Do nothing
        }
    }
}
