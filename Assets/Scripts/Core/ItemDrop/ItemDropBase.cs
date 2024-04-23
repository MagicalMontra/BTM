using Cysharp.Threading.Tasks;
using DG.Tweening;
using HotPlay.BoosterMath.Core.Player;
using UnityEngine;
using Zenject;

namespace HotPlay.BoosterMath.Core
{

    public abstract class ItemDropBase : MonoBehaviour
    {
        public Shadow Shadow => shadow;
        
        [SerializeField]
        protected float moveSpeed = 6f;

        [SerializeField]
        protected SpriteRenderer renderer;

        [SerializeField]
        protected GameObject[] idleVfxs;
        
        [SerializeField]
        protected GameObject[] pickUpVfxs;
        
        [Inject]
        private readonly PlayerSpawner playerSpawner;
    
        protected bool isDespawning;
        
        private Tween tween;

        private Shadow shadow;

        private void Reinitialize(Vector3 position, Transform parent)
        {
            tween?.Kill();
            shadow = null;
            transform.SetParent(parent);
            transform.position = position;
            renderer.DOFade(1f, 0.1f);

            foreach (var vfx in idleVfxs)
            {
                vfx.SetActive(true);
            }

            foreach (var vfx in pickUpVfxs)
            {
                vfx.SetActive(false);
            }
            
            var randomX = Random.Range(-1, 1f);
            var randomVector = new Vector3(randomX, 0);
            tween = transform.DOJump(position + randomVector, 2, 1, 1f);
            OnReinitialize();
        }

        internal virtual void OnDespawned()
        {
            shadow = null;
        }

        internal virtual void OnReinitialize()
        {
            
        }

        private void Update()
        {
            if (!isDespawning)
                return;

            if (Mathf.Abs(transform.position.x - playerSpawner.Current.transform.position.x) <= 0.1f || transform.position.x <= playerSpawner.Current.transform.position.x)
            {
                isDespawning = false;
                return;
            }
            
            transform.position += Vector3.left * (moveSpeed * Time.deltaTime);
        }

        public void AttachShadow(Shadow shadow)
        {
            if (this.shadow != null)
                return;

            this.shadow = shadow;
        }
        
        public abstract UniTask Activate();

        protected async UniTask ActivatePickupVfx()
        {
            foreach (var vfx in pickUpVfxs)
            {
                vfx.SetActive(true);
            }

            await UniTask.Delay(500);
        }
        
        public class Pool : MonoMemoryPool<Vector3, Transform, ItemDropBase>
        {
            [Inject]
            public ItemDropTypeEnum Type { get; }

            [Inject]
            public Sprite Sprite { get; }

            protected override void Reinitialize(Vector3 position, Transform parent, ItemDropBase item)
            {
                item.Reinitialize(position, parent);
            }

            protected override void OnDespawned(ItemDropBase item)
            {
                base.OnDespawned(item);
                item.OnDespawned();
            }
        }
    }
}
