using System.Threading;
using Cysharp.Threading.Tasks;
using HotPlay.BoosterMath.Core.Character;
using JetBrains.Annotations;

namespace HotPlay.BoosterMath.Core.Enemy
{
    public interface IEnemySpawner
    {
        [CanBeNull]
        ICharacter Current { get; }

        UniTask Spawn(CancellationToken cancellationToken);

        UniTask Despawn();
    }
}