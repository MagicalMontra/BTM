using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Zenject;

namespace HotPlay.BoosterMath.Core.UI
{
    public class QuestionComponent : MonoBehaviour, IGameUIComponent
    {
        [SerializeField]
        private TextMeshProUGUI label;

        [SerializeField]
        private Color hideColor;
        
        [SerializeField]
        private Color normalColor;
        
        [Inject]
        private AnswerValidator answerValidator;
        
        private const string hideLabel = "?";

        private void Awake()
        {
            gameObject.SetActive(false);
        }
        
        public async UniTask Show(string sText)
        {
            label.SetText(sText);
            label.color = hideLabel == sText ? hideColor : normalColor;
            label.alpha = 0f;
            gameObject.SetActive(true);
            await label.DOFade(1f, 0.1f);
        }

        public async UniTask Hide()
        {
            label.alpha = 1f;
            await label.DOFade(0f, 0.4f);
        }
    }
}
