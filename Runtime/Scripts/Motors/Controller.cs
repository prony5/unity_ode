using System;
using UnityEngine;
using P7;

namespace UnityODE
{
    [Serializable]
    public class Controller
    {
        [Serializable]
        public class Verification
        {
            [Serializable]
            public enum Signal { meander, sin, triangle }

            #region Inspector
            public bool active;
            public float amplitude = 10f;
            public float frequency = 0.25f;
            public float offset = 0;
            public Signal signal;
            public Telemetry telemetry;
            #endregion

            private float _t = 0;

            public void Reset() { _t = 0; }
            public void SetPhase(float phase) { _t = phase; }
            public float GetPhase() { return _t; }

            public float NextValue(float dt)
            {
                float retValue = 0;
                switch (signal)
                {
                    case Signal.meander:
                        retValue = Mathf.Sin(2f * Mathf.PI * _t);
                        if (retValue != 0)
                            retValue = amplitude * (1f / retValue) * Mathf.Abs(retValue);
                        break;

                    case Signal.sin:
                        retValue = amplitude * Mathf.Sin(2f * Mathf.PI * _t);
                        break;

                    case Signal.triangle:
                        retValue = (2f * amplitude / Mathf.PI) * Mathf.Asin(Mathf.Sin(2f * Mathf.PI * _t));
                        break;
                }

                _t += dt * frequency;
                if (_t > 1f) _t -= 1f;

                return retValue + offset;
            }

            public void UpdateTelemetry(
                float time,
                float accAxis,
                float velMotor,
                float velAxis,
                float velTarget,
                float pos,
                float posDesire,
                float posTarget,
                float current,
                float currentTarget,
                float torque
            )
            {
                if (telemetry == null || telemetry.Count < 12)
                    return;

                telemetry[0].Add(accAxis);
                telemetry[1].Add(velMotor);
                telemetry[2].Add(velAxis);
                telemetry[3].Add(velTarget);
                telemetry[4].Add(velAxis - velTarget);
                telemetry[5].Add(pos);
                telemetry[6].Add(posDesire);
                telemetry[7].Add(posTarget);
                telemetry[8].Add(pos - posTarget);
                telemetry[9].Add(current);
                telemetry[10].Add(currentTarget);
                telemetry[11].Add(torque);
            }
        }

        public enum Type { none, current, velocity, position, positionCircular }

        #region Inspector
        [SerializeField]
        private float _desire;

        public Type type = Type.current;
        [Tooltip("Пропорциональный коэф. контура регулирования скорости.")]
        public float KP;
        [Tooltip("Интегральный коэф. контура регулирования скорости.")]
        public float KI;
        [Tooltip("Диффиренциальный коэф. контура регулирования скорости.")]
        public float KD;

        [System.Serializable]
        public class Gain
        {
            public float min;
            public float max;
            [Tooltip("Если значение ошибки будет больше этой величины, то значение Gain будет минимальным.")]
            public float err;
        }



        [Tooltip("Коэф. контура регулирования положения.")]
        public Gain gain;

        [SerializeField]
        private LimitControl limits = new LimitControl();

        /*
     [System.Serializable]
     public class LimitPos
     {
         public float Kpos;
         [Tooltip("Коэффициент пропорциональный контура скорости")]
         public float Kvel;
         [OdeReadOnly]
         public float targetCurrentLim;
     }
     */
        //   public LimitPos limPos = new LimitPos();

        [Space(10)]
        public Verification verification = new Verification();
        #endregion

        public LimitControl Limits { get { return limits; }}

        /// <summary>
        /// Задание для контура регулирования
        /// Единицы измерения зависят выбранного контура (Type)
        /// </summary>
        public float Desire { get { return _desire; } set { _desire = value; } }

        /// <summary>
        /// Значение Desire с учетом лимитов для контура регулирвания по положению
        /// </summary>
        public float TargetPosition { get { return _targetPosition; } }

        /// <summary>
        /// Флаг насыщения управляющего воздействия. 
        /// Служит для ограничения интегрирования. При необходимости, должен устанавливаться каждый раз после вызова CalcTarget.
        /// 
        /// Используется когда управляющее воздействие не оказывается влияние на объект регулирования.
        /// Например, управляющим воздействием является задание для контура регулирования по току.
        /// В этом случаем нет смысла увеличивать управляющее воздействие, когда ШИМ открыт полностю.
        /// </summary>
        public bool IsReached { get; set; }

        [Serializable]
        public class LimitControl
        {
            [Tooltip("Ограничение по скорости [градус/сек].")]
            public float limitVel = 360f;
            [Tooltip("Ограничение по ускорению [градус/сек^2].")]
            public float limitAcc = 100F;
            [Tooltip("Минимальная граница по величине [единицы].")]
            public float MinPos = -9999999999999999f;
            [Tooltip("Максимальная граница по величине [единицы].")]
            public float MaxPos = 9999999999999999f;

            private float _posReal;
            private float _velReal;
            private float _accReal;
            private float _posRealLast;
            private float _lastDesire;
            private bool _done;
            private bool _done2;

