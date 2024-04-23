using Cysharp.Threading.Tasks;
using HotPlay.PecanUI;
using UnityEngine;
using Zenject;

namespace HotPlay.BoosterMath.Core
{
    public class ScoreBoostActiveTimer : TickDownTimer
    {
        [Inject(Id = "Duration")]
        private readonly float duration;
        
        [Inject(Id = "Multiplier")]
        private readonly float multiplier = 0.1f;

        [Inject]
        private PecanServices services;

        [Inject]
        private readonly GameSessionController gameSessionController;
        
        public override async UniTask Start()
        {
            Counter = Duration = duration;
            gameSessionController.ScoreMultiplier += MultiplyScore;
            services.Events.GameplayEventsHandler.Restart += TryStop;
            services.Events.GameplayEventsHandler.BackToMainMenu += TryStop;
            await base.Start();
        }

        public override async UniTask Stop()
        {
            gameSessionController.ScoreMultiplier -= MultiplyScore;
            services.Events.GameplayEventsHandler.Restart -= TryStop;
            services.Events.GameplayEventsHandler.BackToMainMenu -= TryStop;
            await base.Stop();
        }

        public override async UniTask Reset()
        {
            Counter = Duration;
            await base.Reset();
        }

        protected override void OnTimeOut()
        {
            Stop().Forget();
        }
        
        private float MultiplyScore()
        {
            if (Counter <= 0)
                return 1;
            
            return 1 + multiplier;
        }
        
        private void TryStop()
        {
            Counter = 0;
            Stop().Forget();
        }
    }
}