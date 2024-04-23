#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace HotPlay.PecanUI.Skin
{
    [CustomEditor(typeof(OldToNewSkinConfigConverter))]
    public class OldToNewSkinConfigConverterEditor : UnityEditor.Editor
    {
        private OldToNewSkinConfigConverter _converter;
        private void OnEnable()
        {
            _converter = target as OldToNewSkinConfigConverter;
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.BeginVertical("Box");

            GUILayout.Label("Old Config File");
            _converter.oldSkinConfig = (SkinConfigs)EditorGUILayout.ObjectField(_converter.oldSkinConfig, typeof(SkinConfigs), false);
            GUILayout.Label("New Config File");
            if (_converter.skinConfig == null)
                EditorGUILayout.HelpBox("Duplicating default new Skin config into a new file and put on this field is recommended.", MessageType.Warning);
            
            _converter.skinConfig = (PecanSkinConfigs)EditorGUILayout.ObjectField(_converter.skinConfig, typeof(PecanSkinConfigs), false);

            EditorGUILayout.BeginFadeGroup((_converter.oldSkinConfig == null && _converter.skinConfig == null) ? 0 : 1);

            if (GUILayout.Button("Convert"))
            {
                _converter.Convert();
                serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(_converter.skinConfig);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
            
            EditorGUILayout.EndFadeGroup();
            
            EditorGUILayout.EndVertical();
        }
    }
}
#endif