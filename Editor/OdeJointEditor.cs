using UnityEditor;
using UnityEngine;

namespace UnityODE
{
    [CustomEditor(typeof(OdeJoint))]
    public class OdeJointEditor : Editor
    {
        private OdeJoint Script { get { return target as OdeJoint; } }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (Script.name == null)
                Script.name = Script.gameObject.name;

            if (Script.HasConnectedBody && Script.connectedBody == Script.GetComponent<OdeBody>())
            {
                Script.connectedBody = null;
                Debug.LogError("Joint can't connect the body to itself.");
            }
        }

        public static void DrawLimits(Vector3 anchor, Vector3 axis, Quaternion rotation, float zeroAxisDisplayOffset, Color color, OdeJoint.Limits limits, string label = "Axis")
        {
            float size = 0.5f;
            float sizeSphere = size * 0.1f;
            Vector3 cross = Quaternion.AngleAxis(zeroAxisDisplayOffset, axis) * Vector3.Cross(axis, new Vector3(axis.y, axis.z, axis.x)).normalized;

            Handles.color = color;

            // Rotation circle
            Handles.CircleHandleCap(0, anchor, rotation * Quaternion.LookRotation(axis, cross), size, EventType.Repaint);

            // Axis vector            
            var axisVector = anchor + axis * size;
            Handles.DrawLine(anchor, axisVector);
            Handles.SphereHandleCap(0, axisVector, Quaternion.identity, sizeSphere, EventType.Repaint);
            DrawText(axisVector, label, Color.white);

            // Arcs for the rotation limit
            Handles.color = new Color(color.r, color.g, color.b, 0.15f);
            Handles.DrawSolidArc(anchor, rotation * axis, rotation * cross, limits.min, size);
            Handles.DrawSolidArc(anchor, rotation * axis, rotation * cross, limits.max, size);

            //Draw limits
            Handles.color = color;
            DrawStopLimit(0, anchor, axis, cross, rotation, size, sizeSphere, "0");
            DrawStopLimit(limits.min, anchor, axis, cross, rotation, size, sizeSphere, "Min");
            DrawStopLimit(limits.max, anchor, axis, cross, rotation, size, sizeSphere, "Max");
        }

        public static void DrawStopLimit(float limit, Vector3 anchor, Vector3 axis, Vector3 cross, Quaternion rotation, float size, float sizeSphere, string label = "")
        {
            Vector3 limitPoint = anchor + rotation * Quaternion.AngleAxis(limit, axis) * cross * size;
            // Draw limit line
            Handles.DrawLine(anchor, limitPoint);
            //Draw limit point
            Handles.SphereHandleCap(0, limitPoint, Quaternion.identity, sizeSphere, EventType.Repaint);

            DrawText(limitPoint, label, Color.white);
        }

        public static void DrawText(Vector3 position, string label, Color color)
        {
            if (label == "")
                return;

            var lastColor = Handles.color;
            Handles.color = color;
            Handles.Label(position, label);
            Handles.color = lastColor;
        }
    }
}
