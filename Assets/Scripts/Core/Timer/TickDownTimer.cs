using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace HotPlay.BoosterMath.Core
{
    public abstract class TickDownTimer : ITimer, IInitializable, IDisposable
    {
        public Action OnStart { get; set; }

        public Action OnReset { get; set; }
        
        public Action OnTimedOut { get; set; }

        public Action OnStop { get; set; }
        
        public Action OnTicked { get; set; }
        
        public Func<float> TimeMultiplier { get; set; }
        
        public float Counter { get; protected set; }
        
        public float Duration { get; protected set; }

        private bool isPaused;
        
        private bool isTicking;
        
        [Inject]
        private PauseController pauseController;
        
        [Inject]
        private GameStateProcessor gameStateProcessor;

        public void Initialize()
        {
            pauseController.OnPaused += Pause;
            gameStateProcessor.OnGameStateChanged += OnStateChanged;
            OnInitialized();
        }
        
        public void Dispose()
        {
            pauseController.OnPaused -= Pause;
            gameStateProcessor.OnGameStateChanged -= OnStateChanged;
            OnDisposed();
        }

        public void Tick()
        {
            if (!isTicking)
                return;
            
            if (isPaused)
                return;

            if (Counter <= 0)
            {
                OnTimeOut();
                OnTimedOut?.Invoke();
            }

            OnTick();
            Counter -= Time.deltaTime * TimeMultiplier?.Invoke() ?? Time.deltaTime;
        }

        public virtual async UniTask Start()
        {
            isTicking = true;
            OnStart?.Invoke();
            await UniTask.Yield();
        }
        
        public virtual async UniTask Start(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                await UniTask.Yield();
                return;
            }
            
            isTicking = true;
            OnStart?.Invoke();
            await UniTask.Yield();
        }

        public virtual async UniTask Reset()
        {
            OnReset?.Invoke();
            await UniTask.Yield();
        }

        public virtual async UniTask Reset(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                await UniTask.Yield();
                return;
            }
            
            OnReset?.Invoke();
            await UniTask.Yield();
        }

        public virtual async UniTask AddTime()
        {
            await UniTask.Yield();
        }
        
        public virtual async UniTask AddTime(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                await UniTask.Yield();
                return;
            }
            
            await UniTask.Yield();
        }

        public virtual void Pause(bool isPaused)
        {
            this.isPaused = isPaused;
        }

        public virtual async UniTask Stop()
        {
            isTicking = false;
            OnStop?.Invoke();
            await UniTask.Yield();
        }
        
        public virtual async UniTask Stop(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                await UniTask.Yield();
                return;
            }
            
            isTicking = false;
            OnStop?.Invoke();
            await UniTask.Yield();
        }

        protected virtual void OnTick()
        {
            OnTicked?.Invoke();
        }

        protected virtual void OnInitialized()
        {
            
        }
        
        protected virtual void OnDisposed()
        {
            
        }

        protected abstract void OnTimeOut();

        private void OnStateChanged(GameStateEnum gameState)
        {
            if (gameState is GameStateEnum.Termination or GameStateEnum.Reinitialization)
            {
                Stop().Forget();
                return;
            }
            
            Pause(gameState != GameStateEnum.Process);
        }
    }
}