using Ode.Net.Native;
#if ODE_DOUBLE_PRECISION
using dReal = System.Double;
#else
using dReal = System.Single;
#endif

namespace Ode.Net.Collision
{
    /// <summary>
    /// Represents a ray geom.
    /// </summary>
    public sealed class Ray : Geom
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Ray"/> class with the
        /// specified length.
        /// </summary>
        /// <param name="length">The length of the ray.</param>
        public Ray(dReal length)
            : this(null, length)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Ray"/> class on the
        /// given space and with the specified length.
        /// </summary>
        /// <param name="space">The space that is to contain the geom.</param>
        /// <param name="length">The length of the ray.</param>
        public Ray(Space space, dReal length)
            : base(NativeMethods.dCreateRay(space != null ? space.Id : dSpaceID.Null, length))
        {
        }

        /// <summary>
        /// Gets or sets the length of the ray.
        /// </summary>
        public dReal Length
        {
            get { return NativeMethods.dGeomRayGetLength(Id); }
            set { NativeMethods.dGeomRaySetLength(Id, value); }
        }

        /// <summary>
        /// Gets or sets the direction vector of the ray.
        /// </summary>
        public Vector3 Direction
        {
            get
            {
                Vector3 start, direction;
                NativeMethods.dGeomRayGet(Id, out start, out direction);
                return direction;
            }
            set
            {
                Vector3 start;
                NativeMethods.dGeomCopyPosition(Id, out start);
                NativeMethods.dGeomRaySet(Id, start.X, start.Y, start.Z, value.X, value.Y, value.Z);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether collision returns the first
        /// contact detected between the ray and a trimesh.
        /// </summary>
        public bool FirstContact
        {
            get
            {
                int first, backface;
                NativeMethods.dGeomRayGetParams(Id, out first, out backface);
                return first != 0;
            }
            set
            {
                int first, backface;
                NativeMethods.dGeomRayGetParams(Id, out first, out backface);
                NativeMethods.dGeomRaySetParams(Id, value ? 1 : 0, backface);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether collision returns a contact
        /// between the ray and a backfacing triangle of a trimesh.
        /// </summary>
        public bool BackfaceCull
        {
            get
            {
                int first, backface;
                NativeMethods.dGeomRayGetParams(Id, out first, out backface);
                return backface != 0;
            }
            set
            {
                int first, backface;
                NativeMethods.dGeomRayGetParams(Id, out first, out backface);
                NativeMethods.dGeomRaySetParams(Id, first, value ? 1 : 0);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether collision returns the closest contact
        /// to the ray position.
        /// </summary>
        /// <remarks>
        /// This parameter is ignored if <see cref="FirstContact"/> is set to <b>true</b>.
        /// </remarks>
        public bool ClosestHit
        {
            get { return NativeMethods.dGeomRayGetClosestHit(Id) != 0; }
            set { NativeMethods.dGeomRaySetClosestHit(Id, value ? 1 : 0); }
        }
    }
}
