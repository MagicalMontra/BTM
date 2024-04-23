using System;
using Zenject;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using HotPlay.PecanUI;
using UnityEngine;
using UnityEngine.Assertions;

namespace HotPlay.BoosterMath.Core
{
    public class GameStateProcessor : IInitializable, ITickable, IDisposable
    {
        public Action<GameStateEnum> OnGameStateChanged;
        
        public GameStateEnum LastState { get; private set; }

        [Inject]
        private PecanServices pecanServices;
        
        [Inject]
        private PauseController pauseController;

        [Inject]
        private readonly List<IState> states = new List<IState>();
        
        private bool isExecuting;
        private IState current;
        private Queue<IState> queue = new Queue<IState>();

        public void Initialize()
        {
            pecanServices.Events.GameplayEventsHandler.Restart += OnRestart;
            pecanServices.Events.GameplayEventsHandler.BackToMainMenu += OnMainMenu;
        }

        public void Dispose()
        {
            pecanServices.Events.GameplayEventsHandler.Restart -= OnRestart;
            pecanServices.Events.GameplayEventsHandler.BackToMainMenu -= OnMainMenu;
        }

        private void OnRestart()
        {
            queue.Clear();
            Enqueue(GameStateEnum.Reinitialization);
        }
        
        private void OnMainMenu()
        {
            queue.Clear();
            Enqueue(GameStateEnum.MainMenu);

        }
        
        public void OnStateChanged(IChangeGameStateSignal signal)
        {
            if (signal?.GameState == null)
                return;

            Enqueue(signal.GameState);
        }

        public void Tick()
        {
            if (!isExecuting && current != null)
                HandleCurrentStateActions().Forget();
            
            if (queue.Count <= 0)
                return;

            current ??= queue.Dequeue();
        }

        private void Enqueue(GameStateEnum gameState)
        {
            var selectedState = states.Find(state => state.Name == gameState);
            Assert.IsNotNull(selectedState);
            
#if UNITY_EDITOR || DEBUG
            Debug.Log($"Queued {selectedState.Name} game state");
#endif
            
            queue.Enqueue(selectedState);
        }
        
        private async UniTaskVoid HandleCurrentStateActions()
        {
            LastState = current.Name;
            isExecuting = true;
            OnGameStateChanged?.Invoke(current.Name);
#if UNITY_EDITOR || DEBUG
            Debug.Log($"Entering {current.Name} game state");
#endif
            await UniTask.WaitUntil(() => !pauseController.IsPause);
            await current.Enter();

#if UNITY_EDITOR || DEBUG
            Debug.Log($"Executing {current.Name} game state");
#endif
            await UniTask.WaitUntil(() => !pauseController.IsPause);
            await current.Execute();

#if UNITY_EDITOR || DEBUG
            Debug.Log($"Exiting {current.Name} game state");
#endif
            await UniTask.WaitUntil(() => !pauseController.IsPause);
            await current.Exit();
            
            isExecuting = false;
            current = null;
        }
    }
}
