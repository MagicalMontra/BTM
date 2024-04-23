using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Spine.Unity;
using UnityEngine;
using Zenject;

namespace HotPlay.BoosterMath.Core.Character
{
    public abstract class SpineAnimator : IAnimator, IInitializable, IDisposable
    {
        [Inject]
        internal SkeletonAnimation SkeletonAnim { get; }
        
        [Inject]
        internal AnimationReferenceAsset AnimationReference { get; }

        private float cacheTimeScale;
        
        public void Initialize()
        {
            //TODO: sub pause
            cacheTimeScale = SkeletonAnim.timeScale;
        }

        public void Dispose()
        {
            //TODO: unsub pause
        }

        public abstract UniTask Play(CancellationToken cancellationToken);

        private void OnGamePaused(bool isPaused)
        {
            SkeletonAnim.timeScale = isPaused ? 0 : cacheTimeScale;
        }

        private void OnGameStarted()
        {
            SkeletonAnim.timeScale = cacheTimeScale;
        }
    }
}