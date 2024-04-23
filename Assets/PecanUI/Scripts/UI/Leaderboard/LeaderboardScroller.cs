using System.Threading.Tasks;
using EnhancedUI.EnhancedScroller;
using HotPlay.Utilities;
using System.Threading;

namespace HotPlay.PecanUI.Leaderboard
{
    public class LeaderboardScroller : EnhancedScroller
    {
        public void CenterContentPanelAtItemIndex(int index)
        {
            JumpToDataIndex(
                dataIndex: index,
                scrollerOffset: 0.5f,
                cellOffset: 0.5f
            );
        }

        public async Task CenterContentPanelAtItemIndexAsync(int index, TweenType tweenType, float tweenTime, CancellationToken cancellationToken)
        {
            bool isJumpComplete = false;
            JumpToDataIndex(
                dataIndex: index,
                scrollerOffset: 0.5f,
                cellOffset: 0.5f,
                tweenType: tweenType,
                tweenTime: tweenTime,
                jumpComplete: () => isJumpComplete = true
            );
            await TaskHelper.When(
                predicate: () => isJumpComplete,
                cancellationToken: cancellationToken
            );
        }
    }
}