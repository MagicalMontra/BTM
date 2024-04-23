using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using Doozy.Runtime.UIManager.Components;
using HotPlay.Pooling;
using HotPlay.UI.RenderManagement;
using UnityEngine;

#if PECAN_NAVIGATOR
using UnityEngine.EventSystems;
#endif

namespace HotPlay.PecanUI.Leaderboard
{
    public class LeaderboardListManager : VerticalPooledRenderer
    {
        [SerializeField]
        private LeaderboardListRow leaderboardListRowPrefab;

        [SerializeField]
        private float autoScrollDuration = 2f;

        [SerializeField]
        private float autoScrollSpeed = 100f; //pixel/sec

        [SerializeField]
        private RectTransform contentClone;

        [SerializeField]
        private RectTransform animIndicator;
        public RectTransform AnimIndicator => animIndicator;

        [SerializeField]
        private LeaderboardListRow floatingListRow;

        [SerializeField]
        private CanvasGroup floatingListRowcanvasGroup;

        [SerializeField]
        private CanvasGroup goToTopCanvasGroup;

        [SerializeField]
        private UIButton goToTopButton;

        private bool isMoving;
        private bool hasPlayerRank;
        private int currentPlayerRank;
        private int hiddenItemIndex; //Used for animation flow
        private bool enableOnScrollBehaviour = true;

        public void GoToTop() => MoveToTop().Forget();
        
        public void GoToPlayer() => MoveToPlayer().Forget();
        
        private async UniTaskVoid MoveToTop()
        {
            if (isMoving)
                return;
            
            isMoving = true;
            
#if PECAN_NAVIGATOR
            GameObject latestSelected = EventSystem.current.currentSelectedGameObject;
#endif
            
            StopScrollMovement();
            SetScrollInteractable(false, false);
            StopScrollMovement();
            SetGoToTopCanvasGroupAlpha(0f);

            if (floatingListRow.Index < 6 || hasPlayerRank)
                SetGoToPlayerCanvasGroupAlpha(0f).Forget();
            
            enableOnScrollBehaviour = false;
            await AnimateCenterContentPanelAtItemIndex(0);
            UpdatePlayerRankVisibility(GetVisibleCellRects());
            SetGoToPlayerCanvasGroupAlpha(1f).Forget();
            if (this == null)
            {
                await UniTask.Yield();
                return;
            }
            
            enableOnScrollBehaviour = true;
            isMoving = false;
            SetScrollInteractable(false, true);
            
#if PECAN_NAVIGATOR
            EventSystem.current.SetSelectedGameObject(latestSelected);
#endif

            await UniTask.Yield();
        }

        private async UniTaskVoid MoveToPlayer()
        {
            if (isMoving)
                return;

            isMoving = true;
            
#if PECAN_NAVIGATOR
            GameObject latestSelected = EventSystem.current.currentSelectedGameObject;
#endif
            
            StopScrollMovement();
            SetScrollInteractable(false, false);
            StopScrollMovement();
            UpdatePlayerRankVisibility(GetVisibleCellRects());
            SetGoToPlayerCanvasGroupAlpha(0f).Forget();
            SetGoToTopCanvasGroupAlpha(0f);
            enableOnScrollBehaviour = false;
            await AnimateCenterContentPanelAtItemIndex(floatingListRow.Index);
            
            if (floatingListRow.Index >= 4)
                SetGoToTopCanvasGroupAlpha(1f);
            
            if (this == null)
            {
                await UniTask.Yield();
                return;
            }
            
            enableOnScrollBehaviour = true;
            isMoving = false;
            SetScrollInteractable(false, true);
            
#if PECAN_NAVIGATOR
            EventSystem.current.SetSelectedGameObject(latestSelected);
#endif
            
            await UniTask.Yield();
        }

        public void SetEnableOnScrollBehaviour(bool value)
        {
            enableOnScrollBehaviour = value;
        }

        public void SetGoToTopCanvasGroupAlpha(float value)
        {
            goToTopCanvasGroup.alpha = value;
            goToTopCanvasGroup.interactable = value > 0f;
        }

