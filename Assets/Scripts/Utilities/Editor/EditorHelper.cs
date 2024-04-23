#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace HotPlay
{
    public class EditorHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="serializedObject">The object for editing properties</param>
        /// <param name="unityPropertiesName">the name of the unity properties</param>
        /// <returns></returns>
        public static List<SerializedProperty> GetNonSerializedProperty(SerializedObject serializedObject, HashSet<string> unityPropertiesName)
        {
            var iterator = serializedObject.GetIterator();
            List<SerializedProperty> nonUnityProperties = new List<SerializedProperty>();
            iterator.Next(true);
            while (iterator.Next(false))
            {
                if (!unityPropertiesName.Contains(iterator.name))
                {
                    nonUnityProperties.Add(serializedObject.FindProperty(iterator.name));
                }
            }
            return nonUnityProperties;
        }

        /// <summary>
        /// Display all non Unity's properties.
        /// </summary>
        /// <param name="nonUnityProperties"></param>
        public static void PropertyField(List<SerializedProperty> nonUnityProperties)
        {
            foreach (var property in nonUnityProperties)
            {
                EditorGUILayout.PropertyField(property, true);
            }
        }
    }
}
#endif