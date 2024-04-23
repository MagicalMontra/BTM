using Cysharp.Threading.Tasks;
using HotPlay.PecanUI;
using Zenject;

namespace HotPlay.BoosterMath.Core
{
    public class RewindActiveTimer : TickDownTimer
    {
        [Inject(Id = "Duration")]
        private readonly float duration;

        [Inject]
        private PecanServices services;

        public override async UniTask Start()
        {
            Counter = Duration = duration;
            services.Events.GameplayEventsHandler.Restart += TryStop;
            services.Events.GameplayEventsHandler.BackToMainMenu += TryStop;
            await base.Start();
        }

        public override async UniTask Stop()
        {
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

        private void TryStop()
        {
            Counter = 0;
            Stop().Forget();
        }
    }
}