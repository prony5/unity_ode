using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace UnityODE
{
    [CustomEditor(typeof(OdeJointHinge))]
    [CanEditMultipleObjects]
    public class OdeJointHingeEditor : OdeJointEditor
    {
        private OdeJointHinge Script { get { return target as OdeJointHinge; } }

        void OnSceneGUI()
        {
            if (Script.Axis == Vector3.zero || Script.JointLimits.visible == false) return;

            if (!Application.isPlaying)
                Script.defaultLocalRotation = Script.transform.localRotation;

            Vector3 anchor = Script.transform.position;
            if (Application.isPlaying)
                anchor += Direction(OdeUtils.RotateAroundPivot(Script.Anchor, Vector3.zero, Quaternion.Inverse(Script.defaultLocalRotation) * Script.transform.localRotation), false);
            else
                anchor += Direction(Script.Anchor);

            Vector3 cross = Direction(Quaternion.AngleAxis(Script.zeroAxisDisplayOffset, Script.Axis) * Vector3.Cross(Script.Axis, new Vector3(Script.Axis.y, Script.Axis.z, Script.Axis.x)).normalized);
            Vector3 Axis = Direction(Script.Axis.normalized);

            Quaternion offsRot = Application.isPlaying ? Script.connectedBody == null ? Quaternion.identity : Script.connectedBody.transform.rotation * Quaternion.Inverse(Script.defaultRotationConnectedBody) : Quaternion.identity;

            float size = 0.5f;
            var mf = Script.GetComponent<MeshFilter>();
            if (mf != null && mf.sharedMesh != null)
                size = mf.sharedMesh.bounds.size.magnitude;
            if (size > 1) size = 1;

            float sizeSphere = 0.15f * size;
            if (sizeSphere > 0.05f) sizeSphere = 0.05f;

            Handles.color = OdeEdittorDefault.color;
            // Rotation circle
            Handles.CircleHandleCap(0, anchor, offsRot * Quaternion.LookRotation(Axis, cross), size, EventType.Repaint);

            // Axis vector
            DrawArrow(anchor, offsRot * (Axis * size), OdeEdittorDefault.color, "Axis", sizeSphere * 0.1f);

            // Zero rotation vector
            DrawArrow(anchor, offsRot * (cross * size), OdeEdittorDefault.color, " 0", sizeSphere);

            // Arcs for the rotation limit
            Handles.color = new Color(OdeEdittorDefault.color.r, OdeEdittorDefault.color.g, OdeEdittorDefault.color.b, 0.15f);
            Handles.DrawSolidArc(anchor, offsRot * Axis, offsRot * cross, Script.JointLimits.min, size);
            Handles.DrawSolidArc(anchor, offsRot * Axis, offsRot * cross, Script.JointLimits.max, size);

            Handles.color = OdeEdittorDefault.color;
            // Handles for adjusting rotation limits in the scene
            Quaternion minRotation = Quaternion.AngleAxis(Script.JointLimits.min, Axis);
            Handles.DrawLine(anchor, anchor + offsRot * (minRotation * cross * size));

            Quaternion maxRotation = Quaternion.AngleAxis(Script.JointLimits.max, Axis);
            Handles.DrawLine(anchor, anchor + offsRot * (maxRotation * cross * size));

            // Undoable scene handles
            float min = Script.JointLimits.min;
            min = DrawLimitHandle(min, anchor + offsRot * (minRotation * cross * size), Quaternion.identity, size, "Min", -10, sizeSphere);
            if (min != Script.JointLimits.min)
            {
                if (!Application.isPlaying) Undo.RecordObject(Script, "Min Limit");
                Script.JointLimits.min = min;
            }

            float max = Script.JointLimits.max;
            max = DrawLimitHandle(max, anchor + offsRot * (maxRotation * cross * size), Quaternion.identity, size, "Max", 10, sizeSphere);
            if (max != Script.JointLimits.max)
            {
                if (!Application.isPlaying) Undo.RecordObject(Script, "Max Limit");
                Script.JointLimits.max = max;
            }

            #region Draw rotate menu
            Handles.color = Color.white;

            // Quick Editing Tools
            Handles.BeginGUI();
            GUILayout.BeginArea(new Rect(10, Screen.height - 100, 200, 50), "Joint Hinge", "Window");

            // Rotating display
            if (GUILayout.Button("Rotate display 90 degrees"))
            {
                if (!Application.isPlaying) Undo.RecordObject(Script, "Rotate Display");
                Script.zeroAxisDisplayOffset += 90;
                if (Script.zeroAxisDisplayOffset >= 360) Script.zeroAxisDisplayOffset = 0;
            }

            GUILayout.EndArea();
            Handles.EndGUI();
            #endregion
        }

        void DrawArrow(Vector3 position, Vector3 direction, Color color, string label = "", float size = 0.01f)
        {
            Handles.color = color;
            Handles.DrawLine(position, position + direction);
            Handles.SphereHandleCap(0, position + direction, Quaternion.identity, size, EventType.Repaint);
            Handles.color = Color.white;

            if (label != "")
            {
                GUI.color = Color.white;
                Handles.Label(position + direction, label);
                Handles.color = color;
            }
        }

        float DrawLimitHandle(float limit, Vector3 position, Quaternion rotation, float radius, string label, float openingValue, float sizeCircle)
        {
            limit = Handles.ScaleValueHandle(limit, position, rotation, radius, Handles.SphereHandleCap, 1);
            string labelInfo = label + ": " + limit.ToString();

            // If value is 0, draw a button to 'open' the value, because we cant scale 0
            if (limit == 0)
            {
                labelInfo = "Open " + label;
                if (Handles.Button(position, rotation, radius * sizeCircle, radius * 0.07f, Handles.SphereHandleCap))
                {
                    limit = openingValue;
                }
            }

            Handles.Label(position, labelInfo);

            return limit;
        }

        private Vector3 Direction(Vector3 v, bool ignoreParent = true)
        {
            if (Application.isPlaying && ignoreParent)
                return Script.defaultRotation * v;

            if (Script.transform.parent == null)
                return Script.defaultLocalRotation * v;
            else
                return (Script.transform.parent.rotation * Script.defaultLocalRotation) * v;

        }

    }
}