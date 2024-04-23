using Cysharp.Threading.Tasks;
using Spine.Unity;
using UnityEngine;
using Zenject;

namespace HotPlay.BoosterMath.Core.Character
{
    public class CharacterInstaller : MonoInstaller<CharacterInstaller>
    {
        [SerializeField]
        private ICharacter.AttackTypeEnum attackType;

        [SerializeField]
        private bool isFloating;
        
        [SerializeField]
        private string id;
        
        [SerializeField]
        private int maxHealth = 3;
        
        [SerializeField]
        private int attackPower = 1;

        [SerializeField]
        private WorldSpaceVFXBase attackVfxPrefab;
        
        [SerializeField]
        private WorldSpaceVFXBase getHitVfxPrefab;
        
        [SerializeField]
        private AnimationReferenceAsset idleAnimRef = default!;

        [SerializeField]
        private AnimationReferenceAsset walkAnimRef = default!;

        [SerializeField]
        private AnimationReferenceAsset attackAnimRef = default!;

        [SerializeField]
        private AnimationReferenceAsset getHitAnimRef = default!;

        [SerializeField]
        private AnimationReferenceAsset deathAnimRef = default!;

        private const string maxHealthId = "MaxHealth";
        
        private const string attackVFXId = "AttackVFX";
        
        private const string getHitVfxId = "GetHitVFX";
        
        private const string attackPowerId = "AttackPower";
        
        private const string idleAnimRefId = "Idle";
        
        private const string walkAnimRefId = "Walk";
        
        private const string attackAnimRefId = "Attack";

        private const string getHitAnimRefId = "GetHit";
        
        private const string deathAnimRefId = "Death";

        public override void InstallBindings()
        {
            Container.Bind<string>().FromInstance(id).AsSingle();
            if (attackVfxPrefab != null)
                Container.Bind<WorldSpaceVFXBase>().WithId(attackVFXId).FromInstance(attackVfxPrefab).AsTransient();
            
            Container.Bind<WorldSpaceVFXBase>().WithId(getHitVfxId).FromInstance(getHitVfxPrefab).AsTransient();
            
            Container.Bind<bool>().FromInstance(isFloating).AsSingle();
            Container.Bind<ICharacter.AttackTypeEnum>().FromInstance(attackType).AsSingle();
            Container.Bind<int>().WithId(maxHealthId).FromInstance(maxHealth).AsTransient();
            Container.Bind<int>().WithId(attackPowerId).FromInstance(attackPower).AsTransient();

            Container.Bind<IAnimator>().WithId(idleAnimRefId).FromSubContainerResolve().ByMethod(InstallIdleAnimator).AsTransient();
            Container.Bind<IAnimator>().WithId(walkAnimRefId).FromSubContainerResolve().ByMethod(InstallWalkAnimator).AsTransient();
            Container.Bind<IAnimator>().WithId(attackAnimRefId).FromSubContainerResolve().ByMethod(InstallAttackAnimator).AsTransient();
            Container.Bind<IAnimator>().WithId(getHitAnimRefId).FromSubContainerResolve().ByMethod(InstallGetHitAnimator).AsTransient();
            Container.Bind<IAnimator>().WithId(deathAnimRefId).FromSubContainerResolve().ByMethod(InstallDeathAnimator).AsTransient();
        }
        
        private void InstallIdleAnimator(DiContainer subContainer)
        {
            subContainer.Bind<IAnimator>().To<LoopSpineAnimator>().AsSingle();
            subContainer.Bind<AnimationReferenceAsset>().FromInstance(idleAnimRef).AsSingle();
        }
        
        private void InstallWalkAnimator(DiContainer subContainer)
        {
            subContainer.Bind<IAnimator>().To<LoopSpineAnimator>().AsSingle();
            subContainer.Bind<AnimationReferenceAsset>().FromInstance(walkAnimRef).AsSingle();
        }
        
        private void InstallAttackAnimator(DiContainer subContainer)
        {
            subContainer.Bind<float>().FromInstance(0.95f).AsSingle();
            subContainer.Bind<IAnimator>().To<OneshotSpineAnimator>().AsSingle();
            subContainer.Bind<AnimationReferenceAsset>().FromInstance(attackAnimRef).AsSingle();
        }

        private void InstallGetHitAnimator(DiContainer subContainer)
        {
            subContainer.Bind<IAnimator>().To<OneshotSpineAnimator>().AsSingle();
            subContainer.Bind<AnimationReferenceAsset>().FromInstance(getHitAnimRef).AsSingle();
        }
        
        private void InstallDeathAnimator(DiContainer subContainer)
        {
            subContainer.Bind<IAnimator>().To<OneshotSpineAnimator>().AsSingle();
            subContainer.Bind<AnimationReferenceAsset>().FromInstance(deathAnimRef).AsSingle();
        }
    }
}