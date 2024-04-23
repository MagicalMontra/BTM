#nullable enable

namespace HotPlay.PecanUI.Events
{
    public enum LeaderboardDialogOpenType
    {
        None = 0,
        Manual = 1,
        Auto = 2,
    }

    public static class LeaderboardDialogOpenTypeExtension
    {
        private const string AutoKey = "auto";
        private const string ManualKey = "manual";

        public static LeaderboardDialogOpenType Parse(string key)
        {
            return key switch
            {
                AutoKey => LeaderboardDialogOpenType.Auto,
                ManualKey => LeaderboardDialogOpenType.Manual,
                _ => LeaderboardDialogOpenType.None
            };
        }
    }
}
