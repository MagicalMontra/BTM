using HotPlay.PecanUI;
using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Doozy.Runtime.UIManager;
using HotPlay.PecanUI.Gameplay;
using HotPlay.QuickMath.Calculation;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;
using Random = UnityEngine.Random;

namespace HotPlay.BoosterMath.Core.UI
{
    public class QuestionController : IInitializable, IDisposable
    {
        public QuestionPanel Panel => panel;
        public event Func<int?> onUIConstructed;

        [InjectOptional]
        private float panelFadeDuration = 0.4f;

        [Inject]
        private ITimer questionTimer;
        
        [Inject]
        private QuestionPanel panelPrefab;

        [Inject]
        private PecanServices pecanServices;
        
        [Inject]
        private QuestionPanel.Factory panelFactory;

        [Inject]
        private AnswerValidator validator;
        
        [Inject(Id = "NumberPool")]
        private IGameUIComponent.Pool numberPool;
        
        [Inject(Id = "OperatorPool")]
        private IGameUIComponent.Pool operatorPool;

        private int answerIndex;
        private QuestionPanel panel;
        private Vector3 originalSlotSize;
        private QuestionData currentData;
        private IGameUIComponent resultComponent;
        private List<UniTask> hideTasks = new List<UniTask>();
        private List<IGameUIComponent> numberComponents = new List<IGameUIComponent>();
        private List<IGameUIComponent> operatorComponents = new List<IGameUIComponent>();
        
        public void Initialize()
        {
            questionTimer.OnTicked += UpdateQuestionGauge;
            validator.onAnswered += OnAnswer;
            onUIConstructed += validator.GetSelectedAnswer;
            panel ??= panelFactory.Create(panelPrefab, pecanServices.GetCustomGamePlayPanel<GameplayUI>().transform);
            originalSlotSize = panel.QuestionSlot.transform.localScale;
        }
        
        public void Dispose()
        {
            validator.onAnswered -= OnAnswer;
            questionTimer.OnTicked -= UpdateQuestionGauge;
            onUIConstructed -= validator.GetSelectedAnswer;
        }

        private void OnAnswer()
        {
            OnAnswerAsync().Forget();
        }

        private async UniTaskVoid OnAnswerAsync()
        {
            if (answerIndex < numberComponents.Count)
                await numberComponents[answerIndex].Show(currentData.Pairs[answerIndex].Number.ToString());
            else
            {
                if (resultComponent != null)
                    await resultComponent.Show(currentData.Result.ToString());
            }
        }
        
        public async UniTask Show(QuestionData data)
        {
            currentData = data;
            panel ??= panelFactory.Create(panelPrefab, pecanServices.GetCustomGamePlayPanel<GameplayUI>().transform);
            
            if (!panel.gameObject.activeInHierarchy)
                panel.gameObject.SetActive(true);
            
            panel.QuestionSlot.gameObject.SetActive(true);
            panel.QuestionSlot.transform.localScale = Vector3.zero;
            panel.QuestionSlot.transform.DOScale(originalSlotSize, panelFadeDuration).SetEase(Ease.OutBack);

            var selectedAnswer = onUIConstructed?.Invoke();
            Assert.IsTrue(selectedAnswer.HasValue);

            for (int i = 0; i < data.Pairs.Length; i++)
            {
                if (selectedAnswer == i)
                    answerIndex = i;
                
                var questionInstance = numberPool.Spawn(panel.QuestionSlot.transform);
                questionInstance.Show(selectedAnswer == i ? "?" : data.Pairs[i].Number.ToString()).Forget();
                numberComponents.Add(questionInstance);
                var operatorInstance = operatorPool.Spawn(panel.QuestionSlot.transform);
                operatorInstance.Show(Convert.ToChar(data.Pairs[i].Symbol).ToString()).Forget();
                operatorComponents.Add(operatorInstance);
            }

            if (selectedAnswer == data.Pairs.Length)
                answerIndex = data.Pairs.Length;
            
            resultComponent = numberPool.Spawn(panel.QuestionSlot.transform);
            resultComponent.Show(selectedAnswer == data.Pairs.Length ? "?" : data.Result.ToString()).Forget();
            panel.QuestionTimeGauge.Show();
            await UniTask.Yield();
        }

        public async UniTask HideAsync()
        {
            hideTasks.Clear();

            foreach (var t in numberComponents.Where(t => t.gameObject.activeInHierarchy))
            {
                hideTasks.Add(t.Hide());
            }

            foreach (var t in operatorComponents.Where(t => t.gameObject.activeInHierarchy))
            {
                hideTasks.Add(t.Hide());
            }
            
            if (resultComponent != null)
                hideTasks.Add(resultComponent.Hide());
            
            await UniTask.WhenAll(hideTasks);
            await panel.QuestionSlot.transform.DOScale(Vector3.zero, panelFadeDuration).SetEase(Ease.OutBack);
            panel.QuestionSlot.gameObject.SetActive(false);
            DespawnComponents();
        }
        
        public void Hide()
        {
            panel.QuestionSlot.transform.DOScale(Vector3.zero, panelFadeDuration).OnComplete(() =>
            {
                DespawnComponents();
                panel.QuestionSlot.gameObject.SetActive(false);
            }).SetEase(Ease.OutBack);
        }

        private void DespawnComponents()
        {
            foreach (var t in numberComponents)
            {
                numberPool.Despawn(t);
            }

            foreach (var t in operatorComponents)
            {
                operatorPool.Despawn(t);
            }

            numberComponents.Clear();
            operatorComponents.Clear();

            if (resultComponent != null)
            {
                numberPool.Despawn(resultComponent);
                resultComponent = null;
            }
        }
        
        private void UpdateQuestionGauge()
        {
            panel.QuestionTimeGauge.UpdateGauge(questionTimer.Counter / questionTimer.Duration);
        }
    }
}