using HotPlay.Utilities;
using UnityEngine;

namespace HotPlay.PecanUI
{
    public class PecanSessionTimer : MonoBehaviour
    {
        public int Seconds => Mathf.CeilToInt(seconds);
        
        private bool active;
        private float seconds;

        public void StartSession()
        {
            Reset();
            active = true;
        }

        public void StopSession()
        {
            active = false;
        }
        
        public void Reset()
        {
            seconds = 0;
        }

        private void Update()
        {
            if (!active)
                return;

            seconds += Time.deltaTime;
        }
    }
}