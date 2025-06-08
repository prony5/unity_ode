using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System;

namespace UnityODE
{
    [AddComponentMenu("ODE/Joints/Wheel")]
    public class OdeJointWheel : OdeJoint
    {
        #region Inspector
        [Header("Hinge properties")]
        [Tooltip("Точка прикрепления оси колеса к поворотной оси."), OdeReadOnlyOnPlay]
        public Vector3 anchor;
        [Tooltip("Точка прикрепления колеса к поворотной оси."), OdeReadOnly]
        public Vector3 anchorWheel;

        [Tooltip("Ось поворота колеса."), OdeReadOnlyOnPlay]
        public Vector3 axisWheelRotation = Vector3.up;
        [Tooltip("Ось вращения колеса."), OdeReadOnlyOnPlay]
        public Vector3 axisWheel = Vector3.right;

        public Limits limitsWheelRotation = new Limits();
        public Limits limitsWheel = new Limits();

        public SpringDamper suspension = new SpringDamper();

        [Header("Debug")]
        public bool showLimits = true;
        #endregion

        #region Public
        /// <summary>
        /// Угол поворота колеса [градус].
        /// </summary>
        public float ValueWheelRotation { get { return _valueWheelRotation; } }

        /// <summary>
        /// Уголовая скорость поворота колеса [градус/сек].
        /// </summary>
        public float VelocityWheelRotation { get { return _velocityWheelRotation; } }

        /// <summary>
        /// Уголовая скорость вращения колеса [градус/сек].
        /// </summary>
        public float VelocityWheel { get { return _velocityWheel; } }

        public void AddTorques(float wheelRotationTorque, float wheelTorque)
        {
            if (Inited)
                _h2Joint.AddTorques(wheelRotationTorque, wheelTorque);
        }

        [HideInInspector]
        public float zeroAxisDisplayOffset = 0;
        #endregion

        [SerializeField, Tooltip("Угол поворота колеса [градус].")]
        private float _valueWheelRotation; private float _lastValueWheelRotation;
        [SerializeField, Tooltip("Уголовая скорость поворота колеса [градус/сек].")]
        private float _velocityWheelRotation;
        [SerializeField, Tooltip("Уголовая скорость вращения колеса [градус/сек].")]
        private float _velocityWheel;

        private Ode.Net.Joints.Hinge2 _h2Joint;

        protected override Ode.Net.Joints.Joint Init()
        {
            if (!HasConnectedBody)
            {
                OdeWorld.Debug.LogError("Wheel joint must be have connected body!");
                return null;
            }

            _h2Joint = new Ode.Net.Joints.Hinge2(OdeWorld.CurrentWorld);

            limitsWheelRotation.Motor = _h2Joint.LimitMotor1;
            limitsWheel.Motor = _h2Joint.LimitMotor2;

            return _h2Joint;
        }

        protected override void AfterInit()
        {
            OdeWorld.OnBeforeStep += BeforeSimStep;
            OdeWorld.OnAfterStep += AfterSimStep;

            base.AfterInit();

            _h2Joint.Anchor = (transform.position + transform.rotation * anchor).ToODE();
            _h2Joint.Axis1 = (transform.rotation * axisWheelRotation).ToODE();
            _h2Joint.Axis2 = (transform.rotation * axisWheel).ToODE();
        }

        protected override void BeforeDestroy()
        {
            OdeWorld.OnBeforeStep -= BeforeSimStep;
            OdeWorld.OnAfterStep -= AfterSimStep;

            base.BeforeDestroy();
        }

        public float TargetTorque;

        void BeforeSimStep()
        {

            limitsWheelRotation.Motor.LowStop = Mathf.Deg2Rad * limitsWheelRotation.min;
            limitsWheelRotation.Motor.HighStop = Mathf.Deg2Rad * limitsWheelRotation.max;
            //  limitsWheelRotation.Motor.Velocity = Mathf.Deg2Rad * limitsWheelRotation.velocity;
            //      limitsWheelRotation.Motor.MaxForce = limitsWheelRotation.force;
            /*
                        limitsWheel.LimitMotor.LowStop = Mathf.Deg2Rad * limitsWheel.min;
                        limitsWheel.LimitMotor.HighStop = Mathf.Deg2Rad * limitsWheel.max;
                        limitsWheel.LimitMotor.Velocity = Mathf.Deg2Rad * limitsWheel.velocity;
                        limitsWheel.LimitMotor.MaxForce = limitsWheel.force;
                        */

            float erp; float cfm; suspension.CalcEprCfm(out erp, out cfm);
            _h2Joint.SuspensionErp = erp;
            _h2Joint.SuspensionCfm = cfm;

            AddTorques(0, TargetTorque);
        }

        void AfterSimStep()
        {
            if (Body.Inited && (HasConnectedBody == false || connectedBody.Inited))
            {
                // Забор данных связанных с поворотом колеса
                _valueWheelRotation = Mathf.Rad2Deg * (float)_h2Joint.Angle1;
                _velocityWheelRotation = (_valueWheelRotation - _lastValueWheelRotation) / OdeWorld.Settings.stepTime;
                _lastValueWheelRotation = _valueWheelRotation;

                // Забор данных связанных с вращением колеса
                _velocityWheel = Mathf.Rad2Deg * (float)_h2Joint.Angle2Rate;
                //    _torqueWheel = (Quaternion.Inverse(connectedBody.Rotation) * _jointFeedback.TorqueOnBody2.ToUnity()).magnitude;

            }
        }

    }
}