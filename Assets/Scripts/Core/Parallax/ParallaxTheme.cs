using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using HotPlay.Utilities;
using Zenject;

namespace HotPlay.BoosterMath.Core
{
    public class ParallaxTheme
    {
        [Inject]
        private readonly List<ParallaxElement> elements = new List<ParallaxElement>();

        public void Move()
        {
            foreach (var element in elements)
            {
                element.Move();
            }
        }
        
        public void Stop()
        {
            foreach (var element in elements)
            {
                element.Stop();
            }
        }
        
        public async UniTask MoveForSeconds(float seconds)
        {
            Move();
            await UniTask.Delay(seconds.GetDurationMS());
            Stop();
        }
    }
}