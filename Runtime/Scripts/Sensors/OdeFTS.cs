using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityODE
{
    [AddComponentMenu("ODE/Sensors/ForceTorque")]
    [RequireComponent(typeof(OdeJoint))]
    public class OdeFTS : OdeSensor
    {
        #region Inspector
        public bool local = true;
        [OdeReadOnly]
        public Vector3 Force;
        [OdeReadOnly]
        public Vector3 Torque;
        [Range(0, 100)]
        public float filter = 0;
        #endregion

        private bool _inited;
        private OdeJoint joint;
        private Ode.Net.Joints.JointFeedback jf;
        private OdeFilter _filter;

        private void OnDisable()
        {
            if (_inited)
            {
                _inited = false;
                OdeWorld.OnAfterStep -= AfterSimStep;
            }
        }

        void Start()
        {
            _filter = new OdeFilter(6, OdeWorld.Settings.stepTime);
            _values = new float[6];
            _valuesID = new string[6] { "FX", "FY", "FZ", "TX", "TY", "TZ" };

            joint = GetComponent<OdeJoint>();
            if (joint == null || joint.UseFeedback == false)
                return;
            jf = joint.Joint.Feedback;
            if (jf == null)
                return;

            OdeWorld.OnAfterStep += AfterSimStep;
            _inited = true;
        }

        private void Update()
        {
            _filter.tau = filter;
        }

        void AfterSimStep()
        {
            if (_inited == false || joint.Inited == false || joint.Body.Inited == false)
                return;

            if (local)
            {
                var rot = Quaternion.Inverse(joint.Body.Rotation);
                Force = rot * jf.ForceOnBody1.ToUnity();
                Torque = rot * jf.TorqueOnBody1.ToUnity();
            }
            else
            {
                Force = jf.ForceOnBody1.ToUnity();
                Torque = jf.TorqueOnBody1.ToUnity();
            }

            if (float.IsInfinity(Force.x) || float.IsInfinity(Force.y) || float.IsInfinity(Force.z) || float.IsNaN(Force.x) || float.IsNaN(Force.y) || float.IsNaN(Force.z))
                return;

            if (filter > 0)
            {
                Force.x = (float)_filter.Calc(Force.x, 0);
                Force.y = (float)_filter.Calc(Force.y, 1);
                Force.z = (float)_filter.Calc(Force.z, 2);
                Torque.x = (float)_filter.Calc(Torque.x, 3);
                Torque.y = (float)_filter.Calc(Torque.y, 4);
                Torque.z = (float)_filter.Calc(Torque.z, 5);
            }

            _values[0] = Force.x;
            _values[1] = Force.y;
            _values[2] = Force.z;
            _values[3] = Torque.x;
            _values[4] = Torque.y;
            _values[5] = Torque.z;
        }

    }
}
