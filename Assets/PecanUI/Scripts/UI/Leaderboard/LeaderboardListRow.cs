using Cysharp.Threading.Tasks;
using HotPlay.Pooling;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Threading.Tasks;
using DG.Tweening;
using Doozy.Runtime.UIManager.Animators;
using HotPlay.PecanUI.Skin;
using I2.Loc;

namespace HotPlay.PecanUI.Leaderboard
{
    public class LeaderboardListRow : PoolObject<LeaderboardListRow>
    {
        public int Index { get; private set; }
        
        public RectTransform Rect => rect;
        
        [SerializeField]
        private RectTransform rect;

        [SerializeField]
        private CanvasGroup canvasGroup;

        [SerializeField]
        private TMP_Text rankText;

        [SerializeField]
        private Image tagImageBottom;

        [SerializeField]
        private Image tagImageTop;

        [SerializeField]
        private Image tagImageOutline;

        [SerializeField]
        private Image avatarImage;

        [SerializeField]
        private Image bgImage;

        [SerializeField]
        private Image bevelImage;

        [SerializeField]
        private TMP_Text playerNameText;

        [SerializeField]
        private TMP_Text scoreText;

        [SerializeField]
        private float moveAnimationDuration = 1f;

#region Tag BG Color
        [SerializeField]
        private Color firstPlaceFrameBottomColor;

        [SerializeField]
        private Color secondPlaceFrameBottomColor;

        [SerializeField]
        private Color thirdPlaceFrameBottomColor;

        private Color normalFrameBottomColor;

        [SerializeField]
        private Color selfFrameBottomColor;

        [SerializeField]
        private Color firstPlaceFrameTopColor;

        [SerializeField]
        private Color secondPlaceFrameTopColor;

        [SerializeField]
        private Color thirdPlaceFrameTopColor;

        private Color normalFrameTopColor;

        [SerializeField]
        private Color selfFrameTopColor;
#endregion

#region Outline Color
        [SerializeField]
        private Color firstPlaceFrameOutlineColor;

        [SerializeField]
        private Color secondPlaceFrameOutlineColor;

        [SerializeField]
        private Color thirdPlaceFrameOutlineColor;

        private Color normalFrameOutlineColor;

        [SerializeField]
        private Color selfFrameOutlineColor;
#endregion

#region Ranking Text Color
        [SerializeField]
        private Color firstPlaceTextColor;

        [SerializeField]
        private Color secondPlaceTextColor;

        [SerializeField]
        private Color thirdPlaceTextColor;

        private Color normalTextColor;

        [SerializeField]
        private Color selfTextColor;
#endregion

#region
        [SerializeField]
        private Color selfNameTextColor;
#endregion

#region  BG color
        private Color normalBGColor;

        [SerializeField]
        private Color selfBGColor;
#endregion

#region Bevel color
        [SerializeField]
        private Color selfBevelColor;
#endregion

        #region Rank Up Arrow Anim
        [SerializeField]
        private CanvasGroup rankUpArrowCanvasGroup;

        [SerializeField]
        private RectTransform rankUpArrow;

        [SerializeField]
        private RectTransform arrowStartPosition;

        [SerializeField]
        private RectTransform arrowTargetPostion;
#endregion

#region  Rank Up Text Anim
        [SerializeField]
        private TextMeshProUGUI rankUpText;

        [SerializeField]
        private LocalizationParamsManager rankUpTextParamsManager;

        [SerializeField]
        private CanvasGroup rankUpTextCanvasGroup;
#endregion

#region  Rank Up White Blinking Anim
        [SerializeField]
        private CanvasGroup rankUpWhiteBlinkingCanvasGroup;

        [SerializeField]
        private float targetWhiteBlinkingAlpha;

        [SerializeField]
        private float whiteBlinkingDuration;
#endregion

        [SerializeField]
        private SkinTMPHandler scoreTextSkin;

        [SerializeField]
        private SkinTMPHandler rankTextSkin;
        
        [SerializeField]
        private SkinTMPHandler nameTextSkin;
        
        [SerializeField]
        private SkinImageHandler depthSkin;
        
        [SerializeField]
        private SkinImageHandler bevelSkin;
        
        [SerializeField]
        private SkinImageHandler outlineSkin;

