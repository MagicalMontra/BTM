using UnityEngine;
using Zenject;

namespace HotPlay.BoosterMath.Core.UI
{
    public class AnswerPanel : MonoBehaviour
    {
        public AnswerComponent DebugComponent => debugComponent;
        
        public AnswerComponent[] Components => components;
        
        [SerializeField]
        private AnswerComponent[] components;
        
        [SerializeField]
        private AnswerComponent debugComponent;

        public class Factory : PlaceholderFactory<AnswerPanel, Transform, AnswerPanel>
        {
            
        }
    }
}