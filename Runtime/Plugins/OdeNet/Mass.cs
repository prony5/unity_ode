using Ode.Net.Collision;
using Ode.Net.Native;
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
    /// Represents a rigid body mass.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Mass : IEquatable<Mass>
    {
        /// <summary>
        /// Specifies the total mass of the rigid body.
        /// </summary>
        public dReal TotalMass;

        /// <summary>
        /// Specifies the position of the center of gravity in body frame.
        /// </summary>
        public Vector3 Center;

        /// <summary>
        /// Specifies the 3x3 inertia tensor in body frame, about the
        /// point of reference.
        /// </summary>
        public Matrix3 Inertia;

        /// <summary>
        /// Initializes a new instance of the <see cref="Mass"/> structure with the
        /// specified parameters.
        /// </summary>
        /// <param name="totalMass">The total mass of the rigid body.</param>
        /// <param name="center">The position of the center of gravity in body frame.</param>
        /// <param name="i11">The value of the first diagonal element of the inertia matrix.</param>
        /// <param name="i22">The value of the second diagonal element of the inertia matrix.</param>
        /// <param name="i33">The value of the third diagonal element of the inertia matrix.</param>
        /// <param name="i12">The value of the first antidiagonal of the inertia matrix.</param>
        /// <param name="i13">The value of the second antidiagonal of the inertia matrix.</param>
        /// <param name="i23">The value of the third antidiagonal of the inertia matrix.</param>
        public Mass(
            dReal totalMass, Vector3 center,
            dReal i11, dReal i22, dReal i33,
            dReal i12, dReal i13, dReal i23)
        {
            NativeMethods.dMassSetParameters(out this, totalMass,
                center.X, center.Y, center.Z,
                i11, i22, i33, i12, i13, i23);
        }

        /// <summary>
        /// Gets a value indicating whether the mass parameters are valid.
        /// </summary>
        public bool IsValid
        {
            get { return NativeMethods.dMassCheck(ref this) != 0; }
        }

        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified
        /// <see cref="Mass"/> value.
        /// </summary>
        /// <param name="other">A <see cref="Mass"/> value to compare to this instance.</param>
        /// <returns>
        /// <b>true</b> if <paramref name="other"/> has the same mass, center of
        /// gravity and inertia matrix as this instance; otherwise, <b>false</b>.
        /// </returns>
        public bool Equals(Mass other)
        {
            return TotalMass == other.TotalMass && Center == other.Center && Inertia == other.Inertia;
        }

        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified object.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        /// <returns>
        /// <b>true</b> if <paramref name="obj"/> is an instance of <see cref="Mass"/> and
        /// has the same mass, center of gravity and inertia matrix as this instance;
        /// otherwise, <b>false</b>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj is Mass)
            {
                return Equals((Mass)obj);
            }

            return false;
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode()
        {
            return TotalMass.GetHashCode() ^ Center.GetHashCode() ^ Inertia.GetHashCode();
        }

        /// <summary>
        /// Converts the numeric value of this instance to its equivalent string representation.
        /// </summary>
        /// <returns>
        /// The string representation of the mass, center of gravity and inertia matrix
        /// of this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                "{{TotalMass: {0}, Center: {1}, Inertia: {2}}}",
                TotalMass, Center, Inertia);
        }

        /// <summary>
        /// Adjusts the mass parameters so that the total mass is now <paramref name="newMass"/>.
        /// </summary>
        /// <param name="mass">The mass to adjust.</param>
        /// <param name="newMass">The new total mass of the rigid body.</param>
        /// <returns>The adjusted mass.</returns>
        public static Mass Adjust(Mass mass, dReal newMass)
        {
            NativeMethods.dMassAdjust(ref mass, newMass);
            return mass;
        }

        /// <summary>
        /// Adjusts the mass parameters so that the total mass is now <paramref name="newMass"/>.
        /// </summary>
        /// <param name="mass">The mass to adjust.</param>
        /// <param name="newMass">The new total mass of the rigid body.</param>
        /// <param name="result">The adjusted mass.</param>
        public static void Adjust(ref Mass mass, dReal newMass, out Mass result)
        {
            result = mass;
            NativeMethods.dMassAdjust(ref result, newMass);
        }

        /// <summary>
        /// Adjusts the mass parameters to represent a mass object displaced by the specified
        /// translation relative to the body frame.
        /// </summary>
        /// <param name="mass">The mass to translate.</param>
        /// <param name="translation">The displacement vector in body frame.</param>
        /// <returns>The displaced mass.</returns>
        public static Mass Translate(Mass mass, Vector3 translation)
        {
            NativeMethods.dMassTranslate(ref mass, translation.X, translation.Y, translation.Z);
            return mass;
        }

        /// <summary>
        /// Adjusts the mass parameters to represent a mass object displaced by the specified
        /// translation relative to the body frame.
        /// </summary>
        /// <param name="mass">The mass to translate.</param>
        /// <param name="translation">The displacement vector in body frame.</param>
        /// <param name="result">The displaced mass.</param>
        public static void Translate(ref Mass mass, ref Vector3 translation, out Mass result)
        {
            result = mass;
            NativeMethods.dMassTranslate(ref result, translation.X, translation.Y, translation.Z);
        }

        /// <summary>
        /// Adjusts the mass parameters to represent a mass object rotated by the specified
        /// rotation relative to the body frame.
        /// </summary>
        /// <param name="mass">The mass to rotate.</param>
        /// <param name="rotation">The rotation matrix in body frame.</param>
        /// <returns>The rotated mass.</returns>
        public static Mass Rotate(Mass mass, Matrix3 rotation)
        {
            NativeMethods.dMassRotate(ref mass, ref rotation);
            return mass;
        }

        /// <summary>
        /// Adjusts the mass parameters to represent a mass object rotated by the specified
        /// rotation relative to the body frame.
        /// </summary>
        /// <param name="mass">The mass to rotate.</param>
        /// <param name="rotation">The rotation matrix in body frame.</param>
        /// <param name="result">The rotated mass.</param>
        public static void Rotate(ref Mass mass, ref Matrix3 rotation, out Mass result)
        {
            result = mass;
            NativeMethods.dMassRotate(ref result, ref rotation);
        }

        /// <summary>
        /// Adds two <see cref="Mass"/> structures and returns the result as a new
        /// <see cref="Mass"/>.
        /// </summary>
        /// <param name="mass1">The first <see cref="Mass"/> to add.</param>
        /// <param name="mass2">The second <see cref="Mass"/> to add.</param>
        /// <returns>The sum of the two masses.</returns>
        public static Mass Add(Mass mass1, Mass mass2)
        {
            NativeMethods.dMassAdd(ref mass1, ref mass2);
            return mass1;
        }

        /// <summary>
        /// Adds two <see cref="Mass"/> structures.
        /// </summary>
        /// <param name="mass1">The first <see cref="Mass"/> to add.</param>
        /// <param name="mass2">The second <see cref="Mass"/> to add.</param>
        /// <param name="result">The sum of the two masses.</param>
        public static void Add(ref Mass mass1, ref Mass mass2, out Mass result)
        {
            result = mass1;
            NativeMethods.dMassAdd(ref result, ref mass2);
        }

        /// <summary>
        /// Creates a zero mass, with center at the origin of the body frame.
        /// </summary>
        /// <param name="result">The created mass.</param>
        public static void CreateZero(out Mass result)
        {
            NativeMethods.dMassSetZero(out result);
        }

        /// <summary>
        /// Creates a zero mass, with center at the origin of the body frame.
        /// </summary>
        /// <returns>The created mass.</returns>
        public static Mass CreateZero()
        {
            Mass result;
            CreateZero(out result);
            return result;
        }

        /// <summary>
        /// Creates a spherical mass of the specified density and radius, with
        /// center at the origin of the body frame.
        /// </summary>
        /// <param name="density">The density of the sphere.</param>
        /// <param name="radius">The radius of the sphere.</param>
        /// <param name="result">The created mass.</param>
        public static void CreateSphere(dReal density, dReal radius, out Mass result)
        {
            NativeMethods.dMassSetSphere(out result, density, radius);
        }

        /// <summary>
        /// Creates a spherical mass of the specified density and radius, with
        /// center at the origin of the body frame.
        /// </summary>
        /// <param name="density">The density of the sphere.</param>
        /// <param name="radius">The radius of the sphere.</param>
        /// <returns>The created mass.</returns>
        public static Mass CreateSphere(dReal density, dReal radius)
        {
            Mass result;
            CreateSphere(density, radius, out result);
            return result;
        }

        /// <summary>
        /// Creates a spherical mass of the specified total mass and radius, with
        /// center at the origin of the body frame.
        /// </summary>
        /// <param name="totalMass">The total mass of the sphere.</param>
        /// <param name="radius">The radius of the sphere.</param>
        /// <param name="result">The created mass.</param>
        public static void CreateSphereTotal(dReal totalMass, dReal radius, out Mass result)
        {
            NativeMethods.dMassSetSphereTotal(out result, totalMass, radius);
        }

        /// <summary>
        /// Creates a spherical mass of the specified total mass and radius, with
        /// center at the origin of the body frame.
        /// </summary>
        /// <param name="totalMass">The total mass of the sphere.</param>
        /// <param name="radius">The radius of the sphere.</param>
        /// <returns>The created mass.</returns>
        public static Mass CreateSphereTotal(dReal totalMass, dReal radius)
        {
            Mass result;
            CreateSphereTotal(totalMass, radius, out result);
            return result;
        }

        /// <summary>
        /// Creates a capsule mass of the specified parameters and density, with
        /// center at the origin of the body frame.
        /// </summary>
        /// <param name="density">The density of the capsule.</param>
        /// <param name="direction">The orientation of the cylinder's long axis.</param>
        /// <param name="radius">The radius of the cylinder (and the spherical cap).</param>
        /// <param name="length">The length of the cylinder (not counting the spherical cap).</param>
        /// <param name="result">The created mass.</param>
        public static void CreateCapsule(
            dReal density, DirectionAxis direction,
            dReal radius, dReal length, out Mass result)
        {
            NativeMethods.dMassSetCapsule(out result, density, direction, radius, length);
        }

        /// <summary>
        /// Creates a capsule mass of the specified parameters and density, with
        /// center at the origin of the body frame.
        /// </summary>
        /// <param name="density">The density of the capsule.</param>
        /// <param name="direction">The orientation of the cylinder's long axis.</param>
        /// <param name="radius">The radius of the cylinder (and the spherical cap).</param>
        /// <param name="length">The length of the cylinder (not counting the spherical cap).</param>
        /// <returns>The created mass.</returns>
        public static Mass CreateCapsule(dReal density, DirectionAxis direction, dReal radius, dReal length)
        {
            Mass result;
            CreateCapsule(density, direction, radius, length, out result);
            return result;
        }

        /// <summary>
        /// Creates a capsule mass of the specified parameters and total mass, with
        /// center at the origin of the body frame.
        /// </summary>
        /// <param name="totalMass">The total mass of the capsule.</param>
        /// <param name="direction">The orientation of the cylinder's long axis.</param>
        /// <param name="radius">The radius of the cylinder (and the spherical cap).</param>
        /// <param name="length">The length of the cylinder (not counting the spherical cap).</param>
        /// <param name="result">The created mass.</param>
        public static void CreateCapsuleTotal(
            dReal totalMass, DirectionAxis direction,
            dReal radius, dReal length, out Mass result)
        {
            NativeMethods.dMassSetCapsuleTotal(out result, totalMass, direction, radius, length);
        }

        /// <summary>
        /// Creates a capsule mass of the specified parameters and total mass, with
        /// center at the origin of the body frame.
        /// </summary>
        /// <param name="totalMass">The total mass of the capsule.</param>
        /// <param name="direction">The orientation of the cylinder's long axis.</param>
        /// <param name="radius">The radius of the cylinder (and the spherical cap).</param>
        /// <param name="length">The length of the cylinder (not counting the spherical cap).</param>
        /// <returns>The created mass.</returns>
        public static Mass CreateCapsuleTotal(
            dReal totalMass, DirectionAxis direction,
            dReal radius, dReal length)
        {
            Mass result;
            CreateCapsuleTotal(totalMass, direction, radius, length, out result);
            return result;
        }

        /// <summary>
        /// Creates a flat-ended cylindrical mass of the specified parameters and
        /// density, with center at the origin of the body frame.
        /// </summary>
        /// <param name="density">The density of the cylinder.</param>
        /// <param name="direction">The orientation of the cylinder's long axis.</param>
        /// <param name="radius">The radius of the cylinder.</param>
        /// <param name="length">The length of the cylinder.</param>
        /// <param name="result">The created mass.</param>
        public static void CreateCylinder(
            dReal density, DirectionAxis direction,
            dReal radius, dReal length, out Mass result)
        {
            NativeMethods.dMassSetCylinder(out result, density, direction, radius, length);
        }

        /// <summary>
        /// Creates a flat-ended cylindrical mass of the specified parameters and
        /// density, with center at the origin of the body frame.
        /// </summary>
        /// <param name="density">The density of the cylinder.</param>
        /// <param name="direction">The orientation of the cylinder's long axis.</param>
        /// <param name="radius">The radius of the cylinder.</param>
        /// <param name="length">The length of the cylinder.</param>
        /// <returns>The created mass.</returns>
        public static Mass CreateCylinder(dReal density, DirectionAxis direction, dReal radius, dReal length)
        {
            Mass result;
            CreateCylinder(density, direction, radius, length, out result);
            return result;
        }

        /// <summary>
        /// Creates a flat-ended cylindrical mass of the specified parameters and
        /// total mass, with center at the origin of the body frame.
        /// </summary>
        /// <param name="totalMass">The total mass of the cylinder.</param>
        /// <param name="direction">The orientation of the cylinder's long axis.</param>
        /// <param name="radius">The radius of the cylinder.</param>
        /// <param name="length">The length of the cylinder.</param>
        /// <param name="result">The created mass.</param>
        public static void CreateCylinderTotal(
            dReal totalMass, DirectionAxis direction,
            dReal radius, dReal length, out Mass result)
        {
            NativeMethods.dMassSetCylinderTotal(out result, totalMass, direction, radius, length);
        }

        /// <summary>
        /// Creates a flat-ended cylindrical mass of the specified parameters and
        /// total mass, with center at the origin of the body frame.
        /// </summary>
        /// <param name="totalMass">The total mass of the cylinder.</param>
        /// <param name="direction">The orientation of the cylinder's long axis.</param>
        /// <param name="radius">The radius of the cylinder.</param>
        /// <param name="length">The length of the cylinder.</param>
        /// <returns>The created mass.</returns>
        public static Mass CreateCylinderTotal(
            dReal totalMass, DirectionAxis direction,
            dReal radius, dReal length)
        {
            Mass result;
            CreateCylinderTotal(totalMass, direction, radius, length, out result);
            return result;
        }

        /// <summary>
        /// Creates a box mass of the given dimensions and density, with center
        /// at the origin of the body frame.
        /// </summary>
        /// <param name="density">The density of the box.</param>
        /// <param name="lx">The side length of the box along the x axis.</param>
        /// <param name="ly">The side length of the box along the y axis.</param>
        /// <param name="lz">The side length of the box along the z axis.</param>
        /// <param name="result">The created mass.</param>
        public static void CreateBox(dReal density, dReal lx, dReal ly, dReal lz, out Mass result)
        {
            NativeMethods.dMassSetBox(out result, density, lx, ly, lz);
        }

        /// <summary>
        /// Creates a box mass of the given dimensions and density, with center
        /// at the origin of the body frame.
        /// </summary>
        /// <param name="density">The density of the box.</param>
        /// <param name="lx">The side length of the box along the x axis.</param>
        /// <param name="ly">The side length of the box along the y axis.</param>
        /// <param name="lz">The side length of the box along the z axis.</param>
        /// <returns>The created mass.</returns>
        public static Mass CreateBox(dReal density, dReal lx, dReal ly, dReal lz)
        {
            Mass result;
            CreateBox(density, lx, ly, lz, out result);
            return result;
        }

        /// <summary>
        /// Creates a box mass of the given dimensions and total mass, with center
        /// at the origin of the body frame.
        /// </summary>
        /// <param name="totalMass">The total mass of the box.</param>
        /// <param name="lx">The side length of the box along the x axis.</param>
        /// <param name="ly">The side length of the box along the y axis.</param>
        /// <param name="lz">The side length of the box along the z axis.</param>
        /// <param name="result">The created mass.</param>
        public static void CreateBoxTotal(dReal totalMass, dReal lx, dReal ly, dReal lz, out Mass result)
        {
            NativeMethods.dMassSetBoxTotal(out result, totalMass, lx, ly, lz);
        }

        /// <summary>
        /// Creates a box mass of the given dimensions and total mass, with center
        /// at the origin of the body frame.
        /// </summary>
        /// <param name="totalMass">The total mass of the box.</param>
        /// <param name="lx">The side length of the box along the x axis.</param>
        /// <param name="ly">The side length of the box along the y axis.</param>
        /// <param name="lz">The side length of the box along the z axis.</param>
        /// <returns>The created mass.</returns>
        public static Mass CreateBoxTotal(dReal totalMass, dReal lx, dReal ly, dReal lz)
        {
            Mass result;
            CreateBoxTotal(totalMass, lx, ly, lz, out result);
            return result;
        }

        /// <summary>
        /// Creates a mass for the given triangle mesh with the specified density,
        /// and center at the geometrical center of mass.
        /// </summary>
        /// <param name="density">The density of the triangle mesh.</param>
        /// <param name="triMesh">The triangle mesh for which to compute mass parameters.</param>
        /// <param name="result">The created mass.</param>
        public static void CreateTriMesh(dReal density, TriMesh triMesh, out Mass result)
        {
            if (triMesh == null)
            {
                throw new ArgumentNullException("triMesh");
            }

            NativeMethods.dMassSetTrimesh(out result, density, triMesh.Id);
        }

        /// <summary>
        /// Creates a mass for the given triangle mesh with the specified density,
        /// and center at the geometrical center of mass.
        /// </summary>
        /// <param name="density">The density of the triangle mesh.</param>
        /// <param name="triMesh">The triangle mesh for which to compute mass parameters.</param>
        /// <returns>The created mass.</returns>
        public static Mass CreateTriMesh(dReal density, TriMesh triMesh)
        {
            Mass result;
            CreateTriMesh(density, triMesh, out result);
            return result;
        }

        /// <summary>
        /// Creates a mass for the given triangle mesh with the specified total mass,
        /// and center at the geometrical center of mass.
        /// </summary>
        /// <param name="totalMass">The total mass of the triangle mesh.</param>
        /// <param name="triMesh">The triangle mesh for which to compute mass parameters.</param>
        /// <param name="result">The created mass.</param>
        public static void CreateTriMeshTotal(dReal totalMass, TriMesh triMesh, out Mass result)
        {
            if (triMesh == null)
            {
                throw new ArgumentNullException("triMesh");
            }

            NativeMethods.dMassSetTrimeshTotal(out result, totalMass, triMesh.Id);
        }

        /// <summary>
        /// Creates a mass for the given triangle mesh with the specified total mass,
        /// and center at the geometrical center of mass.
        /// </summary>
        /// <param name="totalMass">The total mass of the triangle mesh.</param>
        /// <param name="triMesh">The triangle mesh for which to compute mass parameters.</param>
        /// <returns>The created mass.</returns>
        public static Mass CreateTriMeshTotal(dReal totalMass, TriMesh triMesh)
        {
            Mass result;
            CreateTriMeshTotal(totalMass, triMesh, out result);
            return result;
        }

        /// <summary>
        /// Tests whether two <see cref="Mass"/> structures are equal.
        /// </summary>
        /// <param name="left">
        /// The <see cref="Mass"/> structure on the left of the equality operator.
        /// </param>
        /// <param name="right">
        /// The <see cref="Mass"/> structure on the right of the equality operator.
        /// </param>
        /// <returns>
        /// <b>true</b> if <paramref name="left"/> and <paramref name="right"/> have
        /// equal mass, center of gravity and inertia matrix; otherwise, <b>false</b>.
        /// </returns>
        public static bool operator ==(Mass left, Mass right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Tests whether two <see cref="Mass"/> structures are different.
        /// </summary>
        /// <param name="left">
        /// The <see cref="Mass"/> structure on the left of the inequality operator.
        /// </param>
        /// <param name="right">
        /// The <see cref="Mass"/> structure on the right of the inequality operator.
        /// </param>
        /// <returns>
        /// <b>true</b> if <paramref name="left"/> and <paramref name="right"/> differ in mass,
        /// center of gravity or inertia matrix; <b>false</b> if <paramref name="left"/> and
        /// <paramref name="right"/> are equal.
        /// </returns>
        public static bool operator !=(Mass left, Mass right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// Adds two <see cref="Mass"/> structures and returns the result as a new
        /// <see cref="Mass"/>.
        /// </summary>
        /// <param name="left">
        /// The <see cref="Mass"/> structure on the left of the addition operator.
        /// </param>
        /// <param name="right">
        /// The <see cref="Mass"/> structure on the right of the addition operator.
        /// </param>
        /// <returns>The sum of the two masses.</returns>
        public static Mass operator +(Mass left, Mass right)
        {
            NativeMethods.dMassAdd(ref left, ref right);
            return left;
        }
    }
}
