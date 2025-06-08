using System;

namespace Ode.Net.Collision
{
    /// <summary>
    /// Specifies the contact modes for a collision surface.
    /// </summary>
    [Flags]
    public enum ContactModes
    {
        /// <summary>
        /// Specifies whether to use axis dependent friction. If set, use Mu for friction
        /// direction 1, use Mu2 for friction direction 2.
        /// </summary>
        Mu2 = 0x001,

        /// <summary>
        /// Specifies whether to take FrictionDirection1 as friction direction 1, otherwise
        /// automatically compute friction direction 1 to be perpendicular to the contact normal.
        /// </summary>
        FrictionDirection1 = 0x002,

        /// <summary>
        /// Specifies whether the contact surface is bouncy. The exact amount of bouncyness
        /// is controlled by the bounce parameter.
        /// </summary>
        Bounce = 0x004,

        /// <summary>
        /// Specifies whether the error reduction parameter of the contact normal can be set
        /// with the SoftERP parameter. This is useful to make surfaces soft.
        /// </summary>
        SoftErp = 0x008,

        /// <summary>
        /// Specifies whether the constraint force mixing parameter of the contact normal can
        /// be set with the SoftCFM parameter. This is useful to make surfaces soft.
        /// </summary>
        SoftCfm = 0x010,

        /// <summary>
        /// Specifies whether the contact surface is assumed to be moving independently of the
        /// motion of the bodies, like a conveyor belt running over the surface. When this flag
        /// is set, Motion1 defines the surface velocity in friction direction 1.
        /// </summary>
        Motion1 = 0x020,

        /// <summary>
        /// Specifies whether the contact surface is assumed to be moving independently of the
        /// motion of the bodies, like a conveyor belt running over the surface. When this flag
        /// is set, Motion2 defines the surface velocity in friction direction 2.
        /// </summary>
        Motion2 = 0x040,

        /// <summary>
        /// Specifies whether the contact surface is assumed to be moving independently of the
        /// motion of the bodies, like a conveyor belt running over the surface. When this flag
        /// is set, MotionN defines the surface velocity in the direction of the contact normal.
        /// </summary>
        MotionN = 0x080,

        /// <summary>
        /// Specifies that force-dependent-slip (FDS) should be used in friction direction 1.
        /// </summary>
        Slip1 = 0x100,

        /// <summary>
        /// Specifies that force-dependent-slip (FDS) should be used in friction direction 2.
        /// </summary>
        Slip2 = 0x200,

        /// <summary>
        /// Specifies that rolling/angular friction should be used.
        /// </summary>
        Rolling = 0x400,

        /// <summary>
        /// Specifies that the default constant-force-limit approximation should
        /// be used for all friction directions.
        /// </summary>
        Approx0 = 0x0000,

        /// <summary>
        /// Specifies that the friction pyramid approximation should be used for
        /// friction direction 1.
        /// </summary>
        Approx11 = 0x1000,

        /// <summary>
        /// Specifies that the friction pyramid approximation should be used for
        /// friction direction 2.
        /// </summary>
        Approx12 = 0x2000,

        /// <summary>
        /// Specifies that the friction pyramid approximation should be used for
        /// rolling around normal.
        /// </summary>
        Approx1N = 0x4000,

        /// <summary>
        /// Specifies that the friction pyramid approximation should be used for
        /// all friction directions.
        /// </summary>
        Approx1 = 0x7000
    }
}
