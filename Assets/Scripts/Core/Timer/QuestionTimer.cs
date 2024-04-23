using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using HotPlay.BoosterMath.Core.UI;
using UnityEngine;
using Zenject;

namespace HotPlay.BoosterMath.Core
{
    public class QuestionTimer : TickDownTimer
    {
        [InjectOptional]
        private int approachingValue = 10;
        
        [Inject]
        private GameData gameData;

        [Inject]
        private QuestionController questionController;

        [Inject]
        private GameSessionController gameSessionController;
        
        public override async UniTask Start()
        {
            var totalTime = gameData.AnswerTimeCurve.Evaluate(gameSessionController.CurrentStage / ((float)gameSessionController.CurrentStage + approachingValue));
            Duration = totalTime;

            if (Counter <= 0 || Counter > Duration)
            {
                Counter = Duration;
                questionController.Panel.QuestionTimeGauge.UpdateGauge(Counter / Duration);
            }

            await base.Start();
        }
        
        public override async UniTask Start(CancellationToken cancellationToken)
        {
            var totalTime = gameData.AnswerTimeCurve.Evaluate(gameSessionController.CurrentStage / ((float)gameSessionController.CurrentStage + approachingValue));
            Duration = totalTime;
            
            if (Counter <= 0 || Counter > Duration)
            {
                Counter = Duration;
                questionController.Panel.QuestionTimeGauge.UpdateGauge(Counter / Duration);
            }

            await base.Start(cancellationToken);
        }

        public override async UniTask Reset()
        {
            var totalTime = gameData.AnswerTimeCurve.Evaluate(gameSessionController.CurrentStage / ((float)gameSessionController.CurrentStage + approachingValue));
            Duration = totalTime;
            Counter = Duration;
            questionController.Panel.QuestionTimeGauge.UpdateGauge(Counter / Duration);
            await base.Reset();
        }
        
        public override async UniTask Reset(CancellationToken cancellationToken)
        {
            var totalTime = gameData.AnswerTimeCurve.Evaluate(gameSessionController.CurrentStage / ((float)gameSessionController.CurrentStage + approachingValue));
            Duration = totalTime;
            Counter = Duration;
            await questionController.Panel.QuestionTimeGauge.UpdateGaugeAsync(Counter / Duration, cancellationToken);
            
            if (cancellationToken.IsCancellationRequested)
            {
                questionController.Panel.QuestionTimeGauge.UpdateGauge(Counter / Duration);
                await UniTask.Yield();
                return;
            }
            
            await base.Reset(cancellationToken);
        }

        public override async UniTask AddTime()
        {
            var value = gameSessionController.CurrentStage / ((float)gameSessionController.CurrentStage + approachingValue);
            var totalTime = gameData.AnswerTimeCurve.Evaluate(value);
            var additionalTime = totalTime; 
            additionalTime *= gameData.AnswerTimeRecoverCurve.Evaluate(value);
            Counter = Counter <= 0 ? totalTime : Mathf.Clamp(Counter + additionalTime, 0, totalTime);
            questionController.Panel.QuestionTimeGauge.UpdateGauge(Counter / Duration);
            await UniTask.Yield();
        }
        
        public override async UniTask AddTime(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                await UniTask.Yield();
                return;
            }
            
            var value = gameSessionController.CurrentStage / ((float)gameSessionController.CurrentStage + approachingValue);
            var totalTime = gameData.AnswerTimeCurve.Evaluate(value);
            var additionalTime = totalTime; 
            additionalTime *= gameData.AnswerTimeRecoverCurve.Evaluate(value);
            Counter = Counter <= 0 ? totalTime : Mathf.Clamp(Counter + additionalTime, 0, totalTime);
            await questionController.Panel.QuestionTimeGauge.UpdateGaugeAsync(Counter / Duration, cancellationToken);

            if (cancellationToken.IsCancellationRequested)
            {
                questionController.Panel.QuestionTimeGauge.UpdateGauge(Counter / Duration);
                await UniTask.Yield();
                return;
            }
            
            await base.AddTime(cancellationToken);
        }

        protected override void OnTimeOut()
        {
            Stop().Forget();
        }
    }
}