            internal float Solve(float dt, float desire)
            {
                desire = (desire > MaxPos) ? MaxPos : desire;
                desire = (desire < MinPos) ? MinPos : desire;

                if (_done && _lastDesire == desire)
                    return desire;

                _done = false;
                _done2 = false;
                _lastDesire = desire;

                float phalf = (desire - _posReal) / 2.0f;

                //ускорение в сторону задания в зависимости от того по какую сторону от задания мы находимся
                if (phalf > 0.0f)
                    _accReal = limitAcc;
                else
                    _accReal = -limitAcc;

                // прогнозируем когда будем торомозить на максимальном ускорении, только в том числе если мы приближаемся к заданию а не отдаляемся
                if (
                    (Mathf.Abs(desire - _posReal) < _velReal * _velReal / 2.0f / limitAcc + Mathf.Abs(_velReal) * dt) &&
                    (Mathf.Abs(desire - _posReal) - Mathf.Abs(desire - _posRealLast) < 0)
                    )
                {
                    if (phalf > 0.0f)
                        _accReal = -limitAcc;
                    else
                        _accReal = limitAcc;
                }

                _velReal += _accReal * dt;
                if (_velReal > limitVel)
                    _velReal = limitVel;
                if (_velReal < -limitVel)
                    _velReal = -limitVel;

                _posRealLast = _posReal;

                _posReal += _velReal * dt + _accReal * dt * dt / 2.0f;

                if (
                    ((_posReal - desire) > 0.0f && (_posRealLast - desire) < 0.0f)
                    ||
                    ((_posReal - desire) < 0.0f && (_posRealLast - desire) > 0.0f)
                    )
                {
                    _done2 = true;
                }

                //если пересекли задание и скорость упала до околонулевой то точка достигнута
                if ((Mathf.Abs(_velReal) < (limitAcc * dt * 2f)) && (_done2))
                {
                    _done = true;
                    _posReal = desire;
                    _velReal = 0;
                    _accReal = 0;
                }
                return _posReal;
            }
        }

        private float _errI;
        private float _errLast;
        private float _targetPosition;

        public void Reset()
        {
            _errI = 0;
            _errLast = 0;
            IsReached = false;
        }

        float DegNormalize(float value)
        {
            //преобразуем величину угла в целочисленное значение с точностью 0,001 градус
            long norm = (long)(value * 1000f);

            //преобразуем угол в диапозон от -180 до 180
            while (norm > 180000)
                norm -= 360000;
            while (norm < -180000)
                norm += 360000;

            return norm / 1000f;
        }

        public float CalcCurrent(IMotor motor)
        {
            if (verification.active)
                _desire = verification.NextValue(OdeWorld.Settings.stepTime);
            else
                verification.Reset();

            float targetCurrent = 0;
            float targetVelocity = 0;
            float targetPosition = 0;

            switch (type)
            {
                case Type.none:
                    Reset();
                    targetCurrent = 0;
                    break;

                case Type.current:
                    Reset();
                    targetCurrent = _desire;
                    break;

                default:
                    if (type == Type.position || type == Type.positionCircular)
                    {
                        // Пересчитываем задание с учетом периодичности круга
                        if (type == Type.positionCircular)
                            _desire = motor.AxisPosition + DegNormalize(DegNormalize(_desire) - DegNormalize(motor.AxisPosition));

                        //Пересчитываем задание с учетом лимитов ускорения и скорости
                        targetPosition = limits.Solve(OdeWorld.Settings.stepTime, _desire);
                        //  targetPosition = _desire;

                        //Пересчитываем задание по углу в задание по скорости
                        var errAbs = Mathf.Abs((targetPosition - motor.AxisPosition));
                        targetVelocity = (targetPosition - motor.AxisPosition) * Mathf.Clamp(gain.max * (errAbs < 0.01f ? 1 : gain.err / errAbs), gain.min, gain.max);
                        //targetVelocity = (targetPosition - motor.AxisPosition) * 100f;
                    }
                    else
                        targetVelocity = _desire;

                    // Проверяем задание на лимит по угловой скорости
                    if (Mathf.Abs(targetVelocity) > limits.limitVel)
                        targetVelocity = limits.limitVel * Mathf.Sign(targetVelocity);

                    // Расчитываем управляющее воздействие (ПИ регулятор скороости)
                    var E = (targetVelocity - motor.AxisVelocity);
                    var I = _errI + KI * E * OdeWorld.Settings.stepTime;
                    var D = (E - _errLast) / OdeWorld.Settings.stepTime;
                    targetCurrent = E * KP + I + D * KD;

                    if (IsReached == false)
                        _errI = I;

                    IsReached = false;
                    _errLast = E;
                    break;

            }

            /*
                        // Механизм выталкивания из области программного ограничения по положению

                        if (motor.AxisPosition > limits.MaxPos || motor.AxisPosition < limits.MinPos)
                        {
                            var targetLim = (motor.AxisPosition > limits.MaxPos) ? limits.MaxPos : limits.MinPos;

                            //Пересчитываем задание по углу в задание по скорости                        
                            targetVelocity = (targetLim - motor.AxisPosition) * limPos.Kpos;

                            var ELim = (targetVelocity - motor.AxisVelocity);
                            limPos.targetCurrentLim = ELim * limPos.Kvel;

                            IsReached = true;

                            if ((targetCurrent > limPos.targetCurrentLim && motor.AxisPosition > limits.MaxPos)
                                ||
                                (targetCurrent < limPos.targetCurrentLim && motor.AxisPosition < limits.MinPos))
                            {
                                targetCurrent = limPos.targetCurrentLim;
                            }
                        }
                        */

            _targetPosition = targetPosition;
            verification.UpdateTelemetry(
                OdeWorld.Statistics.time,
                motor.AxisAcceleration,
                motor.MotorVelocity,
                motor.AxisVelocity,
                targetVelocity,
                motor.AxisPosition,
                type == Type.position ? _desire : 0, TargetPosition,
                motor.Current,
                targetCurrent,
                motor.AxisTorque
                );

            return targetCurrent;
        }
    }
}