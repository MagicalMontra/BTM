using HotPlay.PecanUI;
using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using HotPlay.QuickMath.Calculation;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;
using Random = UnityEngine.Random;

namespace HotPlay.BoosterMath.Core.UI
{
    public class AnswerUIController : IInitializable, IDisposable
    {
        public event Func<int?> onUIConstructed;

        [Inject]
        private AnswerPanel panelPrefab;

        [Inject]
        private AnswerValidator validator;
        
        [Inject]
        private PecanServices pecanServices;
        
        [Inject]
        private AnswerPanel.Factory factory;

        [Inject]
        private RewirdInputController inputController;

        [Inject]
        private GameModeController gameModeController;

        private AnswerPanel panel;
        private List<UniTask> showTasks = new List<UniTask>();
        private List<UniTask> hideTasks = new List<UniTask>();
        private HashSet<int> existedNumber = new HashSet<int>();

        public void Initialize()
        {
            onUIConstructed += validator.GetSelectedAnswer;
            panel ??= factory.Create(panelPrefab, pecanServices.GetCustomGamePlayPanel<GameplayUI>().transform);
            panel.gameObject.SetActive(false);

#if CHEAT_ENABLED
            panel.DebugComponent.gameObject.SetActive(true);
#else
            panel.DebugComponent.gameObject.SetActive(false);
#endif
            
        }
        
        public void Dispose()
        {
            onUIConstructed -= validator.GetSelectedAnswer;
        }

        public async UniTask Show(QuestionData data)
        {
            panel ??= factory.Create(panelPrefab, pecanServices.GetCustomGamePlayPanel<GameplayUI>().transform);
            
            if (!panel.gameObject.activeInHierarchy)
                panel.gameObject.SetActive(true);

            var selectedAnswer = onUIConstructed?.Invoke();
            Assert.IsTrue(selectedAnswer.HasValue);
            int answer = selectedAnswer < data.Pairs.Length ? data.Pairs[selectedAnswer.Value].Number : data.Result;
            
            existedNumber.Clear();
            showTasks.Clear();
            existedNumber.Add(answer);
            int correctValueIndex = Random.Range(0, panel.Components.Length);
            
            for (int i = 0; i < panel.Components.Length; i++)
            {
                var value = correctValueIndex == i ? answer : RandomFakeAnswer(answer);
                showTasks.Add( panel.Components[i].Show(value.ToString()));
            }

            BindAnswerInput(panel.Components);
            
#if CHEAT_ENABLED
            showTasks.Add(panel.DebugComponent.Show(answer.ToString()));
#endif
            await UniTask.WhenAll(showTasks);
        }

        public async UniTask HideAsync()
        {
            if (!panel.gameObject.activeInHierarchy)
                panel.gameObject.SetActive(true);
            
            hideTasks.Clear();
            UnbindAnswerInput(panel.Components);
            
            for (int i = 0; i < panel.Components.Length; i++)
            {
                if (panel.Components[i].gameObject.activeInHierarchy)
                    hideTasks.Add(panel.Components[i].Hide());
            }
            
#if CHEAT_ENABLED
            if (panel.DebugComponent.gameObject.activeInHierarchy)
                hideTasks.Add(panel.DebugComponent.Hide());
#endif
            await UniTask.WhenAll(hideTasks);
            await UniTask.Yield();
        }
        
        public void Hide()
        {
            if (!panel.gameObject.activeInHierarchy)
                panel.gameObject.SetActive(true);
            
            hideTasks.Clear();
            UnbindAnswerInput(panel.Components);
            
            for (int i = 0; i < panel.Components.Length; i++)
            {
                panel.Components[i].Hide().Forget();
            }
            
#if UNITY_EDITOR || CHEAT_ENABLED
            if (panel.DebugComponent.gameObject.activeInHierarchy)
                panel.DebugComponent.Hide().Forget();
#endif
        }

        private void BindAnswerInput(AnswerComponent[] components)
        {
            inputController.UpKeyDown += components[0].Answer;
            inputController.RightKeyDown += components[1].Answer;
            inputController.DownKeyDown += components[2].Answer;
            inputController.LeftKeyDown += components[3].Answer;
        }
        
        private void UnbindAnswerInput(AnswerComponent[] components)
        {
            inputController.UpKeyDown -= components[0].Answer;
            inputController.RightKeyDown -= components[1].Answer;
            inputController.DownKeyDown -= components[2].Answer;
            inputController.LeftKeyDown -= components[3].Answer;
        }

        private int RandomFakeAnswer(int answer)
        {
            var value = Random.Range(1, gameModeController.CurrentGameMode.Settings.MaxFakeAnswerRange);

            while (existedNumber.Contains(value))
            {
                value = Random.Range(
                    answer - value,
                    answer + value
                );
            }

            existedNumber.Add(value);

            return value;
        }
    }
}