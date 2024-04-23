using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using HotPlay.Utilities;
using UnityEngine;
using Zenject;

namespace HotPlay.BoosterMath.Core
{
    public class ParallaxThemeController : MonoBehaviour
    {
        public string Id => id;
        
        [SerializeField]
        private string id;
        
        [SerializeField]
        private List<ParallaxElement> elements = new List<ParallaxElement>();

        public void OnDestroy()
        {
            Stop();
        }

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
        
        public async UniTask MoveForSeconds(float seconds, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                await UniTask.Yield();
                return;
            }
            
            Move();
            
            if (cancellationToken.IsCancellationRequested)
            {
                Stop();
                await UniTask.Yield();
                return;
            }
            
            await UniTask.Delay(seconds.GetDurationMS(), DelayType.DeltaTime, PlayerLoopTiming.Update, cancellationToken);
            Stop();
        }

        public class Factory : PlaceholderFactory<Object, ParallaxThemeController>
        {
            
        }
    }
}