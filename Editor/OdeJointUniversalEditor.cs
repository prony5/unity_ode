using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace UnityODE
{
    [CustomEditor(typeof(OdeJointUniversal))]
    [CanEditMultipleObjects]
    public class OdeJointUniversalEditor : OdeJointEditor
    {
        private OdeJointUniversal Script { get { return target as OdeJointUniversal; } }

        void OnSceneGUI()
        {
            if (Script.axis1 == Vector3.zero) 
                return;

            if (!Application.isPlaying)
                Script.defaultRotation = Script.transform.rotation;

            Vector3 anchor = Script.transform.position + Script.transform.rotation * Script.anchor;

            float size = 0.5f;
            var mf = Script.GetComponent<MeshFilter>();
            if (mf != null && mf.sharedMesh != null)
                size = mf.sharedMesh.bounds.size.magnitude;
            if (size > 1) size = 1;

            float sizeSphere = 0.15f * size;
            if (sizeSphere > 0.05f) sizeSphere = 0.05f;

            Quaternion rotation = Application.isPlaying && Script.connectedBody != null ? Script.defaultRotation * Script.connectedBody.transform.rotation * Quaternion.Inverse(Script.defaultRotationConnectedBody) : Script.defaultRotation;
            DrawLimits(anchor, Script.axis1, rotation, Script.zeroAxisDisplayOffset, OdeEdittorDefault.color, Script.limits1, "Axis1");
            DrawLimits(anchor, Script.axis2, rotation, Script.zeroAxisDisplayOffset2, OdeEdittorDefault.color * 0.5f, Script.limits2, "Axis2");

            #region Draw rotate menu
            Handles.color = Color.white;

            // Quick Editing Tools
            Handles.BeginGUI();
            GUILayout.BeginArea(new Rect(10, Screen.height - 120, 200, 70), "Joint Universal", "Window");

            // Rotating display
            if (GUILayout.Button("Rotate 90 degrees. Axis 1"))
            {
                if (!Application.isPlaying) Undo.RecordObject(Script, "Rotate Display. Axis 1");
                Script.zeroAxisDisplayOffset += 90;
                if (Script.zeroAxisDisplayOffset >= 360) Script.zeroAxisDisplayOffset = 0;
            }

            // Rotating display
            if (GUILayout.Button("Rotate 90 degrees. Axis 2"))
            {
                if (!Application.isPlaying) Undo.RecordObject(Script, "Rotate Display. Axis 2");
                Script.zeroAxisDisplayOffset2 += 90;
                if (Script.zeroAxisDisplayOffset2 >= 360) Script.zeroAxisDisplayOffset2 = 0;
            }

            GUILayout.EndArea();
            Handles.EndGUI();
            #endregion
        }

    }
}
