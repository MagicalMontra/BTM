using Doozy.Runtime.UIManager.Components;
using HotPlay.PecanUI.Gameplay;
using HotPlay.Utilities;
using UnityEngine;

namespace HotPlay.PecanUI.Debugs
{
    public class GameResultClick : MonoBehaviour
    {
        [SerializeField]
        private GameResultData resultData;

        private void Awake()
        {
            GetComponent<UIButton>().behaviours.AutoGetUnityEvent(Doozy.Runtime.UIManager.UIBehaviour.Name.PointerLeftClick).AddListener(GameOver);
        }

        public void GameOver()
        {
            PecanServices.Instance.Signals.SendResultSignal(resultData);
        }
    }
}
