using HotPlay.BoosterMath.Core.UI;
using HotPlay.PecanUI.Gameplay;
using TMPro;
using UnityEngine;

namespace HotPlay.BoosterMath.Core
{
    public class GameplayUI : BaseCustomGameplayPanel
    {
        public Transform DisableGroup => disableGroup;

        [SerializeField]
        private Transform disableGroup;

        [SerializeField]
        private TextMeshProUGUI scoreText;

        public void UpdateScore(int score)
        {
            scoreText.SetText(score.ToString());
        }
        
        public void UpdateScore(string score)
        {
            scoreText.SetText(score);
        }
    }
}