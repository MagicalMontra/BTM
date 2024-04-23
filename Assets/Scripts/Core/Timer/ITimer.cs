using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Zenject;

namespace HotPlay.BoosterMath.Core
{
    public interface ITimer : ITickable
    {
        Action OnStart { get; set; }
        
        Action OnTicked { get; set; }
        
        Action OnReset { get; set; }
        
        Action OnTimedOut { get; set; }
        
        Action OnStop { get; set; }

        Func<float> TimeMultiplier { get; set; }
        
        float Counter { get; }
        
        float Duration { get; }

        UniTask Start();
        UniTask Start(CancellationToken cancellationToken);
        
        UniTask Reset();

        UniTask Reset(CancellationToken cancellationToken);
        
        UniTask AddTime();

        UniTask AddTime(CancellationToken cancellationToken);

        void Pause(bool isPaused);

        UniTask Stop();
        UniTask Stop(CancellationToken cancellationToken);
    }
}
