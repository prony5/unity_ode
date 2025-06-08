using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityODE
{
    public static class OdeUtils
    {
        public static Ode.Net.Vector3 ToODE(this Vector3 value)
        {
            return new Ode.Net.Vector3(value.x, value.y, value.z);
        }

        public static Vector3 ToUnity(this Ode.Net.Vector3 value)
        {
            return new Vector3((float)value.X, (float)value.Y, (float)value.Z);
        }

        public static Quaternion ToUnity(this Ode.Net.Quaternion value)
        {
            return new Quaternion((float)value.X, (float)value.Y, (float)value.Z, (float)value.W);
        }

        public static Ode.Net.Quaternion ToODE(this Quaternion value)
        {
            return new Ode.Net.Quaternion(value.w, value.x, value.y, value.z);
        }

        public static Vector3 DivideVectors(Vector3 a, Vector3 b)
        {
            if (b.x == 0 || b.y == 0 || b.z == 0)
                return Vector3.zero;
            else
                return new Vector3(a.x / b.x, a.y / b.y, a.z / b.z);
        }

        public static Vector3 Abs(Vector3 value)
        {
            return new Vector3(Mathf.Abs(value.x), Mathf.Abs(value.y), Mathf.Abs(value.z));
        }

        public static Vector3 RotateAroundPivot(Vector3 point, Vector3 pivot, Quaternion rotation)
        {
            Vector3 dir = point - pivot;
            dir = rotation * dir;
            return dir + pivot;
        }

        public static float GetAngle(Quaternion q, Vector3 axis)
        {
            float a = Quaternion.Angle(Quaternion.identity, Quaternion.FromToRotation(q * axis, axis) * q);
            Vector3 secondaryAxis = new Vector3(axis.z, axis.x, axis.y);
            Vector3 cross = Vector3.Cross(secondaryAxis, axis);
            return (Vector3.Dot(q * secondaryAxis, cross) > 0f) ? -a : a;
        }
    }

}
