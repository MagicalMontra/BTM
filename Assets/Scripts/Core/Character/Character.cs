using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Spine.Unity;
using UnityEngine;
using Zenject;

namespace HotPlay.BoosterMath.Core.Character
{
    public class Character : MonoBehaviour, ICharacter
    {
        [Inject]
        public ICharacter.AttackTypeEnum AttackType { get; }
        
        [Inject]
        public bool IsFloating { get; }

        [Inject]
        public string Id { get; }
        
        [Inject(Id = "AttackPower")]
        public int AttackPower { get; }
        
        [Inject(Id = "MaxHealth")]
        public int MaxHealth { get; }
        
        public int CurrentHealth { get; internal set; }
        
        [Inject]
        public MeshRenderer Renderer { get; }

        [InjectOptional(Id = "AttackVFX")]
        public WorldSpaceVFXBase AttackVfx { get; }
        
        [Inject(Id = "GetHitVFX")]
        public WorldSpaceVFXBase GetHitVfx { get; }

        [Inject(Id = "Idle")]
        internal readonly IAnimator idleAnimator;
        
        [Inject(Id = "Walk")]
        internal readonly IAnimator walkAnimator;

        [Inject(Id = "Death")]
        internal readonly IAnimator deathAnimator;
        
        [Inject(Id = "Attack")]
        internal readonly IAnimator attackAnimator;

        [Inject(Id = "GetHit")]
        internal readonly IAnimator getHitAnimator;
        
        [Inject]
        internal readonly SkeletonAnimation skeletonAnim;

        [SerializeField]
        private bool fadeOnDeath;

        public void Dispose()
        {
            Destroy(gameObject);
        }

        public void Reinitialize()
        {
            CurrentHealth = MaxHealth;
            skeletonAnim.skeleton.A = 1f;
        }

        public void Idle(CancellationToken cancellationToken)
        {
            idleAnimator.Play(cancellationToken).Forget();
        }

        public void Walk(CancellationToken cancellationToken)
        {
            walkAnimator.Play(cancellationToken).Forget();
        }

        public void Heal(int amount)
        {
            CurrentHealth = Mathf.Clamp(CurrentHealth + amount, 1, MaxHealth);
        }

        public async UniTask Attack(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                await UniTask.Yield();
                return;
            }
            
            await attackAnimator.Play(cancellationToken);
            
            if (cancellationToken.IsCancellationRequested)
                Idle(cancellationToken);
        }

        public async UniTask GetHit(int damage, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                await UniTask.Yield();
                return;
            }
            
            CurrentHealth = Mathf.Clamp(CurrentHealth - damage, 0, MaxHealth);

            if (CurrentHealth > 0)
            {
                await getHitAnimator.Play(cancellationToken);
                return;
            }
            
            await Death(cancellationToken);
            
            if (cancellationToken.IsCancellationRequested)
            {
                await UniTask.Yield();
                return;
            }
            
            if (fadeOnDeath)
                DOTween.To(() => skeletonAnim.skeleton.A, x => skeletonAnim.skeleton.A = x, 0, 0.5f);
        }

        public async UniTask Death(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                await UniTask.Yield();
                return;
            }
            
            await deathAnimator.Play(cancellationToken); 
        }
    }
}