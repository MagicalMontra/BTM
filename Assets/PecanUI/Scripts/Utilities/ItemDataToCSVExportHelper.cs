#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace HotPlay.PecanUI
{
    public static class ItemDataToCSVExportHelper
    {
        private static string seperator = ",";
        private static string itemLocalizationCategory = PecanServices.Instance.Configs.ShopItemCategory;

        public static void GetCSVToClipboard(params string[] itemNames)
        {
            EditorGUIUtility.systemCopyBuffer = GetString(itemNames);
            Debug.Log("Finished!, paste your new csv data in any text editor");
        }

        private static string GetString(params string[] itemNames)
        {
            return string.Join("", itemNames);
        }

        private static string Format(string itemName)
        {
            return $"{itemLocalizationCategory}/{itemName}{seperator}Text{seperator}{seperator}{itemName}\n";
        }
    }
}
#endif