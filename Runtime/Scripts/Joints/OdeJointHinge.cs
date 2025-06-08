using UnityEngine;

namespace UnityODE
{
    [AddComponentMenu("ODE/Joints/Hinge Joint")]
    public class OdeJointHinge : OdeJointMove
    {
        [Header("Hinge properties")]
        [Tooltip("Точка прикрепления сочленения к телу. Задается в системе координат тела."), SerializeField, OdeReadOnlyOnPlay]
        private Vector3 anchor;

        #region Public
        /// <summary>
        /// Точка прикрепления сочленения к телу. Задается в системе координат тела.
        /// </summary>
        public Vector3 Anchor { get { return anchor; } set { if (!Inited) anchor = value; } }

        /// <summary>
        /// Разница между текущим и прикрепленным телом [градус].
        /// </summary>
        public float Value { get { return _value; } }

        /// <summary>
        /// Скорость между текущим и прикрепленным телом [градус/сек].
        /// </summary>
        public float Velocity { get { return _velocity; } }

        /// <summary>
        /// Прикладывает крутящий момент к оси вращения, на один шаг симуляции [Ньютон*метр].
        /// </summary>
        public void AddTorque(float value)
        {
            if (Inited)
                _hJoint.AddTorque(value);
        }
        #endregion Public

        [HideInInspector]
        public Quaternion defaultLocalRotation = Quaternion.identity;
        [HideInInspector]
        public Quaternion defaultRotationConnectedBody = Quaternion.identity;
        [HideInInspector]
        public Quaternion defaultRotation = Quaternion.identity;
        [HideInInspector]
        public float zeroAxisDisplayOffset;

        private Ode.Net.Joints.Hinge _hJoint;

        protected override Ode.Net.Joints.Joint Init()
        {
            if (BeforeInit())
            {
                _hJoint = new Ode.Net.Joints.Hinge(OdeWorld.CurrentWorld);
                return _hJoint;
            }
            else
                return null;
        }

        protected override void AfterInit()
        {
            base.AfterInit();
            _hJoint.Anchor = (transform.position + transform.rotation * anchor).ToODE();
            _hJoint.Axis = (transform.rotation * axis).ToODE();

            defaultLocalRotation = transform.localRotation;
            defaultRotationConnectedBody = HasConnectedBody ? connectedBody.transform.rotation : Quaternion.identity;
            defaultRotation = transform.rotation;
        }

        protected override Ode.Net.Joints.JointLimitMotor GetLimitMotor()
        {
            return _hJoint.LimitMotor;
        }

        protected override float GetValue()
        {
            return Mathf.Rad2Deg * (float)_hJoint.Angle;
        }

        protected override float GetVelocity()
        {
            return Mathf.Rad2Deg * (float)_hJoint.AngleRate;
        }

        protected override void UpdateLimits()
        {
            _limits.Motor.LowStop = Mathf.Deg2Rad * _limits.min;
            _limits.Motor.HighStop = Mathf.Deg2Rad * _limits.max;
        }
    }
}