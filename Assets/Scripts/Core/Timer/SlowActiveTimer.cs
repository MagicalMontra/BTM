using Cysharp.Threading.Tasks;
using HotPlay.PecanUI;
using UnityEngine;
using Zenject;

namespace HotPlay.BoosterMath.Core
{
    public class SlowActiveTimer : TickDownTimer
    {
        [Inject(Id = "Duration")]
        private readonly float duration;
        
        [Inject(Id = "Multiplier")]
        private readonly float multiplier = 0.3f;
        
        [Inject]
        private PecanServices services;

        [Inject]
        private readonly QuestionTimer questionTimer;
        
        public override async UniTask Start()
        {
            Counter = Duration = duration;
            questionTimer.TimeMultiplier += Slow;
            services.Events.GameplayEventsHandler.Restart += TryStop;
            services.Events.GameplayEventsHandler.BackToMainMenu += TryStop;
            await base.Start();
        }

        public override async UniTask Stop()
        {
            questionTimer.TimeMultiplier -= Slow;
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

        private float Slow()
        {
            if (Counter <= 0)
                return 1;
            
            return 1f - multiplier;
        }
        
        private void TryStop()
        {
            Counter = 0;
            Stop().Forget();
        }
    }
}