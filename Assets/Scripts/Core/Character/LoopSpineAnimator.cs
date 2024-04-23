using System.Threading;
using Cysharp.Threading.Tasks;

namespace HotPlay.BoosterMath.Core.Character
{
    public class LoopSpineAnimator : SpineAnimator
    {
        public override async UniTask Play(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                SkeletonAnim.state.SetEmptyAnimation(0, 0);
                await UniTask.Yield();
                return;
            }
            
            SkeletonAnim.state.SetAnimation(0, AnimationReference, true);
            await UniTask.Yield();
        }
    }
}