        [SerializeField]
        private SkinImageHandler backgroundSkin;
        
        [SerializeField]
        private SkinImageHandler playerBorderSkin;

        [SerializeField]
        private SkinImageHandler leaderboardTagTopSkin;

        [SerializeField]
        private SkinImageHandler leaderboardTagBottomSkin;
        
        [SerializeField]
        private SkinImageHandler leaderboardTagOutlineSkin;

        [SerializeField]
        private UIContainerUIAnimator uiAnimator;

        private void Awake()
        {
            uiAnimator.showAnimation.OnPlayCallback.AddListener(SetShow);
            uiAnimator.hideAnimation.OnFinishCallback.AddListener(SetHide);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            uiAnimator.showAnimation.OnPlayCallback.RemoveListener(SetShow);
            uiAnimator.hideAnimation.OnFinishCallback.RemoveListener(SetHide);
        }

        public void Setup(PlayerLeaderboardProfileData profileData, int index, int currentPlayerRank, bool isVisible, bool isFloating = false)
        {
            if (profileData == null)
            {
                return;
            }

            Index = index;
            int rank = index + 1;
            bool isCurrentPlayer = currentPlayerRank == index + 1;
            var pecanSkinConfig = PecanServices.Instance.SkinConfigs;

            if(isCurrentPlayer)
            {
                bgImage.color = selfBGColor;

                if (isFloating)
                {
                    bevelImage.color = selfBevelColor;
                    playerNameText.color = selfNameTextColor;
                }
                else
                {
                    bevelImage.color = selfBGColor;
                    nameTextSkin.UpdateSkin(pecanSkinConfig);
                }
            }
            else
            {
                bevelSkin.UpdateSkin(pecanSkinConfig);
                nameTextSkin.UpdateSkin(pecanSkinConfig);
                backgroundSkin.UpdateSkin(pecanSkinConfig);
            }
            

            depthSkin.UpdateSkin(pecanSkinConfig);
            outlineSkin.UpdateSkin(pecanSkinConfig);
            scoreTextSkin.UpdateSkin(pecanSkinConfig);
            playerBorderSkin.UpdateSkin(pecanSkinConfig);

            Color tagBottomColor = GetTagBottomColor(rank, isCurrentPlayer);
            Color tagTopColor = GetTagTopColor(rank, isCurrentPlayer);
            Color tagOutlineColor = GetTagOutlineColor(rank, isCurrentPlayer);
            Color rankTextColor = GetRankTextColor(rank, isCurrentPlayer);

            tagImageBottom.color = tagBottomColor;
            tagImageTop.color = tagTopColor;
            tagImageOutline.color = tagOutlineColor;
            rankText.color = rankTextColor;

            rankText.text = (rank).ToString();
            avatarImage.sprite = profileData.AvatarSprite;
            playerNameText.text = profileData.PlayerName;
            scoreText.text = profileData.Score.ToString();
            SetVisibility(isVisible);
        }

        public void SetVisibility(bool isVisible)
        {
            canvasGroup.alpha = isVisible ? 1 : 0;
        }

        private bool hasShow;

        public async UniTask AnimateTransitionAnimation(bool shouldShow)
        {
            if(shouldShow && !hasShow)
            {
                uiAnimator.Show();
                await UniTask.WaitUntil(() => hasShow);
            }
            else if(!shouldShow && hasShow)
            {
                uiAnimator.Hide();
                await UniTask.WaitUntil(() => !hasShow);
            }
        }

        private void SetShow()
        {
            hasShow = true;
        }

        private void SetHide()
        {
            hasShow = false;
        }

        public async UniTask MoveTransformAsync(Vector3 targetPosition, Ease ease,float overrideDuration = 0)
        {
            Transform t = GetComponent<Transform>();
            float duration = overrideDuration > 0 ? overrideDuration : moveAnimationDuration;
            float time = 0;
            t.DOMove(targetPosition, duration, true).SetEase(ease);
            while (
                t != null &&
                time <= duration
            )
            {
                time += Time.deltaTime;
                await Task.Yield();
            }
        }

