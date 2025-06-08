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
    /// Represents the properties of colliding surfaces.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SurfaceParameters : IEquatable<SurfaceParameters>
    {
        /// <summary>
        /// Specifies the contact modes for the surface.
        /// </summary>
        public ContactModes Mode;

        /// <summary>
        /// Specifies the Coulomb friction coefficient of the surface.
        /// </summary>
        /// <remarks>
        /// This must be in the range 0 to Infinity. 0 results in a frictionless
        /// contact, and dInfinity results in a contact that never slips. Note
        /// that frictionless contacts are less time consuming to compute than
        /// ones with friction, and infinite friction contacts can be cheaper
        /// than contacts with finite friction. This must always be set.
        /// </remarks>
        public dReal Mu;

        /// <summary>
        /// Specifies the optional Coulomb friction coefficient for friction direction 2.
        /// </summary>
        /// <remarks>
        /// This is only used if the corresponding mode is set.
        /// </remarks>
        public dReal Mu2;

        /// <summary>
        /// Specifies the rolling friction coefficient around direction 1.
        /// </summary>
        public dReal Rho;

        /// <summary>
        /// Specifies the rolling friction coefficient around direction 2.
        /// </summary>
        public dReal Rho2;

        /// <summary>
        /// Specifies the rolling friction coefficient around the normal direction.
        /// </summary>
        public dReal RhoN;

        /// <summary>
        /// Specifies the restitution parameter for the surface. 0 means the surface is
        /// not bouncy at all, 1 is maximum bouncyness.
        /// </summary>
        /// <remarks>
        /// This is only used if the corresponding mode is set.
        /// </remarks>
        public dReal Bounce;

        /// <summary>
        /// Specifies the minimum incoming velocity necessary for bounce.
        /// </summary>
        /// <remarks>
        /// Incoming velocities below this will effectively have a bounce
        /// parameter of 0. This is only used if the corresponding mode is set.
        /// </remarks>
        public dReal BounceVelocity;

        /// <summary>
        /// Specifies the contact normal ``softness'' parameter.
        /// </summary>
        /// <remarks>
        /// This is only used if the corresponding mode is set.
        /// </remarks>
        public dReal SoftErp;

        /// <summary>
        /// Specifies the contact normal ``softness'' parameter.
        /// </summary>
        /// <remarks>
        /// This is only used if the corresponding mode is set.
        /// </remarks>
        public dReal SoftCfm;

        /// <summary>
        /// Specifies the surface velocity in friction direction 1.
        /// </summary>
        /// <remarks>
        /// This is only used if the corresponding mode is set.
        /// </remarks>
        public dReal Motion1;

        /// <summary>
        /// Specifies the surface velocity in friction direction 2.
        /// </summary>
        /// <remarks>
        /// This is only used if the corresponding mode is set.
        /// </remarks>
        public dReal Motion2;

        /// <summary>
        /// Specifies the surface velocity in the direction of the contact normal.
        /// </summary>
        /// <remarks>
        /// This is only used if the corresponding mode is set.
        /// </remarks>
        public dReal MotionN;

        /// <summary>
        /// Specifies the coefficient of force-dependent-slip (FDS) for friction direction 1.
        /// </summary>
        /// <remarks>
        /// This is only used if the corresponding mode is set.
        /// </remarks>
        public dReal Slip1;

        /// <summary>
        /// Specifies the coefficient of force-dependent-slip (FDS) for friction direction 2.
        /// </summary>
        /// <remarks>
        /// This is only used if the corresponding mode is set.
        /// </remarks>
        public dReal Slip2;

        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified
        /// <see cref="SurfaceParameters"/> value.
        /// </summary>
        /// <param name="other">A <see cref="SurfaceParameters"/> value to compare to this instance.</param>
        /// <returns>
        /// <b>true</b> if <paramref name="other"/> has the same parameters
        /// as this instance; otherwise, <b>false</b>.
        /// </returns>
        public bool Equals(SurfaceParameters other)
        {
            return Mode == other.Mode &&
                   Mu == other.Mu &&
                   Mu2 == other.Mu2 &&
                   Rho == other.Rho &&
                   Rho2 == other.Rho2 &&
                   RhoN == other.RhoN &&
                   Bounce == other.Bounce &&
                   BounceVelocity == other.BounceVelocity &&
                   SoftErp == other.SoftErp &&
                   SoftCfm == other.SoftCfm &&
                   Motion1 == other.Motion1 &&
                   Motion2 == other.Motion2 &&
                   MotionN == other.MotionN &&
                   Slip1 == other.Slip1 &&
                   Slip2 == other.Slip2;
        }

        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified object.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        /// <returns>
        /// <b>true</b> if <paramref name="obj"/> is an instance of <see cref="SurfaceParameters"/> and
        /// has the same parameters as this instance; otherwise, <b>false</b>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj is SurfaceParameters)
            {
                return Equals((SurfaceParameters)obj);
            }

            return false;
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode()
        {
            return Mode.GetHashCode() ^
                   Mu.GetHashCode() ^
                   Mu2.GetHashCode() ^
                   Rho.GetHashCode() ^
                   Rho2.GetHashCode() ^
                   RhoN.GetHashCode() ^
                   Bounce.GetHashCode() ^
                   BounceVelocity.GetHashCode() ^
                   SoftErp.GetHashCode() ^
                   SoftCfm.GetHashCode() ^
                   Motion1.GetHashCode() ^
                   Motion2.GetHashCode() ^
                   MotionN.GetHashCode() ^
                   Slip1.GetHashCode() ^
                   Slip2.GetHashCode();
        }

        /// <summary>
        /// Converts the numeric value of this instance to its equivalent string representation.
        /// </summary>
        /// <returns>
        /// The string representation of the surface parameters of this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                "{{Mode:{0}}}",Mode);
        }

        /// <summary>
        /// Tests whether two <see cref="SurfaceParameters"/> structures are equal.
        /// </summary>
        /// <param name="left">
        /// The <see cref="SurfaceParameters"/> structure on the left of the equality operator.
        /// </param>
        /// <param name="right">
        /// The <see cref="SurfaceParameters"/> structure on the right of the equality operator.
        /// </param>
        /// <returns>
        /// <b>true</b> if <paramref name="left"/> and <paramref name="right"/> have
        /// equal surface parameters; otherwise, <b>false</b>.
        /// </returns>
        public static bool operator ==(SurfaceParameters left, SurfaceParameters right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Tests whether two <see cref="SurfaceParameters"/> structures are different.
        /// </summary>
        /// <param name="left">
        /// The <see cref="SurfaceParameters"/> structure on the left of the inequality operator.
        /// </param>
        /// <param name="right">
        /// The <see cref="SurfaceParameters"/> structure on the right of the inequality operator.
        /// </param>
        /// <returns>
        /// <b>true</b> if <paramref name="left"/> and <paramref name="right"/> differ
        /// in their surface parameters; <b>false</b> if <paramref name="left"/>
        /// and <paramref name="right"/> are equal.
        /// </returns>
        public static bool operator !=(SurfaceParameters left, SurfaceParameters right)
        {
            return !left.Equals(right);
        }
    }
}
