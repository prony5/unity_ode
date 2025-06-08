using UnityEngine;
using UnityEditor;

namespace UnityODE
{
    [CustomEditor(typeof(OdeIMU))]
    public class OdeSensorImuEditor : Editor
    {
        private OdeIMU script { get { return target as OdeIMU; } }

        void OnSceneGUI()
        {
            float size = 0.5f;
            var mf = script.GetComponent<MeshFilter>();
            if (mf != null && mf.sharedMesh != null)
                size = mf.sharedMesh.bounds.size.magnitude;
            if (size > 1) size = 1;

            if (Application.isPlaying == false) script.defaultRotation = script.transform.rotation;

            var pos = script.transform.position + script.refPoint;
            var rot = script.transform.rotation;

            Handles.color = OdeEdittorDefault.color;
            Handles.CubeHandleCap(0, pos, Quaternion.identity, 0.025f, EventType.Repaint);
            GUI.color = Color.white;
            Handles.Label(pos, "IMU [" + script.name + "]");
        }
    }
}