        public void PlayRankUpIconAnim(float duration, float delay)
        {
            rankUpArrowCanvasGroup.alpha = 0;
            rankUpArrow.anchoredPosition = arrowStartPosition.anchoredPosition;
            rankUpArrowCanvasGroup.DOFade(1, duration).SetEase(Ease.Flash).SetDelay(delay);
            
            rankUpArrow.DOLocalMoveY(arrowTargetPostion.anchoredPosition.y, duration).SetEase(Ease.InFlash).SetDelay(delay).OnComplete
            (() => 
            {
                rankUpArrowCanvasGroup.alpha = 0;
                rankUpArrow.anchoredPosition = arrowStartPosition.anchoredPosition;
            });
        }

        public void PlayRankUpTextAnim(int value, float showAnimDuration, float showDuration, float hideAnimDuration, Action callback)
        {
            rankUpTextCanvasGroup.alpha = 0;
            rankUpTextParamsManager.SetParameterValue("RANK", value.ToString(), true);
            Sequence sequence = DOTween.Sequence(this);
            sequence.Append(rankUpTextCanvasGroup.DOFade(1, showAnimDuration).SetEase(Ease.Flash));
            sequence.Append(rankUpTextCanvasGroup.DOFade(0, hideAnimDuration).SetEase(Ease.Flash).SetDelay(showDuration));
            sequence.OnComplete(() => 
            {
                callback?.Invoke();
            });

            sequence.Play();
        }

        public void PlayRankUpWhiteBlinkingAnim()
        {
            rankUpWhiteBlinkingCanvasGroup.alpha = 0;
            Sequence sequence = DOTween.Sequence(this);
            sequence.Append(rankUpWhiteBlinkingCanvasGroup.DOFade(targetWhiteBlinkingAlpha, whiteBlinkingDuration / 2).SetEase(Ease.Linear));
            sequence.Append(rankUpWhiteBlinkingCanvasGroup.DOFade(0, whiteBlinkingDuration / 2)).SetEase(Ease.Linear);

            sequence.Play();
        }

        private Color GetTagBottomColor(int rank, bool isCurrentPlayer)
        {
            switch (rank)
            {
                case 1: return firstPlaceFrameBottomColor;
                case 2: return secondPlaceFrameBottomColor;
                case 3: return thirdPlaceFrameBottomColor;
                default:
                    if(isCurrentPlayer)
                    {
                        return selfFrameBottomColor;
                    }
                    else
                    {
                        var data = leaderboardTagBottomSkin.GetValue(PecanServices.Instance.SkinConfigs);
                        return data?.Color ?? selfFrameBottomColor;
                    }
            }
        }

        private Color GetTagTopColor(int rank, bool isCurrentPlayer)
        {
            switch (rank)
            {
                case 1: return firstPlaceFrameTopColor;
                case 2: return secondPlaceFrameTopColor;
                case 3: return thirdPlaceFrameTopColor;
                default:
                    if(isCurrentPlayer)
                    {
                        return selfFrameTopColor;
                    }
                    else
                    {
                        var data = leaderboardTagTopSkin.GetValue(PecanServices.Instance.SkinConfigs);
                        return data?.Color ?? selfFrameTopColor;
                    }
            }
        }

        private Color GetTagOutlineColor(int rank, bool isCurrentPlayer)
        {
            switch (rank)
            {
                case 1: return firstPlaceFrameOutlineColor;
                case 2: return secondPlaceFrameOutlineColor;
                case 3: return thirdPlaceFrameOutlineColor;
                default:
                    if(isCurrentPlayer)
                    {
                        return selfFrameOutlineColor;
                    }
                    else
                    {
                        var data = leaderboardTagOutlineSkin.GetValue(PecanServices.Instance.SkinConfigs);
                        return data?.Color ?? selfFrameOutlineColor;
                    }
            }
        }

        private Color GetRankTextColor(int rank, bool isCurrentPlayer)
        {
            switch (rank)
            {
                case 1: return firstPlaceTextColor;
                case 2: return secondPlaceTextColor;
                case 3: return thirdPlaceTextColor;
                default: 
                    if(isCurrentPlayer)
                    {
                        return selfTextColor;
                    }
                    else
                    {
                        var data = rankTextSkin.GetValue(PecanServices.Instance.SkinConfigs);
                        rankTextSkin.UpdateSkin(PecanServices.Instance.SkinConfigs);
                        return data?.FontColor ?? selfFrameOutlineColor;
                    }
            }
        }
    }
}