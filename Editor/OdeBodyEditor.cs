using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UnityODE
{
    [CustomEditor(typeof(OdeBody))]
    [CanEditMultipleObjects]
    public class OdeBodyEditor : Editor
    {
        private OdeBody Script { get { return target as OdeBody; } }

        void OnSceneGUI()
        {
            Vector3 position = Script.transform.position + OdeUtils.RotateAroundPivot(Script.inertial.com, Vector3.zero, Script.transform.rotation);

            Handles.color = OdeEdittorDefault.color;

            Handles.Label(position, "COM [" + Script.name + "]");
            float dist = 1;
            if (Camera.current != null)
                dist = Vector3.Dot((position - Camera.current.transform.position), Camera.current.transform.forward);
            Handles.SphereHandleCap(0, position, Quaternion.identity, 0.015f * dist, EventType.Repaint);
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var bodyes = Script.GetComponents<OdeBody>();
            if (bodyes.Length > 1)
            {
                for (int i = 1; i < bodyes.Length; i++)
                {
                    if (Application.isEditor)
                        DestroyImmediate(bodyes[i]);
                    else
                        Destroy(bodyes[i]);
                }
                Debug.LogError("More than one OdeBody per GameObject.");
            }
        }
    }

}

