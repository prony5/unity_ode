using Ode.Net.Native;
#if ODE_DOUBLE_PRECISION
using dReal = System.Double;
#else
using dReal = System.Single;
#endif

namespace Ode.Net.Collision
{
    /// <summary>
    /// Represents a capsule geom.
    /// </summary>
    public sealed class Capsule : Geom
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Capsule"/> class with the
        /// specified radius and length.
        /// </summary>
        /// <param name="radius">The radius of the capsule.</param>
        /// <param name="length">The length of the capsule.</param>
        public Capsule(dReal radius, dReal length)
            : this(null, radius, length)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Capsule"/> class on the
        /// given space and with the specified radius and length.
        /// </summary>
        /// <param name="space">The space that is to contain the geom.</param>
        /// <param name="radius">The radius of the capsule.</param>
        /// <param name="length">The length of the capsule.</param>
        public Capsule(Space space, dReal radius, dReal length)
            : base(NativeMethods.dCreateCapsule(space != null ? space.Id : dSpaceID.Null, radius, length))
        {
        }

        /// <summary>
        /// Gets or sets the radius of the capsule.
        /// </summary>
        public dReal Radius
        {
            get
            {
                dReal radius, length;
                NativeMethods.dGeomCapsuleGetParams(Id, out radius, out length);
                return radius;
            }
            set
            {
                dReal radius, length;
                NativeMethods.dGeomCapsuleGetParams(Id, out radius, out length);
                NativeMethods.dGeomCapsuleSetParams(Id, value, length);
            }
        }

        /// <summary>
        /// Gets or sets the length of the capsule.
        /// </summary>
        public dReal Length
        {
            get
            {
                dReal radius, length;
                NativeMethods.dGeomCapsuleGetParams(Id, out radius, out length);
                return length;
            }
            set
            {
                dReal radius, length;
                NativeMethods.dGeomCapsuleGetParams(Id, out radius, out length);
                NativeMethods.dGeomCapsuleSetParams(Id, radius, value);
            }
        }

        /// <summary>
        /// Calculates the depth of the specified point within the capsule.
        /// </summary>
        /// <param name="x">The x-coordinate of the point to test.</param>
        /// <param name="y">The y-coordinate of the point to test.</param>
        /// <param name="z">The z-coordinate of the point to test.</param>
        /// <returns>
        /// The depth of the point. Points inside the capsule will have a
        /// positive depth, points outside it will have a negative depth,
        /// and points on the surface will have a depth of zero.
        /// </returns>
        public dReal PointDepth(dReal x, dReal y, dReal z)
        {
            return NativeMethods.dGeomCapsulePointDepth(Id, x, y, z);
        }
    }
}
