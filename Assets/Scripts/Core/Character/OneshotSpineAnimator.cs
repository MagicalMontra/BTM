using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using HotPlay.Utilities;
using Zenject;

namespace HotPlay.BoosterMath.Core.Character
{
    public class OneshotSpineAnimator : SpineAnimator
    {
        [InjectOptional]
        private float delayTimeMultiplier = 1f;
        
        public override async UniTask Play(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                await UniTask.Yield();
                return;
            }
            
            SkeletonAnim.state.SetAnimation(0, AnimationReference, false);
            await UniTask.Delay((AnimationReference.Animation.Duration * delayTimeMultiplier).GetDurationMS(), DelayType.DeltaTime, PlayerLoopTiming.Update, cancellationToken);
            
            if (cancellationToken.IsCancellationRequested)
                SkeletonAnim.state.SetEmptyAnimation(0, 0);
        }
    }
}