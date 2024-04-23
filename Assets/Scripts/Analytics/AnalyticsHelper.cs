using System;
using System.Collections.Generic;

namespace HotPlay.QuickMath.Analytics
{
    public static class AnalyticsHelper
    {
        private static DateTime startTimeStamp;
        private static int restartCount;
        private static int prevScore;
        private static bool isPlayAgain;
        private static bool isGameOver = true;
        private static string tutorialId;

        public static void StartGame()
        {
            startTimeStamp = DateTime.Now;
            isGameOver = false;
        }

        public static void PlayAgain()
        {
            isPlayAgain = true;
            restartCount = 0;
            startTimeStamp = DateTime.Now;
        }

        public static void Restart()
        {
            restartCount++;
        }

        public static void CleanUp()
        {
            startTimeStamp = DateTime.Now;
            isGameOver = false;
            isPlayAgain = false;
            restartCount = 0;
            prevScore = 0;
        }

        public static void GameOver(int score)
        {
            TimeSpan deltaTime = DateTime.Now - startTimeStamp;
            AnalyticsServices.Instance.LogEvent("gameOver:duration:seconds", (float)deltaTime.TotalSeconds);

            AnalyticsServices.Instance.LogEvent("gameOver:restart:count", restartCount);

            prevScore = score;

            if (isPlayAgain)
            {
                AnalyticsServices.Instance.LogEvent("gameOver:playAgain:score", prevScore);
            }
            else
            {
                AnalyticsServices.Instance.LogEvent("gameOver:playFromHome:score", prevScore);
            }

            Dictionary<string, object> analyticsData = new Dictionary<string, object>()
            {
                { AnalyticsParams.ProgressionStatus, AnalyticsParams.ProgressionComplete },
                { AnalyticsParams.ProgressionName01 , "gameOver" },
                { AnalyticsParams.Score, prevScore }
            };

            AnalyticsServices.Instance.LogProgressionEvent(analyticsData);
        }

        public static void LogTutorialStartEvent(string id)
        {
            tutorialId = id;
            AnalyticsServices.Instance.LogEvent($"tutorial:{tutorialId}:start", 0f);
        }

        public static void LogTutorialCompleteEvent()
        {
            AnalyticsServices.Instance.LogEvent($"tutorial:{tutorialId}:complete", 0f);
            tutorialId = string.Empty;
        }
    }
}
