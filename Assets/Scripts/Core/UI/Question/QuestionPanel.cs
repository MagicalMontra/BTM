using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace HotPlay.BoosterMath.Core.UI
{
    public class QuestionPanel : MonoBehaviour
    {
        public CanvasGroup QuestionSlot => questionSlot;

        public QuestionTimeGauge QuestionTimeGauge => questionTimeGauge;
        
        [SerializeField]
        private CanvasGroup questionSlot;
        
        [SerializeField]
        private QuestionTimeGauge questionTimeGauge;

        public class Factory : PlaceholderFactory<QuestionPanel, Transform, QuestionPanel>
        {
            
        }
    }

    public class UIFactory<T> : IFactory<T, Transform, T> where T : Component
    {
        private DiContainer container;

        public UIFactory(DiContainer container)
        {
            this.container = container;
        }

        public T Create(T prefab, Transform transform)
        {
            return container.InstantiatePrefabForComponent<T>(prefab, transform);
        }
    }
}