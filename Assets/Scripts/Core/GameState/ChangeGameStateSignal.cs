namespace HotPlay.BoosterMath.Core
{
    public class ChangeGameStateSignal : IChangeGameStateSignal
    {
        public GameStateEnum GameState { get; }
        
        public ChangeGameStateSignal(GameStateEnum gameState)
        {
            GameState = gameState;
        }
    }
}