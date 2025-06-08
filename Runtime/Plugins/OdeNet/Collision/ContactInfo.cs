using System.Runtime.InteropServices;

namespace Ode.Net.Collision
{
    /// <summary>
    /// Represents information about the surface properties and geometry
    /// of a contact point.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ContactInfo
    {
        /// <summary>
        /// Specifies properties of the colliding surface.
        /// </summary>
        public SurfaceParameters Surface;

        /// <summary>
        /// Specifies the geometric properties of the contact point.
        /// </summary>
        public ContactGeom Geometry;

        /// <summary>
        /// Specifies the direction along which frictional force is applied.
        /// </summary>
        /// <remarks>
        /// It must be of unit length and perpendicular to the contact normal
        /// (so it is typically tangential to the contact surface). It should
        /// only be defined if the FrictionDirection1 mode is set in Surface.Mode.
        /// The "second friction direction" is a vector computed to be
        /// perpendicular to both the contact normal and FrictionDirection1.
        /// </remarks>
        public Vector3 FrictionDirection1;
    }
}
