using UnityEngine.SceneManagement;

namespace HotPlay.PecanUI.Example
{
    /// <summary>
    /// Example to handle for game with multiple scenes.
    /// </summary>
    public class LoadSceneHandler : Utilities.MonoSingleton<LoadSceneHandler>
    {
        private void Start()
        {
            PecanServices.Instance.Events.InitSoundsStatus(true, true);

            // For MonoSingleton bug where `instance` value won't set if `Instance` is never called and cause instance to not tracked properly
            _ = LoadSceneHandler.Instance;
        }
    }
}
