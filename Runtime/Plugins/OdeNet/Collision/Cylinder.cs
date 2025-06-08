using Ode.Net.Native;
#if ODE_DOUBLE_PRECISION
using dReal = System.Double;
#else
using dReal = System.Single;
#endif

namespace Ode.Net.Collision
{
    /// <summary>
    /// Represents a regular flat-ended cylinder geom.
    /// </summary>
    public sealed class Cylinder : Geom
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Cylinder"/> class with the
        /// specified radius and length.
        /// </summary>
        /// <param name="radius">The radius of the cylinder.</param>
        /// <param name="length">The length of the cylinder.</param>
        public Cylinder(dReal radius, dReal length)
            : this(null, radius, length)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Cylinder"/> class on the
        /// given space and with the specified radius and length.
        /// </summary>
        /// <param name="space">The space that is to contain the geom.</param>
        /// <param name="radius">The radius of the cylinder.</param>
        /// <param name="length">The length of the cylinder.</param>
        public Cylinder(Space space, dReal radius, dReal length)
            : base(NativeMethods.dCreateCylinder(space != null ? space.Id : dSpaceID.Null, radius, length))
        {
        }

        /// <summary>
        /// Gets or sets the radius of the cylinder.
        /// </summary>
        public dReal Radius
        {
            get
            {
                dReal radius, length;
                NativeMethods.dGeomCylinderGetParams(Id, out radius, out length);
                return radius;
            }
            set
            {
                dReal radius, length;
                NativeMethods.dGeomCylinderGetParams(Id, out radius, out length);
                NativeMethods.dGeomCylinderSetParams(Id, value, length);
            }
        }

        /// <summary>
        /// Gets or sets the length of the cylinder.
        /// </summary>
        public dReal Length
        {
            get
            {
                dReal radius, length;
                NativeMethods.dGeomCylinderGetParams(Id, out radius, out length);
                return length;
            }
            set
            {
                dReal radius, length;
                NativeMethods.dGeomCylinderGetParams(Id, out radius, out length);
                NativeMethods.dGeomCylinderSetParams(Id, radius, value);
            }
        }
    }
}
