using Cysharp.Threading.Tasks;
using Eflatun.SceneReference;
using UnityEngine;

namespace HotPlay.PecanUI.SceneLoader
{
    public interface ISceneLoader
    {
        UniTask LoadUniTask(SceneReference scene);
    }
}