using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System.Text;
using HotPlay.PecanUI.Leaderboard;

namespace HotPlay.EditorTools
{
    public class CSVToLeaderboardDataEditorWindow : EditorWindow
    {
        private TextAsset leaderboardCSVData = null;
        private bool useAvatarSprites = false;
        private string avatarSpritesPath = "";
        private string outputPath = "";

        [MenuItem("HotPlay/CSV to LeaderboardData")]
        private static void ShowWindow()
        {
            GetWindow<CSVToLeaderboardDataEditorWindow>("CSV to LeaderboardData");
        }

        private void OnGUI()
        {
            EditorGUIUtility.labelWidth = 250;

            leaderboardCSVData = (TextAsset)EditorGUILayout.ObjectField("CSV Data", leaderboardCSVData, typeof(TextAsset), false);
            useAvatarSprites = EditorGUILayout.Toggle("Use Avatar Sprites", useAvatarSprites);
            if (useAvatarSprites)
            {
                avatarSpritesPath = EditorGUILayout.TextField("Avatar Sprites Path - Root: Resources/", avatarSpritesPath);
                EditorGUILayout.LabelField("Example: LeaderboardAvatars/");
                EditorGUILayout.Space();
            }

            outputPath = EditorGUILayout.TextField("Output Path - Root: <UnityProjectFolder>/", outputPath);
            EditorGUILayout.LabelField("Example: Assets/Resources/LeaderboardData/ (create the folder first)");
            EditorGUILayout.Space();
            if (GUILayout.Button("Generate LeaderboardData from CSV"))
            {
                string csvFullPath = GetFullPath(leaderboardCSVData);
                if(useAvatarSprites)
                {
                    GenerateLeaderboardDataWithAvatar(csvFullPath, avatarSpritesPath, outputPath);
                }
                else
                {
                    GenerateLeaderboardData(csvFullPath, outputPath);
                }
            }
        }

        private static void GenerateLeaderboardDataWithAvatar(string leaderboardCSVDataFullPath, string avatarSpritesRootPath, string leaderboardDataOutputRootPath)
        {
            string[] allLines = File.ReadAllLines(leaderboardCSVDataFullPath);
            Dictionary<string, Sprite> spriteDict = LoadAvatarSpriteDict(avatarSpritesRootPath);
            for (int i = 1; i < allLines.Length; i++)
            {
                string line = allLines[i];
                string[] splitData = line.Split(',');
                if (splitData.Length != 4)
                {
                    Debug.LogError($"{line} does not have 4 columns!");
                    return;
                }

                PlayerLeaderboardProfileData profileData = ScriptableObject.CreateInstance<PlayerLeaderboardProfileData>();
                Debug.Log("playerName: " + splitData[1]);
                Debug.Log("avatarSprite: " + splitData[2]);
                Debug.Log("score: " + splitData[3]);
                profileData.Setup(
                    playerName: splitData[1],
                    avatarSprite: spriteDict[splitData[2]],
                    score: int.Parse(splitData[3]),
                    isHumanPlayer: false
                );
                AssetDatabase.CreateAsset(profileData, $"{leaderboardDataOutputRootPath}{splitData[0]}.asset");
            }
            AssetDatabase.SaveAssets();
        }

        private static void GenerateLeaderboardData(string leaderboardCSVDataFullPath, string leaderboardDataOutputRootPath)
        {
            string[] allLines = File.ReadAllLines(leaderboardCSVDataFullPath);
            for (int i = 1; i < allLines.Length; i++)
            {
                string line = allLines[i];
                string[] splitData = line.Split(',');
                if (splitData.Length != 4)
                {
                    Debug.LogError($"{line} does not have 4 columns!");
                    return;
                }

                PlayerLeaderboardProfileData profileData = ScriptableObject.CreateInstance<PlayerLeaderboardProfileData>();
                Debug.Log("playerName: " + splitData[1]);
                Debug.Log("avatarSprite: " + splitData[2]);
                Debug.Log("score: " + splitData[3]);
                profileData.Setup(
                    playerName: splitData[1],
                    avatarSprite: null,
                    score: int.Parse(splitData[3]),
                    isHumanPlayer: false
                );
                AssetDatabase.CreateAsset(profileData, $"{leaderboardDataOutputRootPath}{splitData[0]}.asset");
            }
            AssetDatabase.SaveAssets();
        }

        private static Dictionary<string, Sprite> LoadAvatarSpriteDict(string avatarSpritesRootPath)
        {
            Dictionary<string, Sprite> spriteDict = new Dictionary<string, Sprite>();
            foreach (Texture2D texture in Resources.LoadAll<Texture2D>(avatarSpritesRootPath))
            {
                Debug.Log("texture name: " + texture.name);
                Sprite[] sprites = Resources.LoadAll<Sprite>(avatarSpritesRootPath + texture.name);
                Debug.Log("sprites count: " + sprites.Length);
                foreach (Sprite sprite in sprites)
                {
                    Debug.Log("Add sprite name: " + sprite.name + " to dict");
                    spriteDict.Add(sprite.name, sprite);
                }
            }
            return spriteDict;
        }

        private string GetFullPath(TextAsset leaderboardCSVData)
        {
            return Application.dataPath + RemoveFirstFoundText(AssetDatabase.GetAssetPath(leaderboardCSVData), "Assets");
        }

        private string RemoveFirstFoundText(string text, string searchText)
        {
            string[] splittedTexts = text.Split('/');
            StringBuilder outputText = new StringBuilder();
            bool foundSearchText = false;
            foreach (var item in splittedTexts)
            {
                if (item.Equals(searchText))
                {
                    foundSearchText = true;
                }
                else if (foundSearchText)
                {
                    outputText.Append('/');
                    outputText.Append(item);
                }
            }
            return outputText.ToString();
        }
    }
}