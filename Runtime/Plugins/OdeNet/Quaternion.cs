using System;
using System.Globalization;
using System.Runtime.InteropServices;
#if ODE_DOUBLE_PRECISION
using dReal = System.Double;
#else
using dReal = System.Single;
#endif

namespace Ode.Net
{
    /// <summary>
    /// Represents a four-dimensional vector (w,x,y,z) used to efficiently
    /// compute angle rotations about the vector axis (x,y,z).
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Quaternion : IEquatable<Quaternion>
    {
        /// <summary>
        /// Represents a <see cref="Quaternion"/> specifying no rotation.
        /// </summary>
        public static readonly Quaternion Identity = new Quaternion(1, 0, 0, 0);

        /// <summary>
        /// Specifies the rotation component of the quaternion.
        /// </summary>
        public dReal W;

        /// <summary>
        /// Specifies the x-value of the vector component of the quaternion.
        /// </summary>
        public dReal X;

        /// <summary>
        /// Specifies the y-value of the vector component of the quaternion.
        /// </summary>
        public dReal Y;

        /// <summary>
        /// Specifies the z-value of the vector component of the quaternion.
        /// </summary>
        public dReal Z;

        /// <summary>
        /// Initializes a new instance of the <see cref="Quaternion"/> structure
        /// with the specified component values.
        /// </summary>
        /// <param name="w">The value of the rotation component of the quaternion.</param>
        /// <param name="x">The value of the x-component of the quaternion.</param>
        /// <param name="y">The value of the y-component of the quaternion.</param>
        /// <param name="z">The value of the z-component of the quaternion.</param>
        public Quaternion(dReal w, dReal x, dReal y, dReal z)
        {
            W = w;
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified
        /// <see cref="Quaternion"/> value.
        /// </summary>
        /// <param name="other">A <see cref="Quaternion"/> value to compare to this instance.</param>
        /// <returns>
        /// <b>true</b> if <paramref name="other"/> has the same W, X, Y and Z components as
        /// this instance; otherwise, <b>false</b>.
        /// </returns>
        public bool Equals(Quaternion other)
        {
            return W == other.W && X == other.X && Y == other.Y && Z == other.Z;
        }

        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified object.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        /// <returns>
        /// <b>true</b> if <paramref name="obj"/> is an instance of <see cref="Quaternion"/> and
        /// has the same W, X, Y and Z components as this instance; otherwise, <b>false</b>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj is Quaternion)
            {
                return Equals((Quaternion)obj);
            }

            return false;
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode()
        {
            return W.GetHashCode() ^ X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode();
        }

        /// <summary>
        /// Converts the numeric value of this instance to its equivalent string representation.
        /// </summary>
        /// <returns>
        /// The string representation of the W, X, Y and Z components of this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "({0}, ({1}, {2}, {3}))", W, X, Y, Z);
        }

        /// <summary>
        /// Tests whether two <see cref="Quaternion"/> structures are equal.
        /// </summary>
        /// <param name="left">
        /// The <see cref="Quaternion"/> structure on the left of the equality operator.
        /// </param>
        /// <param name="right">
        /// The <see cref="Quaternion"/> structure on the right of the equality operator.
        /// </param>
        /// <returns>
        /// <b>true</b> if <paramref name="left"/> and <paramref name="right"/> have
        /// equal W, X, Y and Z components; otherwise, <b>false</b>.
        /// </returns>
        public static bool operator ==(Quaternion left, Quaternion right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Tests whether two <see cref="Quaternion"/> structures are different.
        /// </summary>
        /// <param name="left">
        /// The <see cref="Quaternion"/> structure on the left of the inequality operator.
        /// </param>
        /// <param name="right">
        /// The <see cref="Quaternion"/> structure on the right of the inequality operator.
        /// </param>
        /// <returns>
        /// <b>true</b> if <paramref name="left"/> and <paramref name="right"/> differ
        /// in W, X, Y or Z components; <b>false</b> if <paramref name="left"/> and
        /// <paramref name="right"/> are equal.
        /// </returns>
        public static bool operator !=(Quaternion left, Quaternion right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// Creates a Quaternion using the specified axis and angle
        /// </summary>
        public static Quaternion FromAxisAndAngle(Vector3 axis, dReal angle)
        {
            Quaternion o;
            Native.NativeMethods.dQFromAxisAndAngle(out o, axis.X, axis.Y, axis.Z, angle);
            return o;
        }
    }
}
