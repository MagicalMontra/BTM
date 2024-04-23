using System.Threading;
using Cysharp.Threading.Tasks;

namespace HotPlay.BoosterMath.Core
{
    public interface IState
    {
        GameStateEnum Name { get; }
        
        UniTask Enter();
        
        UniTask Execute();
        
        UniTask Exit();
    }

    public abstract class StateBase : IState
    {
        public abstract GameStateEnum Name { get; }

        public abstract UniTask Enter();

        public abstract UniTask Execute();

        public abstract UniTask Exit();
    }
}