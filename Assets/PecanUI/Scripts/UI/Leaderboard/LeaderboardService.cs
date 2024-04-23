using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Doozy.Runtime.UIManager.Containers;
using HotPlay.Utilities;
using UnityEngine;

namespace HotPlay.PecanUI.Leaderboard
{
    public static class LeaderboardService
    {
        public static List<PlayerLeaderboardProfileData> GenerateLeaderboardDataWithFakeProfiles(
            List<PlayerLeaderboardProfileData> fakeProfiles,
            PlayerLeaderboardProfileData myProfile
        )
        {
            List<PlayerLeaderboardProfileData> playerLeaderboardProfileList = new List<PlayerLeaderboardProfileData>(fakeProfiles);
            playerLeaderboardProfileList.Add(myProfile);
            playerLeaderboardProfileList.Sort();
            playerLeaderboardProfileList.Reverse();
            return playerLeaderboardProfileList;
        }

        public static int GetPlayerLeaderboardRank(string playerName, IEnumerable<PlayerLeaderboardProfileData> leaderboardProfileList)
        {
            //Players can have duplicated name, so I decide to loop find like this to get the first profile with the highest score
            int playerRank = 1;
            foreach (PlayerLeaderboardProfileData profile in leaderboardProfileList)
            {
                if (profile.PlayerName.Equals(playerName))
                {
                    break;
                }
                playerRank++;
            }
            return playerRank;
        }

        public static void ShowLeaderboard(string playerName, List<PlayerLeaderboardProfileData> leaderboardData)
        {
            UIView leaderboardView = UIView.GetViews("MenuUI", "Leaderboard").FirstOrDefault();

            if(leaderboardView != null)
            {
                LeaderboardDialog dialog = leaderboardView.gameObject.GetComponent<LeaderboardDialog>();
                dialog.Setup(leaderboardData, playerName, GetPlayerLeaderboardRank(playerName, leaderboardData));
            }
        }

        public static async UniTask ShowLeaderboardWithAnimation(
            string playerName,
            List<PlayerLeaderboardProfileData> previousLeaderboardData,
            List<PlayerLeaderboardProfileData> currentLeaderboardData
        )
        {
            int previousRank = GetPlayerLeaderboardRank(playerName, previousLeaderboardData);
            int currentRank = GetPlayerLeaderboardRank(playerName, currentLeaderboardData);
            
            UIView leaderboardView = UIView.GetViews("MenuUI", "Leaderboard").FirstOrDefault();

            if(leaderboardView != null)
            {
                LeaderboardDialog dialog = leaderboardView.gameObject.GetComponent<LeaderboardDialog>();

                dialog.Setup(
                    leaderboardProfileDataList: currentLeaderboardData,
                    playerName: playerName,
                    playerRank: currentRank,
                    hideProfileAtRank: currentRank
                );

                await UniTask.WaitUntil(() => dialog.IsScrollActiveAndEnable());
                await dialog.PlayRankChangeAnimation(previousRank, currentRank, currentLeaderboardData);
            }
            else
            {
                await UniTask.Yield();
            }
        }
    }
}