using Ode.Net.Native;
#if ODE_DOUBLE_PRECISION
using dReal = System.Double;
#else
using dReal = System.Single;
#endif

namespace Ode.Net.Collision
{
    /// <summary>
    /// Represents a sphere geom.
    /// </summary>
    public sealed class Sphere : Geom
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Sphere"/> class with the
        /// specified radius.
        /// </summary>
        /// <param name="radius">The radius of the sphere.</param>
        public Sphere(dReal radius)
            : this(null, radius)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Sphere"/> class on the
        /// given space and with the specified radius.
        /// </summary>
        /// <param name="space">The space that is to contain the geom.</param>
        /// <param name="radius">The radius of the sphere.</param>
        public Sphere(Space space, dReal radius)
            : base(NativeMethods.dCreateSphere(space != null ? space.Id : dSpaceID.Null, radius))
        {
        }

        /// <summary>
        /// Gets or sets the radius of the sphere.
        /// </summary>
        public dReal Radius
        {
            get { return NativeMethods.dGeomSphereGetRadius(Id); }
            set { NativeMethods.dGeomSphereSetRadius(Id, value); }
        }

        /// <summary>
        /// Calculates the depth of the specified point within the sphere.
        /// </summary>
        /// <param name="x">The x-coordinate of the point to test.</param>
        /// <param name="y">The y-coordinate of the point to test.</param>
        /// <param name="z">The z-coordinate of the point to test.</param>
        /// <returns>
        /// The depth of the point. Points inside the sphere will have a
        /// positive depth, points outside it will have a negative depth,
        /// and points on the surface will have a depth of zero.
        /// </returns>
        public dReal PointDepth(dReal x, dReal y, dReal z)
        {
            return NativeMethods.dGeomSpherePointDepth(Id, x, y, z);
        }
    }
}
