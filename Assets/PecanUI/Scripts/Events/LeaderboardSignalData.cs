#nullable enable

namespace HotPlay.PecanUI.Events
{
    public class LeaderboardSignalData
    {
        public int PrevScore { get; private set; }
        public int CurrentScore { get; private set; }
        public LeaderboardDialogOpenType OpenType { get; private set; }

        public LeaderboardSignalData(int prevScore, int currentScore, LeaderboardDialogOpenType openType)
        {
            PrevScore = prevScore;
            CurrentScore = currentScore;
            OpenType = openType;
        }

        public LeaderboardSignalData(LeaderboardDialogOpenType openType) : this(0, 0, openType) { }
    }
}
