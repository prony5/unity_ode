using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityODE
{
    public abstract class OdeJointMove : OdeJoint
    {
        #region Inspector
        [Tooltip("Ось вращения/перемещения. Задается в системе координат тела."), SerializeField, OdeReadOnlyOnPlay]
        protected Vector3 axis = Vector3.forward;

        [SerializeField, OdeReadOnly]
        protected float _value;
        [SerializeField, OdeReadOnly]
        protected float _velocity;
        protected float _acceleration;

        [SerializeField]
        protected Limits _limits = new Limits();
        #endregion

        #region Public
        /// <summary>
        /// Ось вращения/перемещения. Задается в системе координат тела.
        /// </summary>
        public Vector3 Axis { get { return axis; } set { if (!Inited) axis = value; } }
        public Limits JointLimits { get { return _limits; } }
        #endregion

        protected abstract Ode.Net.Joints.JointLimitMotor GetLimitMotor();
        protected abstract float GetValue();
        protected abstract float GetVelocity();

        protected bool BeforeInit()
        {
            if (axis == Vector3.zero)
            {
                OdeWorld.Debug.LogError(name + " axis don't must be zero.");
                return false;
            }
            return true;
        }

        protected override void AfterInit()
        {
            base.AfterInit();
            _limits.Motor = GetLimitMotor();
            OdeWorld.OnBeforeStep += BeforeSimStep;
            OdeWorld.OnAfterStep += AfterSimStep;
        }

        protected override void BeforeDestroy()
        {
            OdeWorld.OnBeforeStep -= BeforeSimStep;
            OdeWorld.OnAfterStep -= AfterSimStep;
            base.BeforeDestroy();
        }

        protected virtual void BeforeSimStep()
        {
            UpdateLimits();
        }

        protected virtual void AfterSimStep()
        {
            if (Body.Inited && (HasConnectedBody == false || connectedBody.Inited))
            {
                _value = GetValue();
                var vel = GetVelocity(); 
                _acceleration = (vel - _velocity) / OdeWorld.Settings.stepTime;
                _velocity = vel;
            }
        }

        protected virtual void UpdateLimits()
        {
            _limits.Motor.LowStop = _limits.min;
            _limits.Motor.HighStop = _limits.max;
        }
    }
}