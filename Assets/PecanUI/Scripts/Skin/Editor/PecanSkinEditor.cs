#if UNITY_EDITOR
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace HotPlay.PecanUI.Skin.Editor
{
    public class PecanSkinEditor : OdinEditorWindow
    {
        [MenuItem("Pecan UI/Skin Editor")]
        private static void OpenWindow()
        {
            GetWindow<PecanSkinEditor>().Show();
        }

        [Required(errorMessage: "Missing skin config file")]
        [SerializeField]
        private SkinConfigs skinConfigs;

        [AssetList(Path = "/PecanUI/Prefabs/UI/")]
        [SerializeField]
        private List<GameObject> pecanPrefabs;

        [AssetSelector(IsUniqueList = false)]
        [Tooltip("Additional prefabs for non Pecan")]
        [SerializeField]
        private List<GameObject> additionalPrefabs;

        [HorizontalGroup()]
        [SerializeField]
        private bool ignoreNestedPrefab;

        [HorizontalGroup()]
        [Button("Apply")]
        public void Apply()
        {
            if (skinConfigs == null)
            {
                Debug.LogError($"Missing skin config file");
                return;
            }

            AssetDatabase.SaveAssetIfDirty(skinConfigs);
            skinConfigs.UpdateLookupData();

            foreach (var element in pecanPrefabs)
            {
                TryApplySkin(element);
                SetupAllChilds(element.transform, TryApplySkin);
                var parent = PrefabUtility.GetCorrespondingObjectFromSource(element);
                if (parent == null)
                {
                    EditorUtility.SetDirty(element);
                    PrefabUtility.RecordPrefabInstancePropertyModifications(element);
                    PrefabUtility.SavePrefabAsset(element);
                }
            }

            foreach (var additional in additionalPrefabs)
            {
                TryApplySkin(additional);
                SetupAllChilds(additional.transform, TryApplySkin);
                var parent = PrefabUtility.GetCorrespondingObjectFromSource(additional);
                if (parent == null)
                {
                    EditorUtility.SetDirty(additional);
                    PrefabUtility.RecordPrefabInstancePropertyModifications(additional);
                    PrefabUtility.SavePrefabAsset(additional);
                }
            }

            AssetDatabase.SaveAssets();
            skinConfigs.CleanUpLookupData();
        }

        [HorizontalGroup()]
        [Button("Load")]
        public void Load()
        {
            if (skinConfigs == null)
            {
                Debug.LogError($"Missing skin config file");
                return;
            }

            skinConfigs.UpdateLookupData();

            foreach (var element in pecanPrefabs)
            {
                TryLoadSkin(element);
                SetupAllChilds(element.transform, TryLoadSkin);
            }

            skinConfigs.CleanUpLookupData();
            EditorUtility.SetDirty(skinConfigs);
            AssetDatabase.SaveAssetIfDirty(skinConfigs);
        }

        public void SetupAllChilds(Transform transform, System.Action<GameObject> action)
        {
            foreach (Transform childTransform in transform.transform)
            {
                var isPrefab = PrefabUtility.IsAnyPrefabInstanceRoot(childTransform.gameObject);
                var parent = PrefabUtility.GetCorrespondingObjectFromSource(childTransform.gameObject);

                if (ignoreNestedPrefab && isPrefab)
                {
                    continue;
                }
                
                action?.Invoke(childTransform.gameObject);
                SetupAllChilds(childTransform, action);
            }
        }

        public void TryApplySkin(GameObject go)
        {
            var skinHandler = go.gameObject.GetComponent<BaseSkinHandler>();

            if (skinHandler != null)
            {
                skinHandler.UpdateSkin(skinConfigs);
                EditorUtility.SetDirty(skinHandler.gameObject);
                Debug.Log($"Apply [{skinHandler.name}]: {go.gameObject.name} of {go.gameObject.transform.root.name}");
            }
        }

        public void TryLoadSkin(GameObject go)
        {
            var skinHandler = go.gameObject.GetComponent<BaseSkinHandler>();

            if (skinHandler != null)
            {
                var skinData = skinConfigs.GetSkinData<SkinData>(skinHandler.SkinKey);
                skinHandler.LoadSkin(skinData);
                Debug.Log($"Load [{skinHandler.name}]: {go.gameObject.name} of {go.gameObject.transform.root.name}");
            }
        }
    }
}
#endif