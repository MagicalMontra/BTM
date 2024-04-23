using UnityEngine;

namespace HotPlay.PecanUI
{
    [CreateAssetMenu(menuName = "Create SignalProcessorDatabase", fileName = "SignalProcessorDatabase", order = 0)]
    public class SignalProcessorDatabase : ScriptableObject
    {
        public SignalData[] Data => data;
        
        [SerializeField]
        private SignalData[] data;
    }
}