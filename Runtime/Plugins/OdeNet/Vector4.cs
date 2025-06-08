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
    /// Represents a vector with four components.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector4 : IEquatable<Vector4>
    {
        /// <summary>
        /// Represents a <see cref="Vector4"/> with all of its components set to zero.
        /// </summary>
        public static readonly Vector4 Zero = new Vector4(0, 0, 0, 0);

        /// <summary>
        /// Specifies the x-component of the vector.
        /// </summary>
        public dReal X;

        /// <summary>
        /// Specifies the y-component of the vector.
        /// </summary>
        public dReal Y;

        /// <summary>
        /// Specifies the z-component of the vector.
        /// </summary>
        public dReal Z;

        /// <summary>
        /// Specifies the w-component of the vector.
        /// </summary>
        public dReal W;

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector4"/> structure
        /// with all components set to the same value.
        /// </summary>
        /// <param name="value">The value to initialize each component to.</param>
        public Vector4(dReal value)
        {
            X = Y = Z = W = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector4"/> structure
        /// with the specified component values.
        /// </summary>
        /// <param name="x">The value of the x-component of the vector.</param>
        /// <param name="y">The value of the y-component of the vector.</param>
        /// <param name="z">The value of the z-component of the vector.</param>
        /// <param name="w">The value of the w-component of the vector.</param>
        public Vector4(dReal x, dReal y, dReal z, dReal w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified
        /// <see cref="Vector4"/> value.
        /// </summary>
        /// <param name="other">A <see cref="Vector4"/> value to compare to this instance.</param>
        /// <returns>
        /// <b>true</b> if <paramref name="other"/> has the same X, Y, Z and W components as
        /// this instance; otherwise, <b>false</b>.
        /// </returns>
        public bool Equals(Vector4 other)
        {
            return X == other.X && Y == other.Y && Z == other.Z && W == other.W;
        }

        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified object.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        /// <returns>
        /// <b>true</b> if <paramref name="obj"/> is an instance of <see cref="Vector4"/> and
        /// has the same X, Y, Z and W components as this instance; otherwise, <b>false</b>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj is Vector4)
            {
                return Equals((Vector4)obj);
            }
            
            return false;
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode() ^ W.GetHashCode();
        }

        /// <summary>
        /// Converts the numeric value of this instance to its equivalent string representation.
        /// </summary>
        /// <returns>
        /// The string representation of the X, Y, Z and W components of this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "({0}, {1}, {2}, {3})", X, Y, Z, W);
        }

        /// <summary>
        /// Tests whether two <see cref="Vector4"/> structures are equal.
        /// </summary>
        /// <param name="left">
        /// The <see cref="Vector4"/> structure on the left of the equality operator.
        /// </param>
        /// <param name="right">
        /// The <see cref="Vector4"/> structure on the right of the equality operator.
        /// </param>
        /// <returns>
        /// <b>true</b> if <paramref name="left"/> and <paramref name="right"/> have
        /// equal X, Y, Z and W components; otherwise, <b>false</b>.
        /// </returns>
        public static bool operator ==(Vector4 left, Vector4 right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Tests whether two <see cref="Vector4"/> structures are different.
        /// </summary>
        /// <param name="left">
        /// The <see cref="Vector4"/> structure on the left of the inequality operator.
        /// </param>
        /// <param name="right">
        /// The <see cref="Vector4"/> structure on the right of the inequality operator.
        /// </param>
        /// <returns>
        /// <b>true</b> if <paramref name="left"/> and <paramref name="right"/> differ
        /// in X, Y, Z or W components; <b>false</b> if <paramref name="left"/> and
        /// <paramref name="right"/> are equal.
        /// </returns>
        public static bool operator !=(Vector4 left, Vector4 right)
        {
            return !left.Equals(right);
        }
    }
}
