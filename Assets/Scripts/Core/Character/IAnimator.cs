using System.Threading;
using Cysharp.Threading.Tasks;

namespace HotPlay.BoosterMath.Core.Character
{
    public interface IAnimator
    {
        UniTask Play(CancellationToken cancellationToken);
    }
}