#nullable enable

using Doozy.Runtime.UIManager;
using UnityEngine.Events;
using static Doozy.Runtime.UIManager.UIBehaviour;

namespace HotPlay.Utilities
{
    public static class UIBehavioursExtension
    {
        public static UnityEvent AutoGetUnityEvent(this UIBehaviours uiBehaviour, Name name)
        {
            var behaviour = uiBehaviour.AddBehaviour(name);
            var ev = behaviour.Event;

            if (ev == null)
            {
                behaviour.Event = new UnityEvent();
            }
            return behaviour.Event;
        }
    }
}