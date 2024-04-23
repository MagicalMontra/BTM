using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace HotPlay.BoosterMath.Core.UI
{
    public interface IGameUIComponent
    {
        GameObject gameObject { get; }

        UniTask Show(string sText);

        UniTask Hide();
        
        public class Pool : MemoryPool<Transform, IGameUIComponent>
        {
            [Inject]
            private Transform parent;
            
            protected override void Reinitialize(Transform parent, IGameUIComponent item)
            {
                item.gameObject.transform.SetParent(parent);
                item.gameObject.transform.localScale = Vector3.one;
            }
            
            protected override void OnDespawned(IGameUIComponent item)
            {
                item.gameObject.transform.SetParent(parent);
                item.gameObject.SetActive(false);
            }
        }
    }
}