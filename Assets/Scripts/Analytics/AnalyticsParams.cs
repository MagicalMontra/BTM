namespace HotPlay.QuickMath.Analytics
{
    public static class AnalyticsParams
    {
        public const string ResourceFlowType = "resourceFlowType";
        public const string CurrencyType = "currencyType";
        public const string Amount = "Amount";
        public const string ItemType = "itemType";
        public const string ItemID = "itemId";
        public const string EventName = "eventName";
        public const string Value = "value";
        public const string Score = "score";
        public const string ProgressionName01 = "progression01";
        public const string ProgressionStatus = "progressionStatus";

        #region  Value
        public const string ResourceGainType = "gain";
        public const string ResourceSpendType = "spend";
        public const string ProgressionStart = "start";
        public const string ProgressionComplete = "complete";
        public const string ProgressionFail = "fail";
        public const string GameOver = "gameOver";

        #region CurrencyType
        public const string Coin = "coin";
        #endregion

        #region  Item Type
        public const string Daily = "daily";
        public const string Character = "character";
        public const string Theme = "theme";
        #endregion
        #endregion
    }
}
