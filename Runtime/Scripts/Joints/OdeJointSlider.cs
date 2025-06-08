using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityODE
{
    [AddComponentMenu("ODE/Joints/Slider Joint")]
    public class OdeJointSlider : OdeJointMove
    {
        #region Public
        /// <summary>
        /// Разница между текущим и прикрепленным телом [метр].
        /// </summary>
        public float Value { get { return _value; } }

        /// <summary>
        /// Скорость между текущим и прикрепленным телом [метр/сек].
        /// </summary>
        public float Velocity { get { return _velocity; } }

        /// <summary>
        /// Прикладывает силу к оси скольжения, на один шаг симуляции [Ньютон].
        /// </summary>
        public void AddForce(float value)
        {
            if (Inited)
                _sJoint.AddForce(value);
        }
        #endregion Public

        private Ode.Net.Joints.Slider _sJoint;

        protected override Ode.Net.Joints.Joint Init()
        {
            if (BeforeInit())
            {
                _sJoint = new Ode.Net.Joints.Slider(OdeWorld.CurrentWorld);
                return _sJoint;
            }
            else
                return null;
        }

        protected override void AfterInit()
        {
            base.AfterInit();
            _sJoint.Axis = (transform.rotation * axis).ToODE();
        }

        protected override Ode.Net.Joints.JointLimitMotor GetLimitMotor()
        {
            return _sJoint.LimitMotor;
        }

        protected override float GetValue()
        {
            return (float)_sJoint.Position;
        }

        protected override float GetVelocity()
        {
            return (float)_sJoint.PositionRate;
        }
    }
}
