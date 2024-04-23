using UnityEngine;

namespace HotPlay.PecanUI.SceneLoader
{
    public abstract class LoadingProgressor : MonoBehaviour
    {
        public abstract void SetProgress(float percentage);
    }
}