using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityODE
{
    public interface IMotor
    {
        /// <summary>
        /// Положение выходного вала [градус].
        /// </summary>
        float AxisPosition { get; }

        /// <summary>
        /// Угловая скорость на выходном валу [градус/сек].
        /// </summary>
        float AxisVelocity { get; }

        /// <summary>
        /// Ускорение на выходном валу [градус/сек^2].
        /// </summary>
        float AxisAcceleration { get; }

        /// <summary>
        /// Крутящий момент на выходном валу, с учетом КПД мотора и редуктора [Н*м].
        /// </summary>
        float AxisTorque { get; }

        /// <summary>
        /// Угловая скорость на моторе [об/мин].
        /// </summary>
        float MotorVelocity { get; }

        /// <summary>
        /// Крутящий момент на моторе [Н*м].
        /// </summary>
        float MotorTorque { get; }

        /// <summary>
        /// Потребление тока мотором [А].
        /// </summary>
        float Current { get; }
    }


    [AddComponentMenu("ODE/Motors/Motor")]
    [RequireComponent(typeof(OdeJointHinge))]
    public class OdeMotor : MonoBehaviour, IMotor
    {
        #region Inspector
        [Header("Main properties")]
        public new string name;

        public string model;

        [Tooltip("Номинальный ток [А].")]
        public float nominalCurrent;

        [Tooltip("Частота вращения на холостом ходу [об/мин].")]
        public float noLoadSpeed;

        [Tooltip("Коэф. передачи момента [(Н*м)/А].")]
        public float KT;

        [Tooltip("Зависимость момента от оборотов [(Н*м)/(об/мин)].")]
        public AnimationCurve KV;

        [Tooltip("КПД мотора, с учетом КПД контроллера [%/100]."), Range(0, 1)]
        public float KE = 1;

        [Header("Gear")]
        [Tooltip("Передаточное число редуктора.")]
        public float gear = 1;

        [Tooltip("КПД редуктора [%/100]."), Range(0, 1)]
        public float KR = 1;

        [Header("Move properties")]
        [SerializeField, OdeReadOnly, Tooltip("Положение выходного вала [градус].")]
        private float _axisPosition;

        [SerializeField, OdeReadOnly, Tooltip("Угловая скорость на выходном валу [градус/сек].")]
        private float _axisVelocity;

        private float _axisAcceleration;

        [SerializeField, OdeReadOnly, Tooltip("Крутящий момент на выходном валу, с учетом КПД мотора и редуктора [Н*м].")]
        private float _axisTorque;

        [SerializeField, OdeReadOnly, Tooltip("Угловая скорость на моторе [об/мин].")]
        private float _motorVelocity;

        [SerializeField, OdeReadOnly, Tooltip("Крутящий момент на моторе [Н*м].")]
        private float _motorTorque;

        [SerializeField, OdeReadOnly, Tooltip("Потребление тока мотором [А].")]
        private float _current;

        [Space(20)]
        [SerializeField]
        private Controller _controller = new Controller();
        #endregion

        #region IMotor
        public float AxisPosition { get { return _axisPosition; } }

        public float AxisVelocity { get { return _axisVelocity; } }

        public float AxisAcceleration { get { return _axisAcceleration; } }

        public float AxisTorque { get { return _axisTorque; } }

        public float MotorVelocity { get { return _motorVelocity; } }

        public float MotorTorque { get { return _motorTorque; } }

        public float Current { get { return _current; } }

        #endregion
        public Controller Controller { get { return _controller; } }

        public OdeJointHinge Joint { get { return _joint; } }
        private OdeJointHinge _joint;

        void OnEnable()
        {
            lock (OdeWorld.Sync)
            {
                OdeWorld.OnBeforeStep += BeforeSimStep;
                OdeWorld.OnAfterStep += AfterSimStep;
            }
        }

        void OnDisable()
        {
            lock (OdeWorld.Sync)
            {
                OdeWorld.OnBeforeStep -= BeforeSimStep;
                OdeWorld.OnAfterStep -= AfterSimStep;
            }
        }

        private void Start()
        {
            _joint = GetComponent<OdeJointHinge>();
        }

        protected void BeforeSimStep()
        {
            if (noLoadSpeed <= 0 || gear <= 0)
            {
                _current = 0;
                _motorTorque = 0;
                _axisTorque = 0;
                return;
            }

            // Расчитываем задание тока
            float currentTarget = _controller.CalcCurrent(this);

            // Проверяем задание на лимит по току
            if (Mathf.Abs(currentTarget) > nominalCurrent)
            {
                currentTarget = nominalCurrent * Mathf.Sign(currentTarget);
                _controller.IsReached = true;
            }

            // Расчитываем момент на валу мотора, с учетом КПД мотора и контроллера
            _motorTorque = currentTarget * KT * KE;

            // Расчитываем падение момента от оборотов
            if (Mathf.Sign(currentTarget) == Mathf.Sign(_motorVelocity))
            {
                var motorTorqueLimit = KV.Evaluate(Mathf.Abs(_motorVelocity));
                if (Mathf.Abs(_motorTorque) > motorTorqueLimit)
                {
                    _motorTorque = motorTorqueLimit * Mathf.Sign(_motorTorque);
                    _controller.IsReached = true;
                }
            }

            // Расчитываем ток фактический, из момента заданного. Предпологая, что контроллер успел отработать задание по току за шаг симуляции. 
            _current = KT <= 0 ? 0 : (_motorTorque / KT) / KE;

            // Расчитываем момент на выходном валу с учетом КПД редуктора
            _axisTorque = _motorTorque * gear * KR;

            // Прикладываем момент к оси вращения
            _joint.AddTorque(_axisTorque);
        }

        private float _lastJointValue;
        private int _circleCounter;
        protected void AfterSimStep()
        {
            // Расчитываем ускорение 
            _axisAcceleration = (_joint.Velocity - _axisVelocity) / OdeWorld.Settings.stepTime;

            // Расчитываем угловую скорость на выходном валу
            _axisVelocity = _joint.Velocity;

            //Расчитываем положение выходного вала
            var value = _joint.Value;
            var valueRate = value - _lastJointValue;
            if (valueRate > 180f) _circleCounter--; else if (valueRate < -180f) _circleCounter++;
            _lastJointValue = value;
            _axisPosition = (_circleCounter * 360) + value;

            // Расчитываем угловую скорость мотора
            _motorVelocity = (_axisVelocity / 6f) * gear;
        }
    }

}