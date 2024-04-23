namespace HotPlay.PecanUI.Leaderboard
{
    public class LeaderboardListRowData
    {
        public int RowIndex;
        public PlayerLeaderboardProfileData ProfileData { get => profileData;}

        private PlayerLeaderboardProfileData profileData;

        public LeaderboardListRowData(int rowIndex, PlayerLeaderboardProfileData profileData)
        {
            RowIndex = rowIndex;
            this.profileData = profileData;
        }
    }
}