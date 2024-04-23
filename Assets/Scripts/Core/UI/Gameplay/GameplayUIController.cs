using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using HotPlay.QuickMath.Calculation;
using Zenject;

namespace HotPlay.BoosterMath.Core.UI
{
    public class GameplayUIController : IInitializable, IDisposable
    {
        [Inject]
        private AnswerValidator validator;

        [Inject]
        private AnswerUIController answerUIController;
        
        [Inject]
        private QuestionController questionController;

        private List<UniTask> showTasks = new List<UniTask>();
        private List<UniTask> hideTasks = new List<UniTask>();

        public void Initialize()
        {
            validator.onAnswered += TryHide;
        }
        
        public void Dispose()
        {
            validator.onAnswered -= TryHide;
        }
        
        public async UniTask ShowAsync(QuestionData data)
        {
            validator.Set(data);
            showTasks.Clear();
            showTasks.Add( answerUIController.Show(data));
            showTasks.Add(questionController.Show(data));
            await UniTask.WhenAll(showTasks);
        }

        public async UniTask HideAsync()
        {
            await answerUIController.HideAsync();
            await questionController.HideAsync();
        }

        public void Hide()
        {
            answerUIController.Hide();
            questionController.Hide();
            questionController.Panel.QuestionTimeGauge.Hide();
        }

        private void TryHide()
        {
            HideAsync().Forget();            
        }
    }
}