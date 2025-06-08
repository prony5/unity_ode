namespace Ode.Net.Joints
{
    /// <summary>
    /// Specifies the type of a joint.
    /// </summary>
    public enum JointType
    {
        /// <summary>
        /// Undefined joint type.
        /// </summary>
        None = 0,

        /// <summary>
        /// A ball and socket joint.
        /// </summary>
        Ball,

        /// <summary>
        /// A hinge joint.
        /// </summary>
        Hinge,

        /// <summary>
        /// A slider joint.
        /// </summary>
        Slider,

        /// <summary>
        /// A contact joint.
        /// </summary>
        Contact,

        /// <summary>
        /// A universal joint.
        /// </summary>
        Universal,

        /// <summary>
        /// A hinge-2 joint.
        /// </summary>
        Hinge2,

        /// <summary>
        /// A fixed joint.
        /// </summary>
        Fixed,

        /// <summary>
        /// A null joint.
        /// </summary>
        Null,

        /// <summary>
        /// An angular motor joint.
        /// </summary>
        AngularMotor,

        /// <summary>
        /// A linear motor joint.
        /// </summary>
        LinearMotor,

        /// <summary>
        /// A plane2D constraint.
        /// </summary>
        Plane2D,

        /// <summary>
        /// A prismatic and rotoide joint.
        /// </summary>
        PrismaticRotoide,

        /// <summary>
        /// A prismatic and universal joint.
        /// </summary>
        PrismaticUniversal,

        /// <summary>
        /// A piston joint.
        /// </summary>
        Piston,

        /// <summary>
        /// A double ball joint.
        /// </summary>
        DoubleBall,

        /// <summary>
        /// A double hinge joint.
        /// </summary>
        DoubleHinge,

        /// <summary>
        /// A transmission joint.
        /// </summary>
        Transmission,
    }
}
