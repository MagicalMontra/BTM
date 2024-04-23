#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace HotPlay
{
    public static class EditorUtils
    {
        [MenuItem("HotPlay/Reset Player prefs")]
        public static void ResetPlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}
#endif