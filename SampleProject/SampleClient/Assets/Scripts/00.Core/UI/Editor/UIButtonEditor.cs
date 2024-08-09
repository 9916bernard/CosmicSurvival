using UnityEngine;
using UnityEngine.UI;

namespace UnityEditor.UI
{
    [CustomEditor(typeof(UIButton), true)]
    [CanEditMultipleObjects]
    /// <summary>
    /// Custom Editor for the Text Component.
    /// Extend this class to write a custom editor for a component derived from Text.
    /// </summary>
    public class UIButtonEditor : ButtonEditor
    {
        SerializedProperty _SfxType;

        protected override void OnEnable()
        {
            base.OnEnable();

            _SfxType = serializedObject.FindProperty("_SfxType");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(_SfxType);

            serializedObject.ApplyModifiedProperties();

            base.OnInspectorGUI();

            UIButton generator = (UIButton)target;
            if (GUILayout.Button("Set Property Button"))
            {
                generator.OnClick_SetProperty();
            }
        }
    }
}
