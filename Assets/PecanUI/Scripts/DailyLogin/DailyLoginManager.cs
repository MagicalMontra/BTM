using System;
using UnityEngine;

namespace HotPlay.DailyLogin
{
    public class DailyLoginManager
    {
        public bool IsClaimed => GetCurrentDate() <= nextClaimed;
        public bool IsClaimCoupon => GetCurrentDate() <= nextCouponClaimed;

        public int LastWeekClaimedCount => PlayerPrefs.GetInt(playerPrefsLastWeekCount, 0);

        private int[] rewards;

        private DateTime nextClaimed => epoch.AddSeconds(PlayerPrefs.GetInt(playerPrefsNextResetKey, 0));
        private DateTime nextCouponClaimed => epoch.AddSeconds(PlayerPrefs.GetInt(playerPrefNextCouponResetKey, 0));

        private int claimedCount => PlayerPrefs.GetInt(playerPrefsCountKey, 0);
        private DateTime epoch => new DateTime(1970, 1, 1, 0, 0, 0);

        private const string playerPrefsNextResetKey = "DAILY_LOGIN_NEXT_RESET";
        private const string playerPrefsCountKey = "DAILY_LOGIN_COUNT";
        private const string playerPrefsDebugDayKey = "DAILY_LOGIN_DEBUG_DAY";
        private const string playerPrefsLastWeekCount = "DAILY_LOGIN_LAST_WEEK_COUNT";
        private const string playerPrefNextCouponResetKey = "COUPON_NEXT_RESET";

        public DailyLoginManager(int[] rewards)
        {
            Debug.Assert(rewards != null, $"DailyLoginRewardData not found");
            this.rewards = rewards;
        }

        public int GetDailyLoginData(int index) => rewards[index];

        public void Claim(Action<int> callback)
        {
            if (IsClaimed)
            {
                return;
            }

            int reward = 0;
            int count = 0;

            count = GetCurrentRewardCount();

            reward = rewards[count - 1];
            SetNextClaim(GetToday().AddDays(1), count);

            if (nextClaimed != epoch && IsDifferentWeek(GetCurrentDate(), nextClaimed))
            {
                PlayerPrefs.SetInt(playerPrefsLastWeekCount, claimedCount);
                PlayerPrefs.Save();
            }

            callback?.Invoke(reward);
        }

        public int GetCurrentRewardCount()
        {
            var count = claimedCount;
            if (count >= 7 || nextClaimed.DayOfWeek == DayOfWeek.Monday || IsDifferentWeek(GetCurrentDate(), nextClaimed))
            {
                count = 1;
            }
            else
            {
                count++;
            }

            return count;
        }

        public DateTime GetCurrentDate()
        {
#if CHEAT_ENABLED
            return DateTime.Now.AddDays(GetDebugDays());
#else
            return DateTime.Now;
#endif
        }

        public DateTime GetToday()
        {
#if CHEAT_ENABLED
            return DateTime.Today.AddDays(GetDebugDays());
#else
            return DateTime.Today;
#endif
        }

        /// <summary>
        /// DEBUG ONLY, Forward time by days
        /// </summary>
        /// <param name="days"></param>
        public void TimeForward(int days)
        {
#if CHEAT_ENABLED
            int hackDay = PlayerPrefs.GetInt(playerPrefsDebugDayKey, 0);
            hackDay += days;
            PlayerPrefs.SetInt(playerPrefsDebugDayKey, hackDay);
            PlayerPrefs.Save();
#endif
        }

        /// <summary>
        /// DEBUG ONLY, Reset daily login
        /// </summary>
        public void ResetDailyLogIn()
        {
#if CHEAT_ENABLED
            PlayerPrefs.DeleteKey(playerPrefsNextResetKey);
            PlayerPrefs.DeleteKey(playerPrefsCountKey);
            PlayerPrefs.DeleteKey(playerPrefsDebugDayKey);
            PlayerPrefs.DeleteKey(playerPrefsLastWeekCount);
#endif
        }

        public static bool IsDifferentWeek(DateTime a, DateTime b)
        {
            var midnightA = a.Date;
            var midnightB = b.Date;

            var less = midnightA < midnightB ? midnightA : midnightB;
            var greater = midnightA < midnightB ? midnightB : midnightA;

            var resetTime = less.AddDays(getDayToAdd());

            return resetTime <= greater;

            int getDayToAdd() => less.DayOfWeek switch
            {
                DayOfWeek.Sunday => 1,
                DayOfWeek.Monday => 7,
                DayOfWeek.Tuesday => 6,
                DayOfWeek.Wednesday => 5,
                DayOfWeek.Thursday => 4,
                DayOfWeek.Friday => 3,
                DayOfWeek.Saturday => 2,
                _ => throw new InvalidOperationException(),
            };
        }

        public void SetCouponNextClaim()
        {
            DateTime nextClaim = GetToday().AddDays(1);
            PlayerPrefs.SetInt(playerPrefNextCouponResetKey, (int)(nextClaim - epoch).TotalSeconds);
            PlayerPrefs.Save();
        }

        private void SetNextClaim(DateTime nextClaim, int count)
        {
            PlayerPrefs.SetInt(playerPrefsNextResetKey, (int)(nextClaim - epoch).TotalSeconds);
            PlayerPrefs.SetInt(playerPrefsCountKey, count);
            PlayerPrefs.Save();
        }

        private int GetDebugDays()
        {
#if CHEAT_ENABLED
            return PlayerPrefs.GetInt(playerPrefsDebugDayKey, 0);
#else
            return 0;
#endif
        }
    }
}