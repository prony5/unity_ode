using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityODE
{
    [AddComponentMenu("ODE/Joints/Ball Joint")]
    public class OdeJointBall : OdeJoint
    {
        #region Inspector
        [Header("Ball properties")]
        [Tooltip("Anchor point in the local coordinate system."), OdeReadOnlyOnPlay]
        public Vector3 anchor;
        [Range(0, 1)]
        public float erp = 0.25f;
        [Range(0, 0.0001f)]
        public float cfm = 1e-5f;
        #endregion

        private float _erp;
        private float _cfm;

        private Ode.Net.Joints.Ball _bJoint;

        protected override Ode.Net.Joints.Joint Init()
        {
            _bJoint = new Ode.Net.Joints.Ball(OdeWorld.CurrentWorld);
            return _bJoint;
        }

        protected override void AfterInit()
        {
            base.AfterInit();
            _bJoint.Anchor = (transform.position + transform.rotation * anchor).ToODE();
            OdeWorld.OnBeforeStep += BeforeSimStep;
        }

        protected override void BeforeDestroy()
        {
            OdeWorld.OnBeforeStep -= BeforeSimStep;
            base.BeforeDestroy();
        }

        void BeforeSimStep()
        {
            if (_erp != erp)
            {
                _erp = erp;
                _bJoint.Erp = erp;
            }
            if (_cfm != cfm)
            {
                _cfm = cfm;
                _bJoint.Cfm = cfm;
            }
        }

    }
}