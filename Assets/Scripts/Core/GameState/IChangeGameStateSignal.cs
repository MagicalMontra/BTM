using JetBrains.Annotations;

namespace HotPlay.BoosterMath.Core
{
    public interface IChangeGameStateSignal
    {
        GameStateEnum GameState { get; }
    }
}