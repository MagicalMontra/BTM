using Cysharp.Threading.Tasks;
using DG.Tweening;
using Doozy.Runtime.UIManager;
using Doozy.Runtime.UIManager.Components;
using EnhancedUI.EnhancedScroller;
using HotPlay.PecanUI.Events;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace HotPlay.PecanUI.Leaderboard
{
    public class LeaderboardDialog : BaseDialog
    {
        public enum ScrollDirection {
            Up = 0,
            Down = 1,
        }

        [SerializeField]
        private LeaderboardListManager leaderboardListManager;

        [SerializeField]
        private UIButton closeButton;

        [SerializeField]
        private float shortMovingAnimDuration = 1.5f;

        [SerializeField]
        private float longMovingAnimDuration = 2f;

#region  Animation Property
        [SerializeField]
        private int rankUpAnimationDelayMilliSecond = 1000;

        [SerializeField]
        private float rankChangeHorizontalDistant = 1000;

        [SerializeField]
        private float rankUpArrowAnimDuration = 0.5f;

        [SerializeField]
        private float rankUpTextShowAnimDuration = 0.1f;

        [SerializeField]
        private float rankUpTextShowDuration = 0.3f;

        [SerializeField]
        private float rankUpTextHideAnimDuration = 0.2f;
#endregion

        private List<PlayerLeaderboardProfileData> cachedLeaderboardProfileDataList;
        private string cachedPlayerName;
        private int cachedPlayerRank;
        protected CancellationTokenSource cancellationTokenSource;

#region  MonoBehaviour
        private void Start()
        {
            closeButton.behaviours.AddBehaviour(Doozy.Runtime.UIManager.UIBehaviour.Name.PointerClick).Event.AddListener(OnCloseButtonClicked);
        }
#endregion

        public void Setup(
            List<PlayerLeaderboardProfileData> leaderboardProfileDataList,
            string playerName,
            int playerRank,
            int hideProfileAtRank = -1
        )
        {
            SetupLeaderboardList(
                leaderboardProfileDataList: leaderboardProfileDataList,
                playerName: playerName,
                playerRank: playerRank,
                hideProfileAtRank: hideProfileAtRank
            );
        }

        public bool IsScrollActiveAndEnable()
        {
            return leaderboardListManager.IsScrollActiveAndEnable();
        }

        public void OnIntroAnimationFinished()
        {
            leaderboardListManager.SetEnableOnScrollBehaviour(true);
            leaderboardListManager.SetGoToTopCanvasGroupAlpha(1f);
            closeButton.enabled = true;
            SetScrollingInteractable(true);
            PecanServices.Instance.Events.LeaderboardEventsHandler.ClearSignalData();
#if PECAN_NAVIGATOR
            if (closeButton.gameObject.activeInHierarchy)
            {
                closeButton.Select();
            }
#endif
        }

        public async UniTask PlayRankChangeAnimation(int fromRank, int toRank, List<PlayerLeaderboardProfileData> updatedLeaderboardProfileDataList)
        {
            if (leaderboardListManager == null)
            { 
                await UniTask.Yield();
                return;
            }

            //TODO: add rank down support if need
            if(toRank >= fromRank)
            {
                ShowLeaderboard();
                await UniTask.Yield();
                return;
            }

#if PECAN_NAVIGATOR
            GameObject latestSelected = EventSystem.current.currentSelectedGameObject;
#endif
            closeButton.enabled = false;
            SetScrollingInteractable(false);
            leaderboardListManager.SetEnableOnScrollBehaviour(false);
            leaderboardListManager.SetGoToTopCanvasGroupAlpha(0f);
            leaderboardListManager.SetGoToPlayerCanvasGroupAlpha(0f).Forget();
            leaderboardListManager.StopScrollMovement();
            leaderboardListManager.CenterContentPanelAtItemIndex(fromRank - 1);
            

            LeaderboardListRow targetRankListRow = null;

            //Try get rankTo cell in current visible cell
            foreach (RectTransform rectTransform in leaderboardListManager.GetVisibleCellRects())
            {
                if (
                    rectTransform != null
                    && rectTransform.GetComponent<LeaderboardListRow>() is LeaderboardListRow leaderboardListRow
                )
                {
                    int index = leaderboardListRow.Index;
                    
                    if (index + 1 == toRank)
                    {                        
                        targetRankListRow = leaderboardListRow;
                        break;
                    }
                }
            }

            if(targetRankListRow != null)
            {
                await AnimateShortMovement(fromRank, toRank, updatedLeaderboardProfileDataList, targetRankListRow);
            }
            else
            {
                await AnimateLongMovement(fromRank, toRank, updatedLeaderboardProfileDataList);
            }

            await UniTask.Yield();
#if PECAN_NAVIGATOR
            EventSystem.current.SetSelectedGameObject(latestSelected);
#endif
        }

        private async UniTask AnimateShortMovement(int fromRank, int toRank, List<PlayerLeaderboardProfileData> updatedLeaderboardProfileDataList, LeaderboardListRow targetRankListRow)
        {
            float currentCanvasScale = PecanServices.Instance.GetCurrentCanvasScale();
            Vector3 rankChangeHorizontalDistantVector = new Vector3(rankChangeHorizontalDistant * currentCanvasScale, 0, 0);
            Vector3 rankChangeVerticalDistantVector = new Vector3(0, (leaderboardListManager.GetCellSize(0) + leaderboardListManager.Spacing) * currentCanvasScale, 0);

            LeaderboardListRow rankAnimatedRow = null;

            PlayerLeaderboardProfileData selfProfileData = null;
            foreach(var profileData in updatedLeaderboardProfileDataList)
            {
                //TODO: change to use player id instead
                if(profileData.PlayerName == PecanServices.Instance.Configs.LeaderboardDataProvider.PlayerName)
                {
                    selfProfileData = profileData;
                    break;
                }
            }

            targetRankListRow.SetVisibility(false);

            // Create leaderboard list row for moving animation
            foreach (RectTransform rectTransform in leaderboardListManager.GetVisibleCellRects())
            {
                if (
                    rectTransform != null
                    && rectTransform.GetComponent<LeaderboardListRow>() is LeaderboardListRow leaderboardlistRow
                )
                {
                    int index = leaderboardlistRow.Index;

                    if (index + 1 == fromRank)
                    {
                        rankAnimatedRow = leaderboardListManager.CreateLeaderboardAnimListRow(selfProfileData, toRank - 1, toRank, true);
                        rankAnimatedRow.SetVisibility(true);
                        rankAnimatedRow.transform.position = leaderboardlistRow.transform.position;
                        break;
                    }
                }
            }

            List<LeaderboardListRow> listToMoveDown = new List<LeaderboardListRow>();

            // Prepare leaderboard elements position between ranks for movedown animation
            if(targetRankListRow != null)
            {
                foreach (RectTransform rectTransform in leaderboardListManager.GetVisibleCellRects())
                {
                    if (
                        rectTransform != null
                        && rectTransform.GetComponent<LeaderboardListRow>() is LeaderboardListRow leaderboardListRow
                    )
                    {
                        int rowIndex = leaderboardListRow.Index;
                        if (rowIndex > toRank - 1 && rowIndex <= fromRank - 1)
                        {
                            listToMoveDown.Add(leaderboardListRow);
                            rectTransform.position += rankChangeVerticalDistantVector/2;
                        }
                    }
                }
            }
            
            await UniTask.Delay(rankUpAnimationDelayMilliSecond);

            if (rankAnimatedRow == null)
            {
                await UniTask.Yield();
                return;
            }

            if (leaderboardListManager == null)
            {
                await UniTask.Yield();
                return;
            }

            //Start moving animation
            List<UniTask> movingAnimationTasks = new List<UniTask>();
            float animDuration = shortMovingAnimDuration;

            foreach(var listRow in listToMoveDown)
            {
                Vector3 targetPos = listRow.GetComponent<RectTransform>().position - rankChangeVerticalDistantVector;
                movingAnimationTasks.Add(listRow.MoveTransformAsync(targetPos, DG.Tweening.Ease.OutExpo, animDuration));
            }
            
            UniTask moveTransformTask = rankAnimatedRow.MoveTransformAsync(targetRankListRow.transform.position, DG.Tweening.Ease.OutFlash, animDuration);
            movingAnimationTasks.Add(moveTransformTask);
            rankAnimatedRow.PlayRankUpWhiteBlinkingAnim();

            float rankUpArrowDelay = animDuration > rankUpArrowAnimDuration ? animDuration - rankUpArrowAnimDuration : 0;
            rankAnimatedRow.PlayRankUpIconAnim(rankUpArrowAnimDuration, rankUpArrowDelay);

            await UniTask.WhenAll(movingAnimationTasks);

            rankAnimatedRow.PlayRankUpTextAnim(fromRank - toRank, rankUpTextShowAnimDuration, rankUpTextShowDuration, rankUpTextHideAnimDuration, 
            () => 
            {
                if (rankAnimatedRow != null)
                {
                    Destroy(rankAnimatedRow.gameObject);
                }

                SetupLeaderboardList(
                    leaderboardProfileDataList: updatedLeaderboardProfileDataList,
                    playerName: cachedPlayerName,
                    playerRank: toRank
                );

                leaderboardListManager.CenterContentPanelAtItemIndex(fromRank - 1);
                OnIntroAnimationFinished();
            });
            
            await UniTask.Yield();
        }

        private async UniTask AnimateLongMovement(int fromRank, int toRank, List<PlayerLeaderboardProfileData> updatedLeaderboardProfileDataList)
        {
            float currentCanvasScale = PecanServices.Instance.GetCurrentCanvasScale();
            Vector3 rankChangeHorizontalDistantVector = new Vector3(rankChangeHorizontalDistant * currentCanvasScale, 0, 0);
            Vector3 rankChangeVerticalDistantVector = new Vector3(0, (leaderboardListManager.GetCellSize(0) + leaderboardListManager.Spacing) * currentCanvasScale, 0);

            LeaderboardListRow rankAnimatedRow = null;

            leaderboardListManager.PrepareCloneContent(toRank - 1);

            PlayerLeaderboardProfileData selfProfileData = null;
            foreach(var profileData in updatedLeaderboardProfileDataList)
            {
                //TODO: change to use player id instead
                if(profileData.PlayerName == PecanServices.Instance.Configs.LeaderboardDataProvider.PlayerName)
                {
                    selfProfileData = profileData;
                    break;
                }
            }

            // Create leaderboard list row for moving animation
            foreach (RectTransform rectTransform in leaderboardListManager.GetVisibleCellRects())
            {
                if (
                    rectTransform != null
                    && rectTransform.GetComponent<LeaderboardListRow>() is LeaderboardListRow leaderboardlistRow
                )
                {
                    int index = leaderboardlistRow.Index;
                    if (index + 1 == fromRank)
                    {
                        rankAnimatedRow = leaderboardListManager.CreateLeaderboardAnimListRow(selfProfileData, toRank - 1, toRank, true);
                        rankAnimatedRow.SetVisibility(true);
                        rankAnimatedRow.transform.position = leaderboardlistRow.transform.position;
                        break;
                    }
                }
            }
            
            await UniTask.Delay(rankUpAnimationDelayMilliSecond);

            if (rankAnimatedRow == null)
            {
                return;
            }

            if (leaderboardListManager == null)
            {
                return;
            }

            //Start moving animation;

            List<UniTask> movingAnimationTasks = new List<UniTask>();
            float animDuration = longMovingAnimDuration;
            movingAnimationTasks.Add(leaderboardListManager.AnimateCenterContentPanelAtItemIndex(toRank - 1, animDuration));
            movingAnimationTasks.Add(rankAnimatedRow.MoveTransformAsync(leaderboardListManager.AnimIndicator.transform.position, DG.Tweening.Ease.OutFlash, animDuration));
            rankAnimatedRow.PlayRankUpWhiteBlinkingAnim();

            float rankUpArrowDelay = animDuration > rankUpArrowAnimDuration ? animDuration - rankUpArrowAnimDuration : 0;

            rankAnimatedRow.PlayRankUpIconAnim(rankUpArrowAnimDuration, rankUpArrowDelay);
            
            await UniTask.WhenAll(movingAnimationTasks);
            
            rankAnimatedRow.PlayRankUpTextAnim(fromRank - toRank, rankUpTextShowAnimDuration, rankUpTextShowDuration, rankUpTextHideAnimDuration, 
            () => 
            {
                if (rankAnimatedRow != null)
                {
                    Destroy(rankAnimatedRow.gameObject);
                }

                SetupLeaderboardList(
                    leaderboardProfileDataList: updatedLeaderboardProfileDataList,
                    playerName: cachedPlayerName,
                    playerRank: toRank
                );
                
                OnIntroAnimationFinished();
            });

            await UniTask.Yield();
        }

        private void OnCloseButtonClicked()
        {
            PecanServices.Instance.Events.LeaderboardEventsHandler.InvokeCloseButtonClicked();
        }

        private async void ShowLeaderboardWithAnimation(LeaderboardSignalData signalData)
        {
            PlayerLeaderboardProfileData prevProfile = ScriptableObject.CreateInstance<PlayerLeaderboardProfileData>();
                prevProfile.Setup(
                    playerName: PecanServices.Instance.Configs.LeaderboardDataProvider.PlayerName,
                    avatarSprite: PecanServices.Instance.Configs.LeaderboardDataProvider.PlayerSprite,
                    score: signalData.PrevScore,
                    isHumanPlayer: true
                );

            PlayerLeaderboardProfileData currentProfile = ScriptableObject.CreateInstance<PlayerLeaderboardProfileData>();
                currentProfile.Setup(
                    playerName: PecanServices.Instance.Configs.LeaderboardDataProvider.PlayerName,
                    avatarSprite: PecanServices.Instance.Configs.LeaderboardDataProvider.PlayerSprite,
                    score: signalData.CurrentScore,
                    isHumanPlayer: true
                );

            List<PlayerLeaderboardProfileData> prevLeaderboardData = LeaderboardService.GenerateLeaderboardDataWithFakeProfiles(PecanServices.Instance.Configs.LeaderboardDataProvider.LeaderboardData, prevProfile);
            List<PlayerLeaderboardProfileData> newLeaderboardData = LeaderboardService.GenerateLeaderboardDataWithFakeProfiles(PecanServices.Instance.Configs.LeaderboardDataProvider.LeaderboardData, currentProfile);

            await LeaderboardService.ShowLeaderboardWithAnimation(PecanServices.Instance.Configs.LeaderboardDataProvider.PlayerName, prevLeaderboardData, newLeaderboardData);
        }

        private void ShowLeaderboard()
        {
            PlayerLeaderboardProfileData currentProfile = ScriptableObject.CreateInstance<PlayerLeaderboardProfileData>();
            currentProfile.Setup(
                playerName: PecanServices.Instance.Configs.LeaderboardDataProvider.PlayerName,
                avatarSprite: PecanServices.Instance.Configs.LeaderboardDataProvider.PlayerSprite,
                score: PecanServices.Instance.GetHighScore(),
                isHumanPlayer: true
            );

            List<PlayerLeaderboardProfileData> leaderboardData = LeaderboardService.GenerateLeaderboardDataWithFakeProfiles(PecanServices.Instance.Configs.LeaderboardDataProvider.LeaderboardData, currentProfile);
            int playerRank = LeaderboardService.GetPlayerLeaderboardRank(PecanServices.Instance.Configs.LeaderboardDataProvider.PlayerName, leaderboardData);

            LeaderboardService.ShowLeaderboard(PecanServices.Instance.Configs.LeaderboardDataProvider.PlayerName, leaderboardData);
        }

#region  BaseDialog override functions
        protected override void OnShowing()
        {
            var openType = LeaderboardDialogOpenType.Manual;

            if (PecanServices.Instance.Events.LeaderboardEventsHandler.HasSignalData
                && PecanServices.Instance.Events.LeaderboardEventsHandler.SignalData.OpenType == LeaderboardDialogOpenType.Auto)
            {
                openType = LeaderboardDialogOpenType.Auto;
            }

            switch(openType)
            {
                case LeaderboardDialogOpenType.Auto:
                    ShowLeaderboardWithAnimation(PecanServices.Instance.Events.LeaderboardEventsHandler.SignalData);
                    break;
                case LeaderboardDialogOpenType.Manual:
                    ShowLeaderboard();
                    break;
                default:
                    throw new InvalidOperationException($"Cannot open leaderboard with type {openType}");
            }
            PecanServices.Instance.Events.LeaderboardEventsHandler.RaiseLeaderboardDialogOpened(openType);
        }

        protected override void OnHidden()
        {
            cancellationTokenSource?.Cancel();
            cancellationTokenSource?.Dispose();
        }
#endregion

        private void SetupLeaderboardList(
            List<PlayerLeaderboardProfileData> leaderboardProfileDataList,
            string playerName,
            int playerRank,
            int hideProfileAtRank = -1
        )
        {
            if (leaderboardListManager == null)
            {
                return;
            }
            
            cachedLeaderboardProfileDataList = leaderboardProfileDataList;
            cachedPlayerName = playerName;
            cachedPlayerRank = playerRank;
            leaderboardListManager.Setup(
                playerLeaderboardProfileList: leaderboardProfileDataList,
                currentPlayerRank: playerRank,
                itemIndexToCenter: playerRank - 1,
                hiddenItemIndex: hideProfileAtRank - 1
            );
        }

        private async Task CenterContentByRank(int rank)
        {
            if (leaderboardListManager == null)
            {
                return;
            }

            leaderboardListManager.StopScrollMovement();
            await leaderboardListManager.AnimateCenterContentPanelAtItemIndex(rank - 1);
        }

        private void SetScrollingInteractable(bool isInteractable)
        {
            //moveToMeButton.SetInteractable(isInteractable);
            //moveToTopButton.SetInteractable(isInteractable);

            if (leaderboardListManager == null)
            {
                return;
            }

            leaderboardListManager.SetScrollInteractable(false, isInteractable);
        }
    }
}
