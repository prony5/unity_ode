using UnityEditor;
using UnityEngine;

namespace UnityODE
{
    [CustomEditor(typeof(OdeCenterOfMass))]
    [CanEditMultipleObjects]
    public class OdeCenterOfMassEditor : Editor
    {
        private OdeCenterOfMass Script { get { return target as OdeCenterOfMass; } }

        void Calc()
        {
            var bodies = Script.GetComponentsInChildren<OdeBody>();
            float mass = 0;
            foreach (var b in bodies)
                if (b.enabled)
                    mass += b.inertial.mass;
            Vector3 com = new Vector3();

            if (mass > 0)
            {
                foreach (var b in bodies)
                    if (b.enabled)
                        com += OdeUtils.RotateAroundPivot(b.transform.position + b.inertial.com, b.transform.position, b.transform.rotation) * (b.inertial.mass / mass);
            }

            Script.SetCOM(mass, com);
        }


        void OnSceneGUI()
        {
            if (!Application.isPlaying)
                Calc();

            Handles.color = OdeEdittorDefault.color;

            Handles.Label(Script.position, "COM");
            float dist = 1;
            if (Camera.current != null)
                dist = Vector3.Dot((Script.position - Camera.current.transform.position), Camera.current.transform.forward);
            Handles.SphereHandleCap(0, Script.position, Quaternion.identity, 0.025f * dist, EventType.Repaint);
        }
    }
}