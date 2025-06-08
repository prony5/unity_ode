using Ode.Net.Native;
#if ODE_DOUBLE_PRECISION
using dReal = System.Double;
#else
using dReal = System.Single;
#endif

namespace Ode.Net.Collision
{
    /// <summary>
    /// Represents an infinite (non-placeable) plane geom.
    /// </summary>
    public sealed class Plane : Geom
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Plane"/> class with the
        /// specified plane parameters.
        /// </summary>
        /// <param name="a">The first parameter of the plane equation.</param>
        /// <param name="b">The second parameter of the plane equation.</param>
        /// <param name="c">The third parameter of the plane equation.</param>
        /// <param name="d">The fourth parameter of the plane equation.</param>
        public Plane(dReal a, dReal b, dReal c, dReal d)
            : this(null, a, b, c, d)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Plane"/> class on the
        /// given space and with the specified plane parameters.
        /// </summary>
        /// <param name="space">The space that is to contain the geom.</param>
        /// <param name="a">The first parameter of the plane equation.</param>
        /// <param name="b">The second parameter of the plane equation.</param>
        /// <param name="c">The third parameter of the plane equation.</param>
        /// <param name="d">The fourth parameter of the plane equation.</param>
        public Plane(Space space, dReal a, dReal b, dReal c, dReal d)
            : base(NativeMethods.dCreatePlane(space != null ? space.Id : dSpaceID.Null, a, b, c, d))
        {
        }

        /// <summary>
        /// Gets or sets the parameters defining the plane equation.
        /// </summary>
        public Vector4 Parameters
        {
            get
            {
                Vector4 parameters;
                NativeMethods.dGeomPlaneGetParams(Id, out parameters);
                return parameters;
            }
            set
            {
                NativeMethods.dGeomPlaneSetParams(Id, value.X, value.Y, value.Z, value.W);
            }
        }

        /// <summary>
        /// Calculates the depth of the specified point within the plane.
        /// </summary>
        /// <param name="x">The x-coordinate of the point to test.</param>
        /// <param name="y">The y-coordinate of the point to test.</param>
        /// <param name="z">The z-coordinate of the point to test.</param>
        /// <returns>
        /// The depth of the point. Points inside the plane will have a
        /// positive depth, points outside it will have a negative depth,
        /// and points on the surface will have a depth of zero.
        /// </returns>
        public dReal PointDepth(dReal x, dReal y, dReal z)
        {
            return NativeMethods.dGeomPlanePointDepth(Id, x, y, z);
        }
    }
}
