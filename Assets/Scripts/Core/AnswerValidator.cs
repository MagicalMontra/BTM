
using System;
using HotPlay.QuickMath.Calculation;
using JetBrains.Annotations;
using UnityEngine.Assertions;
using Zenject;
using Random = UnityEngine.Random;

namespace HotPlay.BoosterMath.Core
{
    public class AnswerValidator
    {
        [CanBeNull] public event Action onAnswered;
        [CanBeNull] public event Action onValidAnswered;
        [CanBeNull] public event Action onInvalidAnswered;

        private QuestionData data;
        private int selectedAnswerIndex;

        public int? GetSelectedAnswer()
        {
            if (data == null)
                return null;
            
            return selectedAnswerIndex;
        }

        public string GetAnswerText()
        {
            return (selectedAnswerIndex < data.Pairs.Length ? data.Pairs[selectedAnswerIndex].Number : data.Result).ToString();
        }

        public bool CheckAnswer(int answer)
        {
            int selectedAnswer = selectedAnswerIndex < data.Pairs.Length ? data.Pairs[selectedAnswerIndex].Number : data.Result;
            return answer == selectedAnswer;
        }
        
        public void Set(QuestionData data)
        {
            this.data = data;
            selectedAnswerIndex = Random.Range(0, data.Pairs.Length + 1);
        }
        
        public void Answer(int? answer)
        {
            Assert.IsNotNull(data);
            
            if (!answer.HasValue)
                return;
            
            int selectedAnswer = selectedAnswerIndex < data.Pairs.Length ? data.Pairs[selectedAnswerIndex].Number : data.Result;
            
            onAnswered?.Invoke();

            if (answer.Value != selectedAnswer)
            {
                onInvalidAnswered?.Invoke();
                return;
            }
            
            onValidAnswered?.Invoke();
        }
    }
}