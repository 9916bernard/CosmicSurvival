using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UI;


namespace UnityEditor.UI
{
    [CustomEditor(typeof(UITextMeshProUGUI), true)]
    [CanEditMultipleObjects]
    /// <summary>
    /// Custom Editor for the Text Component.
    /// Extend this class to write a custom editor for a component derived from Text.
    /// </summary>
    public class UITextMeshProGUIEditor : TMP_EditorPanelUI
    {
        SerializedProperty _TextID;

        protected override void OnEnable()
        {
            base.OnEnable();

            _TextID = serializedObject.FindProperty("_TextID");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(_TextID);

            serializedObject.ApplyModifiedProperties();

            UITextMeshProUGUI generator = (UITextMeshProUGUI)target;
            if (GUILayout.Button("Set Property Text"))
            {
                generator.OnClick_SetProperty();
            }

            base.OnInspectorGUI();
        }
    }
}
