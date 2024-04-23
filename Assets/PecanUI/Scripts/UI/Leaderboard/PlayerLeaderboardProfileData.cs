using System;
using UnityEngine;

namespace HotPlay.PecanUI.Leaderboard
{
    [CreateAssetMenu(fileName = "PlayerLeaderboardProfileData", menuName = "DataManagement/PlayerLeaderboardProfileData")]
    [Serializable]
    public class PlayerLeaderboardProfileData : ScriptableObject, IComparable
    {
        [SerializeField]
        private string playerName;
        public string PlayerName => playerName;

        [SerializeField]
        private Sprite avatarSprite;
        public Sprite AvatarSprite => avatarSprite;

        [SerializeField]
        private int score;
        public int Score => score;

        private bool isHumanPlayer;

        public void Setup(string playerName, Sprite avatarSprite, int score, bool isHumanPlayer)
        {
            this.playerName = playerName;
            this.avatarSprite = avatarSprite;
            this.score = score;
            this.isHumanPlayer = isHumanPlayer;
        }

        public void Setup(PlayerLeaderboardProfileData data)
        {
            this.playerName = data.playerName;
            this.avatarSprite = data.avatarSprite;
            this.score = data.score;
            this.isHumanPlayer = data.isHumanPlayer;
        }
        
        public int CompareTo(object obj)
        {
            if (obj == null) return 1;

            PlayerLeaderboardProfileData otherPlayerLeaderboardProfileData = obj as PlayerLeaderboardProfileData;
            if (otherPlayerLeaderboardProfileData != null)
            {
                if(this.Score == otherPlayerLeaderboardProfileData.Score)
                {
                    return this.isHumanPlayer.CompareTo(otherPlayerLeaderboardProfileData.isHumanPlayer);
                }
                else
                {
                    return this.Score.CompareTo(otherPlayerLeaderboardProfileData.Score);
                }
            }
            else
            {
                throw new ArgumentException("Object is not a PlayerLeaderboardProfileData");
            }
        }
    }
}