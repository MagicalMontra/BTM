using HotPlay.Sound;
using UnityEngine;

namespace HotPlay.BoosterMath.Core
{
    [CreateAssetMenu(menuName = "HotPlay/BoosterMath/Data/Create SoundData", fileName = "SoundData", order = 0)]
    public class SoundData : ScriptableObject
    {
        public SoundClip BoosterCollect => boosterCollect;

        public SoundClip CoinCollect => coinCollect;

        public SoundClip TimerCountDown => timerCountDown;

        public SoundClip HeartCollect => heartCollect;

        public SoundClip MonsterAttack => monsterAttack;

        public SoundClip MonsterDefeated => monsterDefeated;

        public SoundClip MonsterSpawn => monsterSpawn;

        public SoundClip PlayerAttack => playerAttack;

        public SoundClip PlayerDefeated => playerDefeated;

        public SoundClip PlayerGetHit => playerGetHit;

        public SoundClip PlayerStep => playerStep;

        public SoundClip TimeOut => timeOut;

        public SoundClip MainMenuBGM => mainMenuBGM;
        public SoundClip GameplayBGM => gameplayBGM;
        public SoundClip BonusAnswer => bonusAnswer;
        public SoundClip CorrectAnswer => correctAnswer;
        public SoundClip WrongAnswer => wrongAnswer;
        public SoundClip GameplayResult => gameplayResult;
        public SoundClip CountdownTimer => countdownTimer;
        public SoundClip WindowPopup => windowPopup;
        public SoundClip WindowHide => windowHide;
        public SoundClip PlayButton => playButton;
        public SoundClip DailyReward => dailyReward;
        public SoundClip ButtonClick => buttonClick;
        public SoundClip UnlockItem => unlockItem;
        public SoundClip ButtonPlus => buttonPlus;
        public SoundClip ButtonMinus => buttonMinus;

        [SerializeField]
        private SoundClip mainMenuBGM;

        [SerializeField]
        private SoundClip gameplayBGM;

        [SerializeField]
        private SoundClip bonusAnswer;
        
        [SerializeField]
        private SoundClip boosterCollect;
        
        [SerializeField]
        private SoundClip coinCollect;
        
        [SerializeField]
        private SoundClip timerCountDown;
        
        [SerializeField]
        private SoundClip heartCollect;
        
        [SerializeField]
        private SoundClip monsterAttack;
        
        [SerializeField]
        private SoundClip monsterDefeated;
        
        [SerializeField]
        private SoundClip monsterSpawn;
        
        [SerializeField]
        private SoundClip playerAttack;
        
        [SerializeField]
        private SoundClip playerDefeated;
        
        [SerializeField]
        private SoundClip playerGetHit;
        
        [SerializeField]
        private SoundClip playerStep;
        
        [SerializeField]
        private SoundClip timeOut;

        [SerializeField]
        private SoundClip correctAnswer;

        [SerializeField]
        private SoundClip wrongAnswer;

        [SerializeField]
        private SoundClip gameplayResult;

        [SerializeField]
        private SoundClip countdownTimer;

        [SerializeField]
        private SoundClip windowPopup;

        [SerializeField]
        private SoundClip windowHide;

        [SerializeField]
        private SoundClip playButton;

        [SerializeField]
        private SoundClip dailyReward;

        [SerializeField]
        private SoundClip buttonClick;

        [SerializeField]
        private SoundClip unlockItem;

        [SerializeField]
        private SoundClip buttonPlus;
        
        [SerializeField]
        private SoundClip buttonMinus;
    }
}