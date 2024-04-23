using System.Threading;
using Cysharp.Threading.Tasks;
using HotPlay.PecanUI;
using HotPlay.Utilities;
using Spine.Unity;
using UnityEngine;
using Zenject;

namespace HotPlay.BoosterMath.Core.Character
{
    public class WorldSpaceSpineVFX : WorldSpaceVFXBase
    {
        [InjectOptional]
        private float delayTimeMultiplier = 1f;
        
        [Inject]
        private PecanServices pecanServices;
        
        [Inject]
        private PauseController pauseController;
        
        [SerializeField]
        internal SkeletonAnimation skeletonAnim;

        [SerializeField]
        internal AnimationReferenceAsset animationReference;

        private float cacheTimeScale;
        
        private void Start()
        {
            cacheTimeScale = skeletonAnim.timeScale;
            pauseController.OnPaused += OnGamePaused;
            pecanServices.Events.GameplayEventsHandler.Play += OnGameStarted;
            pecanServices.Events.GameplayEventsHandler.Restart += OnGameStarted;
        }

        public void OnDestroy()
        {
            pauseController.OnPaused -= OnGamePaused;
            pecanServices.Events.GameplayEventsHandler.Play -= OnGameStarted;
            pecanServices.Events.GameplayEventsHandler.Restart -= OnGameStarted;
        }
        private void OnGamePaused(bool isPaused)
        {
            skeletonAnim.timeScale = isPaused ? 0 : cacheTimeScale;
        }

        private void OnGameStarted()
        {
            skeletonAnim.timeScale = cacheTimeScale;
        }
        
        public override async UniTask Activate()
        {
            skeletonAnim.state.SetAnimation(0, animationReference, false);
            await UniTask.Delay((animationReference.Animation.Duration * delayTimeMultiplier).GetDurationMS());
        }
        
        public override async UniTask Activate(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                await UniTask.Yield();
                return;
            }
            
            skeletonAnim.state.SetAnimation(0, animationReference, false);
            await UniTask.Delay((animationReference.Animation.Duration * delayTimeMultiplier).GetDurationMS(), DelayType.DeltaTime, PlayerLoopTiming.Update, cancellationToken);
            
            if (cancellationToken.IsCancellationRequested)
                skeletonAnim.state.SetEmptyAnimation(0, 0);
        }

        public override void Deactivate()
        {
            Destroy(gameObject);
        }
    }
}