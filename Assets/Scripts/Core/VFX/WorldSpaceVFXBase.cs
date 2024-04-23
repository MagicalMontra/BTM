using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace HotPlay.BoosterMath.Core.Character
{
    public abstract class WorldSpaceVFXBase : MonoBehaviour
    {
        public abstract UniTask Activate();
        public abstract UniTask Activate(CancellationToken cancellationToken);
        public abstract void Deactivate();

        public class Factory : PlaceholderFactory<WorldSpaceVFXBase, Vector3, Transform, WorldSpaceVFXBase>
        {
            
        }
    }
}