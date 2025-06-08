using UnityEngine;
using UnityEditor;

namespace UnityODE
{
    [CustomEditor(typeof(OdeJointBall))]
    [CanEditMultipleObjects]
    public class OdeJointBallEditor : OdeJointEditor
    {
        private OdeJointBall script { get { return target as OdeJointBall; } }

        void OnSceneGUI()
        {

            var position = script.transform.position + script.transform.rotation * script.anchor;
            float size = 0.5f;
            var mf = script.GetComponent<MeshFilter>();
            if (mf != null && mf.sharedMesh != null)
                size = mf.sharedMesh.bounds.size.magnitude;
            if (size > 1) size = 1;


            Handles.color = OdeEdittorDefault.color;
            Handles.SphereHandleCap(0, position, Quaternion.identity, size * 0.05f, EventType.Repaint);

            Handles.DrawLine(position - Vector3.forward * size, position + Vector3.forward * size);
            Handles.CircleHandleCap(0, position, Quaternion.AngleAxis(90, Vector3.forward), size, EventType.Repaint);

            Handles.DrawLine(position - Vector3.up * size, position + Vector3.up * size);
            Handles.CircleHandleCap(0, position, Quaternion.AngleAxis(90, Vector3.up), size, EventType.Repaint);

            Handles.DrawLine(position - Vector3.right * size, position + Vector3.right * size);
            Handles.CircleHandleCap(0, position, Quaternion.AngleAxis(90, Vector3.right), size, EventType.Repaint);

            GUI.color = Color.white;
            Handles.Label(position, "Anhor");

        }
    }
}

