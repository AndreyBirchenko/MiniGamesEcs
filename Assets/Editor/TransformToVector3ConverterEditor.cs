using UnityEditor;

using UnityEngine;

namespace Utility
{
    [CustomEditor(typeof(TransformToVector3Converter))]
    public class TransformToVector3ConverterEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var script = (TransformToVector3Converter) target;

            if (GUILayout.Button("Convert"))
            {
                script.Convert();
            }
        }
    }
}