        public async UniTaskVoid SetGoToPlayerCanvasGroupAlpha(float value)
        {
            var neededFade = value > 0f && !hasPlayerRank;
            if (neededFade)
                floatingListRowcanvasGroup.DOFade(1f, 0.5f);
            
            await floatingListRow.AnimateTransitionAnimation(value > 0f && !hasPlayerRank);

            if (!neededFade)
            {
#if  UNITASK_DOTWEEN_SUPPORT
                await floatingListRowcanvasGroup.DOFade(0f, 0.5f);
#else
                floatingListRowcanvasGroup.DOFade(0f, 0.5f);
                await UniTask.Delay(500, DelayType.DeltaTime);
#endif

            }
            
            await UniTask.Yield();
        }

        public void Setup(List<PlayerLeaderboardProfileData> playerLeaderboardProfileList, int currentPlayerRank, int itemIndexToCenter, int hiddenItemIndex)
        {
            this.currentPlayerRank = currentPlayerRank;
            this.hiddenItemIndex = hiddenItemIndex;
            Clear();
            float rowHeight = leaderboardListRowPrefab.GetComponent<RectTransform>().rect.height;

            foreach (PlayerLeaderboardProfileData profile in playerLeaderboardProfileList)
            {
                AddCellInfo(rowHeight, profile);
            }

            InitContentSizeAndPosition();
            CenterContentPanelAtItemIndex(itemIndexToCenter);

        }

        public LeaderboardListRow CreateLeaderboardAnimListRow(PlayerLeaderboardProfileData playerLeaderboardProfileList, int index, int currentPlayerRank, bool isVisible)
        {
            LeaderboardListRow leaderboardListRow = Instantiate(leaderboardListRowPrefab, AnimatingCellsParentRect);
            leaderboardListRow.Setup(playerLeaderboardProfileList, index, currentPlayerRank, isVisible);
            return leaderboardListRow;
        }

        public void CenterContentPanelAtItemIndex(int index)
        {
            Vector2 targetAnchoredPosition = CalculateCenteredPositionOfItemIndex(index);
            ContentPanel.anchoredPosition = targetAnchoredPosition;
            UpdateVisibleContent();
        }

        public void PrepareCloneContent(int index)
        {
            contentClone.sizeDelta = ContentPanel.sizeDelta;
            Vector3 targetAnchoredPosition = CalculateCenteredPositionOfItemIndex(index, contentClone);
            contentClone.anchoredPosition3D = targetAnchoredPosition;
            CellInfo cellInfo = GetCellInfoByIndex(index);
            SetPosition(cellInfo, animIndicator, cellInfo.Position);
        }

        public bool IsScrollActiveAndEnable()
        {
            return scroll.isActiveAndEnabled;
        }

        public void SetActiveScroll(bool v)
        {
            scroll.enabled = v;
        }

        public async UniTask AnimateCenterContentPanelAtItemIndex(int index, bool isFixedSpeed = false)
        {
            Vector3 targetAnchoredPosition = CalculateCenteredPositionOfItemIndex(index);
            float duration = isFixedSpeed ? Vector3.Distance(ContentPanel.anchoredPosition3D, targetAnchoredPosition) / autoScrollSpeed : autoScrollDuration;
            await AnimateCenterContentPanelAtItemIndex(index, targetAnchoredPosition, duration);
            await UniTask.Yield();
        }

        public async UniTask AnimateCenterContentPanelAtItemIndex(int index, float duration)
        {
            Vector3 targetAnchoredPosition = CalculateCenteredPositionOfItemIndex(index);
            await AnimateCenterContentPanelAtItemIndex(index, targetAnchoredPosition, duration);
            await UniTask.Yield();
        }

