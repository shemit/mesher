using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LineStream))]
public class LineStreamEditor : Editor {

    private void OnSceneGUI()
    {
        LineStream line = target as LineStream;
        Transform handleTransform = line.transform;
        Quaternion handleRotation = handleTransform.rotation;

        for (int i = 0; i < line.points.Count; i++)
        {
            Vector3 p0 = handleTransform.TransformPoint(line.points[i]);
            Vector3 p1 = handleTransform.TransformPoint(line.points[(i + 1) % line.points.Count]);

            Handles.color = Color.white;
            Handles.DrawLine(p0, p1);
            Handles.DoPositionHandle(p0, handleRotation);
            Handles.DoPositionHandle(p1, handleRotation);

            
            EditorGUI.BeginChangeCheck();
            p0 = Handles.DoPositionHandle(p0, handleRotation);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(line, "Move Point");
                EditorUtility.SetDirty(line);
                line.points[i] = handleTransform.InverseTransformPoint(p0);
            }
            EditorGUI.BeginChangeCheck();
            
        }

    }

}
