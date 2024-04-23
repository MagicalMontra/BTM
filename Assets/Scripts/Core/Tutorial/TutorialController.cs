using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Doozy.Runtime.UIManager;
using Zenject;
using HotPlay.PecanUI;
using UnityEngine;
using HotPlay.QuickMath.Analytics;

namespace HotPlay.BoosterMath.Core
{
    public enum Booster
    {
        TimeSlowdown = 0,
        TimeRecover = 1,
        HigherScore = 2,
    }
    public class TutorialController
    {
        public event Action TutorialPause;

        public const string FirstTimeTutorialGroup = "FIRST_TIME_GROUP";

        private const string timeSlowdownBoosterDownTutorialGroup = "TIME_SLOWDOWN_GROUP";
        private const string timeRecoveryBoosterTutorialGroup = "TIME_RECOVERY_GROUP";
        private const string highScoreBoosterTutorialGroup = "HIGH_SCORE_GROUP";

        private const string firstTimeTutorialKey = "FIRST_TIME_TUTORIAL_PASS";
        private const string timeSlowdownBoosterTutorialKey = "TIME_SLOWDOWN_TUTORIAL_PASS";
        private const string timeRecoveryBoosterTutorialKey = "TIME_RECOVERY_TUTORIAL_PASS";
        private const string highScoreBoosterTutorialKey = "HIGH_SCORE_TUTORIAL_PASS";

        [Inject]
        private PecanServices pecanServices;
        
        [Inject]
        private PauseController pauseController;
        

        private List<TutorialData> tutorialDatas = new List<TutorialData>();

        private List<ItemDropTypeEnum> boosters = new List<ItemDropTypeEnum>();

        public void FirstTutorialPass()
        {
            if (IsFirstTimeTutorialPass())
            {
                return;
            }

            SendTutorialSignal();
        }

        private void SendTutorialSignal()
        {
            var tutorials = pecanServices.GetTutorialsByGroup(FirstTimeTutorialGroup);
            pecanServices.Signals.SendTutorialSignal(tutorials);
            pauseController.PauseTutorial();
            SetFirstTimeTutorialPass();
            AnalyticsHelper.LogTutorialStartEvent("tutorial01");
        }

        private bool IsFirstTimeTutorialPass()
        {
            return ES3.Load<bool>(firstTimeTutorialKey, false);
        }

        private void SetFirstTimeTutorialPass()
        {
            ES3.Save(firstTimeTutorialKey, true);
        }

        private bool IsBoosterTutorialPass(ItemDropTypeEnum booster)
        {
            return booster switch
            {
                ItemDropTypeEnum.Slow => ES3.Load<bool>(timeSlowdownBoosterTutorialKey, false),
                ItemDropTypeEnum.Rewind => ES3.Load<bool>(timeRecoveryBoosterTutorialKey, false),
                ItemDropTypeEnum.ScoreBoost => ES3.Load<bool>(highScoreBoosterTutorialKey, false),
                _ => throw new InvalidOperationException($"{booster} is not supported")
            };
        }

        private void SetBoosterTutorialPass(ItemDropTypeEnum booster)
        {
            var key = booster switch
            {
                ItemDropTypeEnum.Slow => timeSlowdownBoosterTutorialKey,
                ItemDropTypeEnum.Rewind => timeRecoveryBoosterTutorialKey,
                ItemDropTypeEnum.ScoreBoost => highScoreBoosterTutorialKey,
                _ => throw new InvalidOperationException($"{booster} is not supported")
            };
            ES3.Save(key, true);

        }

        private string GetBoosterTutorialGroup(ItemDropTypeEnum booster)
        {
            return booster switch
            {
                ItemDropTypeEnum.Slow => timeSlowdownBoosterDownTutorialGroup,
                ItemDropTypeEnum.Rewind => timeRecoveryBoosterTutorialGroup,
                ItemDropTypeEnum.ScoreBoost => highScoreBoosterTutorialGroup,
                _ => string.Empty
            };
        }

        private TutorialData[] GetMultipleBoosterTutorialGroup()
        {
            tutorialDatas.Clear();
            
            foreach (var booster in boosters)
            {
                if (IsBoosterTutorialPass(booster))
                    continue;
            
                foreach (var data in pecanServices.GetTutorialsByGroup(GetBoosterTutorialGroup(booster)))
                {
                    tutorialDatas.Add(data);
                }
                
                SetBoosterTutorialPass(booster);
            }
            
            boosters.Clear();

            return tutorialDatas.ToArray();
        }

        public async UniTaskVoid EngageBoosterTutorialQueue()
        {
            if (boosters.Count <= 0)
            {
                await UniTask.Yield();
                return;
            }
            
            await UniTask.WaitWhile(() => pauseController.IsPause);
            var tutorials = GetMultipleBoosterTutorialGroup();
            pauseController.PauseTutorial();
            pecanServices.Signals.SendTutorialSignal(tutorials);
            await UniTask.Yield();
        }

        public void BoosterTutorialPass(ItemDropTypeEnum booster)
        {
            if (IsBoosterTutorialPass(booster))
                return;

            if (!boosters.Contains(booster))
                boosters.Add(booster);
        }

#if CHEAT_ENABLED
        public void Reset()
        {
            ES3.DeleteKey(firstTimeTutorialKey);
            ES3.DeleteKey(timeSlowdownBoosterTutorialKey);
            ES3.DeleteKey(timeRecoveryBoosterTutorialKey);
            ES3.DeleteKey(highScoreBoosterTutorialKey);
        }
#endif
    }
}
