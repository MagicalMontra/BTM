using System;
using System.Collections.Generic;
using UnityEngine;

namespace HotPlay.PecanUI.Leaderboard
{
    [Serializable]
    public class LeaderboardDataProvider
    {
        public List<PlayerLeaderboardProfileData> LeaderboardData => leaderboardData;
        public string PlayerName => playerName;
        public Sprite PlayerSprite => playerAvatarSprite;
        public int PlayerScore => playerCurrentHighScoreTest;

        [SerializeField]
        private List<PlayerLeaderboardProfileData> leaderboardData;

        [SerializeField]
        private string playerName = "Your Name"; //TODO: Get actual current player name from data

        [SerializeField]
        private Sprite playerAvatarSprite = null; //TODO: Get actual player sprite from data

        [SerializeField]
        private int playerCurrentHighScoreTest = 0; //TODO: Get current high score from player data
    }
}