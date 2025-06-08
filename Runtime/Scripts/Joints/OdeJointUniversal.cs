using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityODE
{
    [AddComponentMenu("ODE/Joints/Universal Joint")]
    public class OdeJointUniversal : OdeJoint
    {
        #region Inspector
        [Header("Universal properties")]
        [Tooltip("Anchor point in the local coordinate system."), OdeReadOnlyOnPlay]
        public Vector3 anchor;
        [Tooltip("Move axis in the local coordinate system."), OdeReadOnlyOnPlay]
        public Vector3 axis1 = Vector3.forward;
        [Tooltip("Move axis in the local coordinate system."), OdeReadOnlyOnPlay]
        public Vector3 axis2 = Vector3.up;

        public Limits limits1 = new Limits();
        public Limits limits2 = new Limits();
        #endregion

        [HideInInspector]
        public float zeroAxisDisplayOffset = 0;
        [HideInInspector]
        public float zeroAxisDisplayOffset2 = 0;

        [HideInInspector]
        public Quaternion defaultRotation;
        [HideInInspector]
        public Quaternion defaultRotationConnectedBody;

        private Ode.Net.Joints.Universal _uJoint;

        protected override Ode.Net.Joints.Joint Init()
        {
            _uJoint = new Ode.Net.Joints.Universal(OdeWorld.CurrentWorld);
            limits1.Motor = _uJoint.LimitMotor1;
            limits2.Motor = _uJoint.LimitMotor2;

            defaultRotation = transform.rotation;
            defaultRotationConnectedBody = connectedBody == null ? Quaternion.identity : connectedBody.transform.rotation;

            return _uJoint;
        }

        protected override void AfterInit()
        {
            OdeWorld.OnBeforeStep += BeforeSimStep;
            base.AfterInit();

            _uJoint.Anchor = (transform.position + transform.rotation * anchor).ToODE();
            _uJoint.Axis1 = (transform.rotation * axis1).ToODE();
            _uJoint.Axis2 = (transform.rotation * axis2).ToODE();
        }

        protected override void BeforeDestroy()
        {
            OdeWorld.OnBeforeStep -= BeforeSimStep;
            base.BeforeDestroy();
        }

        void BeforeSimStep()
        {
            limits1.Motor.LowStop = Mathf.Deg2Rad * limits1.min;
            limits1.Motor.HighStop = Mathf.Deg2Rad * limits1.max;
            // limits1.Motor.Velocity = Mathf.Deg2Rad * limits1.velocity;
            //   limits1.Motor.MaxForce = limits1.force;

            limits2.Motor.LowStop = Mathf.Deg2Rad * limits2.min;
            limits2.Motor.HighStop = Mathf.Deg2Rad * limits2.max;
            // limits2.LimitMotor.Velocity = Mathf.Deg2Rad * limits2.velocity;
            //  limits2.LimitMotor.MaxForce = limits2.force;         
        }

    }
}