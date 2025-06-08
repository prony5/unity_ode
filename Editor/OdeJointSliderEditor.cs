using UnityEngine;
using UnityEditor;

namespace UnityODE
{
    [CustomEditor(typeof(OdeJointSlider))]
    [CanEditMultipleObjects]
    public class OdeJointSliderEditor : OdeJointEditor
    {
        private OdeJointSlider Script { get { return target as OdeJointSlider; } }

        void OnSceneGUI()
        {
            if (Script.Axis == Vector3.zero || Script.JointLimits.visible == false) return;

            Vector3 position = Script.transform.position;
            Quaternion rotation = Script.transform.rotation;

            if (Application.isPlaying)
                position -= (rotation * Script.Axis.normalized) * Script.Value;

            float size = 0.5f;
            var mf = Script.GetComponent<MeshFilter>();
            if (mf != null && mf.sharedMesh != null)
                size = mf.sharedMesh.bounds.size.magnitude;
            if (size > 1) size = 1;

            float sizeSphere = 0.15f * size;
            if (sizeSphere > 0.05f) sizeSphere = 0.05f;
            Handles.color = OdeEdittorDefault.color;

            //Draw limit start
            Handles.SphereHandleCap(0, position, Quaternion.identity, sizeSphere, EventType.Repaint);
            DrawText(position, "0", Color.white);
            /*
            var normAxis = rotation * Script.Axis.normalized;
            if (Mathf.Abs(Script.limitMin - Script.limitMax) > 0.0001f)
            {
                Vector3 minPoint = position + normAxis * Script.limitMin;
                Vector3 maxPoint = position + normAxis * Script.limitMax;

                Handles.DrawLine(minPoint, maxPoint);

                Handles.SphereHandleCap(0, minPoint, Quaternion.identity, sizeSphere, EventType.Repaint);
                DrawText(minPoint, "Min", Color.white);

                Handles.SphereHandleCap(0, maxPoint, Quaternion.identity, sizeSphere, EventType.Repaint);
                DrawText(maxPoint, "Max", Color.white);
            }
            else
            {
                var axis05 = normAxis * 0.5f;
                Handles.DrawLine(position - axis05, position + axis05);
            }
             */
        }
    }
}
