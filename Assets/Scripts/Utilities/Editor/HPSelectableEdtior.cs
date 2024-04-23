#if UNITY_EDITOR
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

namespace HotPlay
{
    /// <summary>
    /// Base class for every class that inherit from UI
    /// This class will get properties name from child class and exclude it from render in Inspector
    /// </summary>
    public abstract class HPSelectableEditor : SelectableEditor
    {
        /// <summary>
        /// List of non unity properties. This allow we to serialize anything that in child class of TAButton
        /// </summary>
        protected List<SerializedProperty> nonUnityProperties = new List<SerializedProperty>();

        /// <summary>
        /// List of Unity's Properties to exclude from draw
        /// </summary>
        protected abstract HashSet<string> unityPropertiesName { get; }

        /// <summary>
        /// Display all non Unity's property.
        /// </summary>
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorHelper.PropertyField(nonUnityProperties);
            serializedObject.ApplyModifiedProperties();
        }

        /// <summary>
        /// Find every property that is not Unity's.
        /// </summary>
        protected override void OnEnable()
        {
            base.OnEnable();
            nonUnityProperties = EditorHelper.GetNonSerializedProperty(serializedObject, unityPropertiesName);

        }

        /// <summary>
        /// Use this function to print all properties
        /// </summary>
        protected void PrintAllProperties()
        {
            StringBuilder allPropertiesText = new StringBuilder();

            var iterate = serializedObject.GetIterator();
            iterate.Next(true);

            //This will iterate through every SerializedProperty of object
            while (iterate.Next(false))
            {
                allPropertiesText.AppendLine(iterate.name);
            }

            Debug.Log(allPropertiesText.ToString());
        }
    }
}
#endif