        private async UniTask AnimateCenterContentPanelAtItemIndex(int index, Vector3 targetAnchoredPosition, float duration)
        {
#if UNITASK_DOTWEEN_SUPPORT
            await ContentPanel.DOAnchorPos3D(targetAnchoredPosition, duration, true).SetEase(Ease.OutExpo);
#else
            ContentPanel.DOAnchorPos3D(targetAnchoredPosition, duration, true).SetEase(Ease.OutExpo);
            float accumDeltaTime = 0;
            while (
                ContentPanel != null
                && accumDeltaTime < duration
            )
            {
                accumDeltaTime += Time.deltaTime;
                await UniTask.Yield();
            }
#endif

            if (ContentPanel != null)
            {
                UpdateVisibleContent();
            }

            await UniTask.Yield();
        }

        public float GetCenterContenAnimDuration(int index, bool isFixedSpeed = false)
        {
            if (isFixedSpeed)
            {
                Vector2 targetAnchoredPosition = CalculateCenteredPositionOfItemIndex(index);
                return Vector3.Distance(ContentPanel.anchoredPosition3D, targetAnchoredPosition) / autoScrollSpeed;
            }
            else
            {
                return autoScrollDuration;
            }
        }

        public async Task MoveToCenter(Transform transform, bool isFixedSpeed)
        {
            Vector3 targetPosition = viewportPoint.transform.position;
            float duration = isFixedSpeed ? Vector3.Distance(transform.position, targetPosition) / autoScrollSpeed : autoScrollDuration;
            float accumDeltaTime = 0;
            while (
                ContentPanel != null
                && Vector3.Distance(transform.position, targetPosition) > 1f
                && accumDeltaTime < duration
            )
            {
                accumDeltaTime += Time.deltaTime;
                transform.position = Vector3.Lerp(transform.position, targetPosition, accumDeltaTime / duration);
                if (Vector3.Distance(transform.position, targetPosition) <= 1f)
                {
                    transform.position = targetPosition;
                }
                await Task.Yield();
            }
        }

        protected override RectTransform CreateCellContent(CellInfo info)
        {
            LeaderboardListRow row = leaderboardListRowPrefab.Rent(Vector3.zero, Quaternion.identity, Vector3.one, ContentPanel);
            row.Setup(
                profileData: info.Payload as PlayerLeaderboardProfileData,
                index: info.Index,
                currentPlayerRank: currentPlayerRank,
                isVisible: info.Index != hiddenItemIndex
            );

            // floating row always be current player
            if (info.Index + 1 == currentPlayerRank)
            {
                floatingListRow.Setup(
                    profileData: info.Payload as PlayerLeaderboardProfileData,
                    index: info.Index,
                    currentPlayerRank: currentPlayerRank,
                    isVisible: true,
                    isFloating: true
            );
            }
            return row.GetComponent<RectTransform>();
        }

        protected override void RemoveCellContent(CellInfo info)
        {
            info.RenderedObject?.GetComponent<PoolObject>()?.Return();
        }

        protected override void OnScrolled()
        {
            base.OnScrolled();

            if (!enableOnScrollBehaviour)
            {
                return;
            }
            
            List<RectTransform> visibleCellRects = GetVisibleCellRects();
            hasPlayerRank = false;
            UpdatePlayerRankVisibility(visibleCellRects);
            floatingListRowcanvasGroup.DOFade(!hasPlayerRank ? 1 : 0, 0.5f);
            floatingListRow.AnimateTransitionAnimation(!hasPlayerRank).Forget();

            bool hasFirstRank = false;
            foreach (var cellRect in visibleCellRects)
            {
                LeaderboardListRow listRow = cellRect.GetComponent<LeaderboardListRow>();
                if (listRow != null && listRow.Index == 0)
                {
                    hasFirstRank = true;
                    break;
                }
            }

            SetGoToTopCanvasGroupAlpha(!hasFirstRank ? 1f : 0f);
        }

        private void UpdatePlayerRankVisibility(List<RectTransform> visibleCellRects)
        {
            foreach (var cellRect in visibleCellRects)
            {
                LeaderboardListRow listRow = cellRect.GetComponent<LeaderboardListRow>();
                if (listRow != null && listRow.Index == floatingListRow.Index)
                {
                    hasPlayerRank = true;
                    break;
                }

                hasPlayerRank = false;
            }
        }
    }
}