using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace UnityODE
{
    [CustomEditor(typeof(OdeJointWheel))]
    [CanEditMultipleObjects]
    public class OdeJointWheelEditor : OdeJointEditor
    {
        private OdeJointWheel Script { get { return target as OdeJointWheel; } }

        void OnSceneGUI()
        {
            if (Script.axisWheelRotation == Vector3.zero || Script.showLimits == false)
                return;

            Handles.color = OdeEdittorDefault.color;

            var rotation = Script.transform.rotation;
            var anchor = Script.transform.position + rotation * Script.anchor;
            if (!Application.isPlaying)
                Script.anchorWheel = Script.connectedBody == null ? Vector3.zero : Script.anchor + Vector3.Scale(Quaternion.Inverse(rotation) * (Script.connectedBody.transform.position - anchor), Script.axisWheel);

            //Draw Anchor
            Handles.SphereHandleCap(0, anchor, Quaternion.identity, 0.025f, EventType.Repaint);
            DrawText(anchor, "Anchor", OdeEdittorDefault.color);

            //Draw WheelRotation
            var connectedPosition = Script.transform.position + rotation * Vector3.Scale(Script.anchor, Vector3.one - Script.axisWheelRotation);
            Handles.DrawLine(anchor, connectedPosition);
            DrawLimits(connectedPosition, Script.axisWheelRotation, rotation, Script.zeroAxisDisplayOffset, OdeEdittorDefault.color, Script.limitsWheelRotation, "Axis Wheel Rotation");

            //Draw Wheel
            if (Script.connectedBody != null)
            {
                //auto size
                float size = 0.5f;
                var mf = Script.connectedBody.GetComponent<MeshFilter>();
                if (mf != null && mf.sharedMesh != null)
                    size = mf.sharedMesh.bounds.size.magnitude;
                if (size > 1) size = 1;
                float sizeSphere = 0.05f * size;
                if (sizeSphere > 0.025f) sizeSphere = 0.025f;

                var wheelRotation = rotation * Quaternion.AngleAxis(-Script.ValueWheelRotation, Script.axisWheelRotation);
                var anchorWheelPosition = anchor + wheelRotation * (Script.anchorWheel - Script.anchor);

                Handles.DrawLine(anchor, anchorWheelPosition);

                Handles.SphereHandleCap(0, anchorWheelPosition, Quaternion.identity, sizeSphere, EventType.Repaint);
                DrawText(anchorWheelPosition, "Anchor Wheel", OdeEdittorDefault.color);

                Vector3 cross = Vector3.Cross(Script.axisWheel, new Vector3(Script.axisWheel.y, Script.axisWheel.z, Script.axisWheel.x)).normalized;
                Handles.CircleHandleCap(0, anchorWheelPosition, wheelRotation * Quaternion.LookRotation(Script.axisWheel, cross), size * 0.5f, EventType.Repaint);

                Handles.color = OdeEdittorDefault.color * 0.25f;
                Handles.DrawSolidArc(anchorWheelPosition, wheelRotation * Script.axisWheel, wheelRotation * cross, 360, size * 0.5f);
            }

            #region Draw rotate menu
            Handles.color = Color.white;

            // Quick Editing Tools
            Handles.BeginGUI();
            GUILayout.BeginArea(new Rect(10, Screen.height - 95, 150, 45), "Joint Universal", "Window");

            // Rotating display
            if (GUILayout.Button("Rotate 90 degrees"))
            {
                if (!Application.isPlaying) Undo.RecordObject(Script, "Rotate Display.");
                Script.zeroAxisDisplayOffset += 90;
                if (Script.zeroAxisDisplayOffset >= 360) Script.zeroAxisDisplayOffset = 0;
            }
            GUILayout.EndArea();
            Handles.EndGUI();
            #endregion

        }

    }
}