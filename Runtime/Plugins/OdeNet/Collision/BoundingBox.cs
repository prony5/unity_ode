using System;
using System.Globalization;
using System.Runtime.InteropServices;
#if ODE_DOUBLE_PRECISION
using dReal = System.Double;
#else
using dReal = System.Single;
#endif

namespace Ode.Net.Collision
{
    /// <summary>
    /// Represents an axis-aligned bounding box.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct BoundingBox : IEquatable<BoundingBox>
    {
        /// <summary>
        /// Specifies the minimum X coordinate of the axis-aligned bounding box.
        /// </summary>
        public dReal MinX;

        /// <summary>
        /// Specifies the maximum X coordinate of the axis-aligned bounding box.
        /// </summary>
        public dReal MaxX;

        /// <summary>
        /// Specifies the minimum Y coordinate of the axis-aligned bounding box.
        /// </summary>
        public dReal MinY;

        /// <summary>
        /// Specifies the maximum Y coordinate of the axis-aligned bounding box.
        /// </summary>
        public dReal MaxY;

        /// <summary>
        /// Specifies the minimum Z coordinate of the axis-aligned bounding box.
        /// </summary>
        public dReal MinZ;

        /// <summary>
        /// Specifies the maximum Z coordinate of the axis-aligned bounding box.
        /// </summary>
        public dReal MaxZ;

        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified
        /// <see cref="BoundingBox"/> value.
        /// </summary>
        /// <param name="other">A <see cref="BoundingBox"/> value to compare to this instance.</param>
        /// <returns>
        /// <b>true</b> if <paramref name="other"/> has the same minimum and maximum
        /// coordinates as this instance; otherwise, <b>false</b>.
        /// </returns>
        public bool Equals(BoundingBox other)
        {
            return MinX == other.MinX &&
                   MaxX == other.MaxX &&
                   MinY == other.MinY &&
                   MaxY == other.MaxY &&
                   MinZ == other.MinZ &&
                   MaxZ == other.MaxZ;
        }

        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified object.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        /// <returns>
        /// <b>true</b> if <paramref name="obj"/> is an instance of <see cref="BoundingBox"/> and
        /// has the same minimum and maximum coordinates as this instance; otherwise, <b>false</b>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj is BoundingBox)
            {
                return Equals((BoundingBox)obj);
            }

            return false;
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode()
        {
            return MinX.GetHashCode() ^
                   MaxX.GetHashCode() ^
                   MinY.GetHashCode() ^
                   MaxY.GetHashCode() ^
                   MinZ.GetHashCode() ^
                   MaxZ.GetHashCode();
        }

        /// <summary>
        /// Converts the numeric value of this instance to its equivalent string representation.
        /// </summary>
        /// <returns>
        /// The string representation of the minimum and maximum coordinates of this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                "{{Min:{0} Max:{1}}}",
                new Vector3(MinX, MinY, MinZ),
                new Vector3(MaxX, MaxY, MaxZ));
        }

        /// <summary>
        /// Tests whether two <see cref="BoundingBox"/> structures are equal.
        /// </summary>
        /// <param name="left">
        /// The <see cref="BoundingBox"/> structure on the left of the equality operator.
        /// </param>
        /// <param name="right">
        /// The <see cref="BoundingBox"/> structure on the right of the equality operator.
        /// </param>
        /// <returns>
        /// <b>true</b> if <paramref name="left"/> and <paramref name="right"/> have
        /// equal minimum and maximum coordinates; otherwise, <b>false</b>.
        /// </returns>
        public static bool operator ==(BoundingBox left, BoundingBox right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Tests whether two <see cref="BoundingBox"/> structures are different.
        /// </summary>
        /// <param name="left">
        /// The <see cref="BoundingBox"/> structure on the left of the inequality operator.
        /// </param>
        /// <param name="right">
        /// The <see cref="BoundingBox"/> structure on the right of the inequality operator.
        /// </param>
        /// <returns>
        /// <b>true</b> if <paramref name="left"/> and <paramref name="right"/> differ
        /// in their minimum and maximum coordinates; <b>false</b> if <paramref name="left"/>
        /// and <paramref name="right"/> are equal.
        /// </returns>
        public static bool operator !=(BoundingBox left, BoundingBox right)
        {
            return !left.Equals(right);
        }
    }
}
