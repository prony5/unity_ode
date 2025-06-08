using System;
using System.Runtime.InteropServices;
#if ODE_DOUBLE_PRECISION
using dReal = System.Double;
#else
using dReal = System.Single;
#endif

namespace Ode.Net.Collision
{
    /// <summary>
    /// Represents the geometry of a contact point between two inter-penetrating bodies.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ContactGeom
    {
        internal static readonly int Size = Marshal.SizeOf(typeof(ContactGeom));

        /// <summary>
        /// Specifies the contact position, in global coordinates.
        /// </summary>
        public Vector3 Position;

        /// <summary>
        /// Specifies a unit length vector that is, generally speaking, perpendicular
        /// to the contact surface.
        /// </summary>
        public Vector3 Normal;

        /// <summary>
        /// Specifies the depth to which the two bodies inter-penetrate each other.
        /// </summary>
        public dReal Depth;

        IntPtr g1, g2;

        /// <summary>
        /// Specifies an additional information field for the first collision geom.
        /// </summary>
        /// <remarks>
        /// The side field gives additional information about what part of the
        /// geom collided. It is only currently used for trimeshes, indicating
        /// the triangle index of the collision.
        /// </remarks>
        public int Side1;

        /// <summary>
        /// Specifies an additional information field for the second collision geom.
        /// </summary>
        /// <remarks>
        /// The side field gives additional information about what part of the
        /// geom collided. It is only currently used for trimeshes, indicating
        /// the triangle index of the collision.
        /// </remarks>
        public int Side2;

        /// <summary>
        /// Gets or sets the first colliding geometry object.
        /// </summary>
        public Geom Geom1
        {
            get { return Geom.FromIntPtr(g1); }
            set { g2 = value != null ? value.Id.DangerousGetHandle() : IntPtr.Zero; }
        }

        /// <summary>
        /// Gets or sets the second colliding geometry object.
        /// </summary>
        public Geom Geom2
        {
            get { return Geom.FromIntPtr(g2); }
            set { g2 = value != null ? value.Id.DangerousGetHandle() : IntPtr.Zero; }
        }
    }
}
