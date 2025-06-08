using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityODE
{
    [AddComponentMenu("ODE/Sensors/IMU")]
    [RequireComponent(typeof(OdeBody))]
    public class OdeIMU : OdeSensor
    {
        #region Inspector    
        public Vector3 refPoint;

        [System.Serializable]
        public class EulerDirection
        {
            public enum Axis { x, y, z }
            public Axis axis;
            public bool invertRotation;
            public bool invertMove;

            public EulerDirection(Axis axis, bool invertRotation = false, bool invertMove = false)
            {
                this.axis = axis;
                this.invertRotation = invertRotation;
                this.invertMove = invertMove;
            }

            public Vector3 ToVector()
            {
                switch (axis)
                {
                    case Axis.x:
                        return new Vector3(1, 0, 0);
                    case Axis.y:
                        return new Vector3(0, 1, 0);
                    default:
                        return new Vector3(0, 0, 1);
                }
            }

            public Vector3 ToVectorR()
            {
                switch (axis)
                {
                    case Axis.x:
                        return invertRotation ? new Vector3(-1, 0, 0) : new Vector3(1, 0, 0);
                    case Axis.y:
                        return invertRotation ? new Vector3(0, -1, 0) : new Vector3(0, 1, 0);
                    default:
                        return invertRotation ? new Vector3(0, 0, -1) : new Vector3(0, 0, 1);
                }
            }

            public Vector3 ToVectorM()
            {
                switch (axis)
                {
                    case Axis.x:
                        return invertMove ? new Vector3(-1, 0, 0) : new Vector3(1, 0, 0);
                    case Axis.y:
                        return invertMove ? new Vector3(0, -1, 0) : new Vector3(0, 1, 0);
                    default:
                        return invertMove ? new Vector3(0, 0, -1) : new Vector3(0, 0, 1);
                }
            }
        }

        [Header("Directions")]
        public EulerDirection pitch = new EulerDirection(EulerDirection.Axis.x);
        public EulerDirection yaw = new EulerDirection(EulerDirection.Axis.y);
        public EulerDirection roll = new EulerDirection(EulerDirection.Axis.z);

        [System.Serializable]
        public class Ypr
        {
            public float yaw;
            public float pitch;
            public float roll;

            public Ypr(float y, float p, float r)
            {
                yaw = y;
                pitch = p;
                roll = r;
            }

            public Ypr(Quaternion q)
            {
                var e = q.eulerAngles;
                yaw = e.y > 180 ? e.y - 360 : e.y;
                pitch = e.x > 180 ? e.x - 360 : e.x;
                roll = e.z > 180 ? e.z - 360 : e.z;
            }
        }

        [Header("Values")]
        [OdeReadOnly]
        public Ypr ypr;

        [OdeReadOnly]
        public Vector3 acc;

        [OdeReadOnly]
        public Vector3 gyro;

        [OdeReadOnly]
        public Quaternion q = Quaternion.identity;

        [Range(0, 100)]
        public float filter;
        #endregion

        [HideInInspector]
        public Quaternion defaultRotation = Quaternion.identity;

        private bool _inited;
        private OdeBody _ubody;
        private OdeFilter _filter;
        private float _g;

        private void OnDisable()
        {
            if (_inited)
            {
                _inited = false;
                OdeWorld.OnAfterStep -= AfterSimStep;
            }
        }

        void Start()
        {
            _g = OdeWorld.Settings.gravity.magnitude; if (_g > 0) _g = 1 / _g;
            _filter = new OdeFilter(6, OdeWorld.Settings.stepTime);
            _values = new float[13];
            _valuesID = new string[13] { "Yaw", "Pitch", "Roll", "AccX", "AccY", "AccZ", "GyroX", "GyroY", "GyroZ", "q1", "q2", "q3", "qw" };

            _ubody = GetComponent<OdeBody>();

            OdeWorld.OnAfterStep += AfterSimStep;
            _inited = true;
        }

        void Update()
        {
            _filter.tau = filter;
        }

        private Vector3 _lastVel;

        void AfterSimStep()
        {
            if (_ubody == null || _ubody.Inited == false)
                return;

            var _body = _ubody.Body;

            //calc acc   
            Vector3 _acc;
            var _vel = _body.GetRelativePointVelocity(refPoint.ToODE()).ToUnity();
            if (float.IsInfinity(_vel.x) || float.IsInfinity(_vel.y) || float.IsInfinity(_vel.z) || float.IsNaN(_vel.x) || float.IsNaN(_vel.y) || float.IsNaN(_vel.z))
                return;

            if (filter > 0)
            {
                double x;
                double y;
                double z;

                _filter.Calc(_vel.x, out x, 0);
                _filter.Calc(_vel.y, out y, 1);
                _filter.Calc(_vel.z, out z, 2);
                _acc = new Vector3((float)x, (float)y, (float)z);
            }
            else
            {
                _acc = (_vel - _lastVel) / OdeWorld.Settings.stepTime;
                _lastVel = _vel;
            }
            //add gravity
            _acc -= OdeWorld.Settings.gravity;

            //calc gyro
            var _gyro = _body.AngularVelocity.ToUnity() * Mathf.Rad2Deg;
            if (filter > 0)
            {
                _gyro.x = (float)_filter.Calc(_gyro.x, 3);
                _gyro.y = (float)_filter.Calc(_gyro.y, 4);
                _gyro.z = (float)_filter.Calc(_gyro.z, 5);
            }

            //convert m/sec^2 to g
            _acc = _g > 0 ? _acc * _g : Vector3.zero;

            //Get Yaw Pitch Roll
            ypr = new Ypr(_body.Quaternion.ToUnity() * Quaternion.Inverse(defaultRotation));
            //Get Rotation
            q = Quaternion.AngleAxis(ypr.yaw, yaw.ToVectorR()) * Quaternion.AngleAxis(ypr.pitch, pitch.ToVectorR()) * Quaternion.AngleAxis(ypr.roll, roll.ToVectorR());
            //Get Acc
            acc = pitch.ToVectorM() * Vector3.Dot(_acc, new Vector3(1, 0, 0)) + yaw.ToVectorM() * Vector3.Dot(_acc, new Vector3(0, 1, 0)) + roll.ToVectorM() * Vector3.Dot(_acc, new Vector3(0, 0, 1));
            //Get Gyro
            gyro = pitch.ToVectorR() * Vector3.Dot(_gyro, new Vector3(1, 0, 0)) + yaw.ToVectorR() * Vector3.Dot(_gyro, new Vector3(0, 1, 0)) + roll.ToVectorR() * Vector3.Dot(_gyro, new Vector3(0, 0, 1));

            //Apply Invert
            if (yaw.invertRotation) ypr.yaw = -ypr.yaw;
            if (pitch.invertRotation) ypr.pitch = -ypr.pitch;
            if (roll.invertRotation) ypr.roll = -ypr.roll;

            _values[0] = ypr.yaw;
            _values[1] = ypr.pitch;
            _values[2] = ypr.roll;
            _values[3] = acc.x;
            _values[4] = acc.y;
            _values[5] = acc.z;
            _values[6] = gyro.x;
            _values[7] = gyro.y;
            _values[8] = gyro.z;
            _values[9] = q.x;
            _values[10] = q.y;
            _values[11] = q.z;
            _values[12] = q.w;
        }

        float VS(Vector3 a, Vector3 b) { return a.x * b.x + a.y * b.y + a.z * b.z; }
    }

}
