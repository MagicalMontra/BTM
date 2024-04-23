#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sirenix.Utilities;
using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HotPlay.PecanUI.Skin
{

    [CustomEditor(typeof(PecanSkinConfigs))]
    public class PecanSkinConfigsEditor : UnityEditor.Editor
    {
        private bool isDebug;

        private List<string> tmpKeys = new List<string>();
        private List<string> imageKeys = new List<string>();

        private Object root;
        private Scene previewScene;
        private Scene previousScene;
        private PecanSkinConfigs configs;
        private GameObject[] previewingObjects;
        private Dictionary<string, bool> previewTmpFlag;
        private Dictionary<string, bool> previewImageFlag;
        private Dictionary<string, bool> tmpEditFlag;
        private Dictionary<string, bool> imageEditFlag;
        private Dictionary<string, string> tmpEditKey;
        private Dictionary<string, string> imageEditKey;
        private Dictionary<string, TMPSkinData> tmpData;
        private Dictionary<string, ImageSkinData> imageData;

        private void OnEnable()
        {
            configs = target as PecanSkinConfigs;
            previewingObjects = null;
            tmpKeys = GetLabelKey();
            imageKeys = GetImageKey();
            LoadData();
        }

        public override void OnInspectorGUI()
        {
            isDebug = EditorGUILayout.Toggle("Debug Mode", isDebug);

            if (isDebug)
            {
                base.OnInspectorGUI();
                return;
            }
            EditorGUILayout.BeginVertical("Box");
            
            DrawCreateTab();

            GUILayout.Label("Search for existed keys");
            EditorGUI.BeginChangeCheck();
            configs.searchKeyword = EditorGUILayout.TextField(configs.searchKeyword, EditorStyles.toolbarSearchField);

            if (EditorGUI.EndChangeCheck() && configs.searchKeyword.Length >= 3)
            {
                LoadData();
            }
            
            if (tmpData != null && tmpData.Count > 0 && configs.searchKeyword.Length >= 3)
            {
                foreach (var data in tmpData)
                {
                    DrawElement(data);
                }
            }
            
            if (imageData != null && imageData.Count > 0 && configs.searchKeyword.Length >= 3)
            {
                foreach (var data in imageData)
                {
                    DrawElement(data);
                }
            }

            EditorGUILayout.EndVertical();
        }

        private void DrawCreateTab()
        {
            EditorGUILayout.BeginVertical("Box");
            if (GUILayout.Button("Apply"))
            {
                ApplySkin();
            }
            
            EditorGUILayout.BeginHorizontal("Button");
            GUILayout.Label("Create New Skin");
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal("Box");
            GUILayout.Label("Select: ");
            EditorGUILayout.SelectableLabel(configs.selectedKey, EditorStyles.toolbarTextField, GUILayout.Height(EditorGUIUtility.singleLineHeight));
            EditorGUILayout.EndHorizontal();

            if (tmpKeys  != null && tmpKeys.Count > 0 || configs.newTMPSkin != null)
            {
                if (tmpKeys.Exists(key => key == configs.selectedKey))
                {
                    if (configs.newTMPSkin == null)
                        configs.newTMPSkin = new TMPSkinData();
                    
                    configs.newTMPSkin.FontAsset = (TMP_FontAsset)EditorGUILayout.ObjectField(configs.newTMPSkin.FontAsset, typeof(TMP_FontAsset), false);
                    configs.newTMPSkin.FontColor = EditorGUILayout.ColorField(configs.newTMPSkin.FontColor);


                    if (configs.newTMPSkin.FontAsset != null)
                    {
                        configs.newTMPSkin.PresetMaterial = (Material)EditorGUILayout.ObjectField(configs.newTMPSkin.PresetMaterial, typeof(Material), false);
                        
                        if (configs.newTMPSkin.PresetMaterial == null)
                            EditorGUILayout.HelpBox("Material is missing or name not match with font asset. Fallback to use default if not support.", MessageType.Warning);
                        else
                        {
                            if (GUILayout.Button("Create"))
                            {
                                configs.TMPSkin.Add(configs.selectedKey, configs.newTMPSkin);
                                configs.selectedKey = "";
                                configs.newTMPSkin = null;
                                EditorUtility.SetDirty(configs);
                                serializedObject.ApplyModifiedProperties();
                                AssetDatabase.Refresh();
                            }
                        }
                        
                    }
                    else
                        EditorGUILayout.HelpBox("Font asset is missing", MessageType.Error);
                    
                    if (GUILayout.Button("Cancel"))
                    {
                        configs.selectedKey = "";
                        configs.newTMPSkin = null;
                        EditorUtility.SetDirty(configs);
                        serializedObject.ApplyModifiedProperties();
                        AssetDatabase.Refresh();
                    }
                }
            }
            
            if (imageKeys  != null && imageKeys.Count > 0 || configs.newImageSkin != null)
            {
                if (imageKeys.Exists(key => key == configs.selectedKey))
                {
                    if (configs.newImageSkin == null)
                        configs.newImageSkin = new ImageSkinData();
                    
                    configs.newImageSkin.Color = EditorGUILayout.ColorField(configs.newImageSkin.Color);
                    if (GUILayout.Button("Create"))
                    {
                        configs.ImageSkin.Add(configs.selectedKey, configs.newImageSkin);
                        configs.selectedKey = "";
                        configs.newImageSkin = null;
                        EditorUtility.SetDirty(configs);
                        serializedObject.ApplyModifiedProperties();
                        AssetDatabase.Refresh();
                    }
                    
                    if (GUILayout.Button("Cancel"))
                    {
                        configs.selectedKey = "";
                        configs.newImageSkin = null;
                        EditorUtility.SetDirty(configs);
                        serializedObject.ApplyModifiedProperties();
                        AssetDatabase.Refresh();
                    }
                }
            }
            
            GUILayout.Label("Search for available keys");
            EditorGUI.BeginChangeCheck();
            configs.availableKeySearch = EditorGUILayout.TextField(configs.availableKeySearch, EditorStyles.toolbarSearchField);
            
            if (EditorGUI.EndChangeCheck() && configs.availableKeySearch.Length >= 3)
            {
                tmpKeys = GetLabelKey();
                imageKeys = GetImageKey();
            }
            
            if (tmpKeys != null && tmpKeys.Count > 0 && configs.availableKeySearch.Length >= 3)
                DrawAvailableKey(tmpKeys);
            
            if (imageKeys != null && imageKeys.Count > 0 && configs.availableKeySearch.Length >= 3)
                DrawAvailableKey(imageKeys);
            
            EditorGUILayout.EndVertical();
        }

        private void DrawAvailableKey(List<string> availableKeys)
        {
            for (int i = 0; i < availableKeys.Count; i++)
            {
                if (!availableKeys[i].ToLower().Contains(configs.availableKeySearch.ToLower()))
                    continue;
                
                EditorGUILayout.BeginHorizontal("Box");
                GUILayout.Label(availableKeys[i]);
                var selectKey = GUILayout.Button("Select");

                if (selectKey)
                {
                    configs.newTMPSkin = null;
                    configs.newImageSkin = null;
                    configs.selectedKey = availableKeys[i];
                }
                
                EditorGUILayout.EndHorizontal();
            }
        }

        private void DrawElement(KeyValuePair<string, TMPSkinData> data)
        {
            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.BeginHorizontal("Button");

            if (!tmpEditFlag[data.Key])
            {
                GUILayout.Label(tmpEditKey[data.Key]);
                tmpEditFlag[data.Key] = GUILayout.Button("Edit");
                EditorGUI.BeginChangeCheck();
            }
            else
            {
                tmpEditKey[data.Key] = EditorGUILayout.TextField(tmpEditKey[data.Key]);
                EditorGUI.BeginChangeCheck();
                tmpEditFlag[data.Key] = !GUILayout.Button("Accept");
            }
            
            EditorGUILayout.EndHorizontal();
            data.Value.FontAsset = (TMP_FontAsset)EditorGUILayout.ObjectField(data.Value.FontAsset, typeof(TMP_FontAsset), false);
            data.Value.FontColor = EditorGUILayout.ColorField(data.Value.FontColor);
            
            if (data.Value.FontAsset != null)
                data.Value.PresetMaterial = (Material)EditorGUILayout.ObjectField(data.Value.PresetMaterial, typeof(Material), false);

            if (EditorGUI.EndChangeCheck())
            {
                configs.TMPSkin.Edit(data.Key, data.Value, tmpEditKey[data.Key]);
                EditorUtility.SetDirty(configs);
                serializedObject.ApplyModifiedProperties();
                LoadData();
                Repaint();
                AssetDatabase.Refresh();
            }
            
            DrawPreviewElement(previewTmpFlag, data.Key);
            EditorGUILayout.Space(10);

            if (GUILayout.Button("Delete"))
            {
                configs.TMPSkin.Remove(data.Key);
                EditorUtility.SetDirty(configs);
                serializedObject.ApplyModifiedProperties();
                LoadData();
                Repaint();
                AssetDatabase.Refresh();
            }
            
            EditorGUILayout.Space(10);
            EditorGUILayout.EndVertical();
        }
        
        private void DrawElement(KeyValuePair<string, ImageSkinData> data)
        {
            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.BeginHorizontal("Button");

            if (!imageEditFlag[data.Key])
            {
                GUILayout.Label(imageEditKey[data.Key]);
                imageEditFlag[data.Key] = GUILayout.Button("Edit");
                EditorGUI.BeginChangeCheck();
            }
            else
            {
                imageEditKey[data.Key] = EditorGUILayout.TextField(imageEditKey[data.Key]);
                EditorGUI.BeginChangeCheck();
                imageEditFlag[data.Key] = !GUILayout.Button("Accept");
            }
            
            EditorGUILayout.EndHorizontal();

            data.Value.Color = EditorGUILayout.ColorField(data.Value.Color);
            
            if (EditorGUI.EndChangeCheck())
            {
                configs.ImageSkin.Edit(data.Key, data.Value, imageEditKey[data.Key]);
                EditorUtility.SetDirty(configs);
                serializedObject.ApplyModifiedProperties();
                LoadData();
                Repaint();
                AssetDatabase.Refresh();
            }
            DrawPreviewElement(previewImageFlag, data.Key);
            EditorGUILayout.Space(10);
            
            if (GUILayout.Button("Delete"))
            {
                configs.ImageSkin.Remove(data.Key);
                EditorUtility.SetDirty(configs);
                serializedObject.ApplyModifiedProperties();
                LoadData();
                Repaint();
                AssetDatabase.Refresh();
            }
            
            EditorGUILayout.Space(10);
            EditorGUILayout.EndVertical();
        }

        private void DrawPreviewElement(Dictionary<string, bool> previewDict, string key)
        {
            if (previewDict[key] && previewScene.isLoaded)
            {
                previewingObjects ??= PrefabStageUtility.GetCurrentPrefabStage().prefabContentsRoot.GetComponentsInChildren<BaseSkinHandler>()
                    .Where(c => c.SkinKey.Contains(key)).Select(c => c.gameObject).ToArray();

                if (GUILayout.Button("Cancel Preview Mode"))
                {
                    previewDict[key] = false;
                    PrefabStageUtility.GetCurrentPrefabStage().ClearDirtiness();
                    EditorSceneManager.OpenScene(previousScene.path);
                    Selection.activeObject = configs;
                    ActiveEditorTracker.sharedTracker.isLocked = false;
                    previewingObjects = null;
                }
                
                EditorGUILayout.BeginVertical("Button");
                GUILayout.Label(key + " will be affecting these objects");
                EditorGUILayout.EndVertical();

                if (previewingObjects != null)
                {
                    foreach (var go in previewingObjects)
                    {
                        EditorGUILayout.ObjectField(go, typeof(GameObject), true);
                    }
                }
            }
            else
            {
                if (GUILayout.Button("Locate in prefab"))
                {
                    previousScene = EditorSceneManager.GetActiveScene();
                    ActiveEditorTracker.sharedTracker.isLocked = true;

                    if (PrefabStageUtility.GetCurrentPrefabStage() == null ||
                        PrefabStageUtility.GetCurrentPrefabStage().prefabContentsRoot.name != configs.PecanServices.name)
                    {
                        previewScene = PrefabStageUtility.OpenPrefab(AssetDatabase.GetAssetPath(configs.PecanServices)).scene;
                    }

                    PrefabStageUtility.GetCurrentPrefabStage().ClearDirtiness();
                    previewDict[key] = true;
                }
            }
        }
        
        private void LoadData()
        {
            tmpData = configs.TMPSkin.ReadMany(configs.searchKeyword).ToDictionary(x => x.Key, x => x.Value);
            imageData = configs.ImageSkin.ReadMany(configs.searchKeyword).ToDictionary(x => x.Key, x => x.Value);
            tmpEditFlag = tmpData.ToDictionary(x => x.Key, x => false);
            imageEditFlag = imageData.ToDictionary(x => x.Key, x => false);
            tmpEditKey = tmpData.ToDictionary(x => x.Key, x => x.Key);
            imageEditKey = imageData.ToDictionary(x => x.Key, x => x.Key);
            previewTmpFlag = tmpData.ToDictionary(x => x.Key, x => false);
            previewImageFlag = imageData.ToDictionary(x => x.Key, x => false);
        }
        
        private static List<string> GetLabelKey()
        {
            List<string> defaultKey = File.ReadAllLines( $"{Application.dataPath}/PecanUI/PecanTMPSkinDefaultKey.txt").ToList();

            var prefabKey = GetAllTMPSkinHandlerKey();
            
            foreach (string key in prefabKey)
            {
                defaultKey.Add(key);
            }
            
            var characterTMPTileKey = GetCharacterTileTMPSkinKey();
            
            foreach (string key in characterTMPTileKey)
            {
                defaultKey.Add(key);
            }
            
            var themeTMPTileKey = GetThemeTileTMPSkinKey();
            
            foreach (string key in themeTMPTileKey)
            {
                defaultKey.Add(key);
            }
            
            var playerRankTMPKey = GetPlayerRankTMPKey();
            
            foreach (string key in playerRankTMPKey)
            {
                defaultKey.Add(key);
            }
            
            defaultKey = defaultKey.Distinct().ToList();
            
            return defaultKey;
        }

        private static List<string> GetImageKey()
        {
            List<string> defaultKey = File.ReadAllLines( $"{Application.dataPath}/PecanUI/PecanImageSkinDefaultKey.txt").ToList();

            var prefabKey = GetAllImageSkinHandlerKey();
            
            foreach (string key in prefabKey)
            {
                defaultKey.Add(key);
            }
            
            var characterImageTileKey = GetCharacterTileImageSkinKey();
            
            foreach (string key in characterImageTileKey)
            {
                defaultKey.Add(key);
            }

            var themeImageTileKey = GetThemeTileImageSkinKey();
            
            foreach (string key in themeImageTileKey)
            {
                defaultKey.Add(key);
            }
            
            var playerRankImageKey = GetPlayerRankImageKey();
            
            foreach (string key in playerRankImageKey)
            {
                defaultKey.Add(key);
            }

            defaultKey = defaultKey.Distinct().ToList();
            
            return defaultKey;
        }

        private void ApplySkin()
        {
            AssetDatabase.Refresh();
            EditorUtility.SetDirty(configs);
            serializedObject.ApplyModifiedProperties();
            
            var pecanPrefab = PrefabUtility.LoadPrefabContents(AssetDatabase.GetAssetPath(configs.PecanServices));

            if (pecanPrefab == null)
                return;
                
            pecanPrefab.GetComponentsInChildren<BaseSkinHandler>(true).ForEach(handler => handler.UpdateSkin(configs));

            PrefabUtility.SaveAsPrefabAsset(pecanPrefab, AssetDatabase.GetAssetPath(configs.PecanServices));
            PrefabUtility.UnloadPrefabContents(pecanPrefab);
        }


        private static IEnumerable GetCharacterTileImageSkinKey()
        {
            var root = "Assets/PecanUI/Prefabs/UI/Shop/ShopItemCha.prefab";
            var enumerable = AssetDatabase.LoadAssetAtPath<GameObject>(root)
                .GetComponentsInChildren<SkinImageHandler>(true)
                .Select(component => component.SkinKey);
            return enumerable;
        }
        
        private static IEnumerable GetCharacterTileTMPSkinKey()
        {
            var root = "Assets/PecanUI/Prefabs/UI/Shop/ShopItemCha.prefab";
            var enumerable = AssetDatabase.LoadAssetAtPath<GameObject>(root)
                .GetComponentsInChildren<SkinTMPHandler>(true)
                .Select(component => component.SkinKey);
            return enumerable;
        }
        
        private static IEnumerable GetThemeTileImageSkinKey()
        {
            var root = "Assets/PecanUI/Prefabs/UI/Shop/ShopItemTheme.prefab";
            var enumerable = AssetDatabase.LoadAssetAtPath<GameObject>(root)
                .GetComponentsInChildren<SkinImageHandler>(true)
                .Select(component => component.SkinKey);
            return enumerable;
        }
        
        private static IEnumerable GetThemeTileTMPSkinKey()
        {
            var root = "Assets/PecanUI/Prefabs/UI/Shop/ShopItemTheme.prefab";
            var enumerable = AssetDatabase.LoadAssetAtPath<GameObject>(root)
                .GetComponentsInChildren<SkinTMPHandler>(true)
                .Select(component => component.SkinKey);
            return enumerable;
        }

        private static IEnumerable GetPlayerRankImageKey()
        {
            var root = "Assets/PecanUI/Prefabs/UI/Leaderboard/LeaderboardListRow.prefab";
            var enumerable = AssetDatabase.LoadAssetAtPath<GameObject>(root)
                .GetComponentsInChildren<SkinImageHandler>(true)
                .Select(component => component.SkinKey);
            return enumerable;
        }
        
        private static IEnumerable GetPlayerRankTMPKey()
        {
            var root = "Assets/PecanUI/Prefabs/UI/Leaderboard/LeaderboardListRow.prefab";
            var enumerable = AssetDatabase.LoadAssetAtPath<GameObject>(root)
                .GetComponentsInChildren<SkinTMPHandler>(true)
                .Select(component => component.SkinKey);
            return enumerable;
        }
        
        private static IEnumerable GetAllTMPSkinHandlerKey()
        {
            var root = "Assets/PecanUI/Prefabs/PecanUIManager.prefab";
            var enumerable = AssetDatabase.LoadAssetAtPath<PecanServices>(root)
                .GetComponentsInChildren<SkinTMPHandler>(true)
                .Select(component => component.SkinKey);
            return enumerable;
        }
        
        private static IEnumerable GetAllImageSkinHandlerKey()
        {
            var root = "Assets/PecanUI/Prefabs/PecanUIManager.prefab";
            var enumerable = AssetDatabase.LoadAssetAtPath<PecanServices>(root)
                .GetComponentsInChildren<SkinImageHandler>(true)
                .Select(component => component.SkinKey);
            return enumerable;
        }
    }
}
#endif