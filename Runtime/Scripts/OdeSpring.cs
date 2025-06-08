using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityODE
{
    [AddComponentMenu("ODE/Joints/Spring")]
    public class OdeSpring : MonoBehaviour
    {
        #region Inspector
        public OdeJointMove joint;
        [Tooltip("Коэф. упругости (Нм/градус).")]
        public float spring;
        [Tooltip("Смещение (градус) - определяет начальное сжатие/растяжение.")]
        public float offset;
        #endregion

        private bool _inited = false;


        void OnEnable()
        {
            _inited = false;
            if (OdeWorld.CurrentWorld == null)
            {
                enabled = false;
                return;
            }

            lock (OdeWorld.Sync)
            {
                OdeWorld.OnAfterStep += AfterSimStep;
                _inited = true;
            }
        }

        void OnDisable()
        {
            lock (OdeWorld.Sync)
            {
                if (_inited == false)
                    return;
                _inited = false;

                OdeWorld.OnAfterStep -= AfterSimStep;
            }
        }

        void AfterSimStep()
        {
            if (joint == null || joint.Inited == false)
                return;

            var _spring = spring < 0 ? 0 : spring;
            /*
            joint.Mode = OdeJointMove.MoveMode.force;
            joint.limitForce = Mathf.Abs((360 + offset) * _spring);
            joint.Force = (-joint.Value + offset) * _spring - ((joint.Velocity * OdeWorld.Settings.stepTime * 2) * _spring);
             * */
        }
    }
}