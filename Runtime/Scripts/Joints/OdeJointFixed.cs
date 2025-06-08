using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityODE
{
    [AddComponentMenu("ODE/Joints/Fixed Joint")]
    public class OdeJointFixed : OdeJoint
    {
        #region Inspector
        [Header("Fixed properties")]
        [Tooltip("Delay in second before fix."), OdeReadOnlyOnPlay]
        public float fixDelay = 0;
        public float breakForce = float.PositiveInfinity;
        #endregion

        private Ode.Net.Joints.Fixed _fJoint;
        private bool _needDestroy = false;

        protected override Ode.Net.Joints.Joint Init()
        {
            UseFeedback = true;
            _fJoint = new Ode.Net.Joints.Fixed(OdeWorld.CurrentWorld);
            return _fJoint;
        }

        protected override void AfterInit()
        {
            base.AfterInit();
            _fJoint.Fix();
            OdeWorld.OnAfterStep += AfterSimStep;
        }

        protected override void BeforeDestroy()
        {
            OdeWorld.OnAfterStep -= AfterSimStep;
            base.BeforeDestroy();
        }

        void AfterSimStep()
        {
            /*
            if (_fixed == false)
            {
                if (OdeWorld.Statistics.time >= fixDelay)
                {
                    _fixed = true;
                    _joint.Attach(Body.Body, connectedBody == null ? null : connectedBody.Body);
                    _fJoint.Fix();
                }
            }
            else
            {
                if (Mathf.Abs(Vector3.Magnitude(_joint.Feedback.ForceOnBody1.ToUnity())) >= breakForce)
                {
                    _inited = false;
                    _needDestroy = true;

                    BeforeDestroy();
                    _joint.Feedback.Dispose();
                    _joint.Dispose();
                }
            }
             */
        }

        void Update()
        {
            if (_needDestroy)
                Destroy(this);
        }
    }

}