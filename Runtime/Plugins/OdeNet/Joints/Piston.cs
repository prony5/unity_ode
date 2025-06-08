using Ode.Net.Native;
#if ODE_DOUBLE_PRECISION
using dReal = System.Double;
#else
using dReal = System.Single;
#endif

namespace Ode.Net.Joints
{
    /// <summary>
    /// Represents a piston joint.
    /// </summary>
    public sealed class Piston : Joint
    {
        readonly JointLimitMotor limitMotorP;
        readonly JointLimitMotor limitMotorR;

        /// <summary>
        /// Initializes a new instance of the <see cref="Piston"/> class.
        /// </summary>
        /// <param name="world">The world on which to place the joint.</param>
        public Piston(World world)
            : this(world, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Piston"/> class on
        /// the specified <see cref="JointGroup"/>.
        /// </summary>
        /// <param name="world">The world on which to place the joint.</param>
        /// <param name="group">The joint group that will contain the joint.</param>
        public Piston(World world, JointGroup group)
            : base(NativeMethods.dJointCreatePiston(world.Id, dJointGroupID.Null), group)
        {
            limitMotorP = new PistonLimitMotor(this, 0);
            limitMotorR = new PistonLimitMotor(this, 1);
        }

        /// <summary>
        /// Gets or sets the joint anchor point, in world coordinates.
        /// </summary>
        public Vector3 Anchor
        {
            get
            {
                Vector3 anchor;
                NativeMethods.dJointGetPistonAnchor(id, out anchor);
                return anchor;
            }
            set
            {
                NativeMethods.dJointSetPistonAnchor(id, value.X, value.Y, value.Z);
            }
        }

        /// <summary>
        /// Gets the anchor point on the second body, in world coordinates. If the
        /// joint is perfectly satisfied, this will be the same value as <see cref="Anchor"/>.
        /// </summary>
        public Vector3 Anchor2
        {
            get
            {
                Vector3 anchor;
                NativeMethods.dJointGetPistonAnchor2(id, out anchor);
                return anchor;
            }
        }

        /// <summary>
        /// Gets or sets the piston axis, in world coordinates.
        /// </summary>
        public Vector3 Axis
        {
            get
            {
                Vector3 axis;
                NativeMethods.dJointGetPistonAxis(id, out axis);
                return axis;
            }
            set
            {
                NativeMethods.dJointSetPistonAxis(id, value.X, value.Y, value.Z);
            }
        }

        /// <summary>
        /// Gets the piston linear position (i.e. the piston's extension).
        /// </summary>
        public dReal Position
        {
            get { return NativeMethods.dJointGetPistonPosition(id); }
        }

        /// <summary>
        /// Gets the piston linear position time derivative.
        /// </summary>
        public dReal PositionRate
        {
            get { return NativeMethods.dJointGetPistonPositionRate(id); }
        }

        /// <summary>
        /// Gets the piston angular position (i.e. the twist between the two bodies).
        /// </summary>
        public dReal Angle
        {
            get { return NativeMethods.dJointGetPistonAngle(id); }
        }

        /// <summary>
        /// Gets the piston angular position time derivative.
        /// </summary>
        public dReal AngleRate
        {
            get { return NativeMethods.dJointGetPistonAngleRate(id); }
        }

        /// <summary>
        /// Gets the limit and motor parameters for the prismatic part of the joint.
        /// </summary>
        public JointLimitMotor LimitMotorP
        {
            get { return limitMotorP; }
        }

        /// <summary>
        /// Gets the limit and motor parameters for the rotoide part of the joint.
        /// </summary>
        public JointLimitMotor LimitMotorR
        {
            get { return limitMotorR; }
        }

        /// <summary>
        /// Sets the piston anchor and the relative position of each body as if the
        /// two bodies were already separated by the specified offset.
        /// </summary>
        /// <param name="anchor">The joint anchor point, in world coordinates.</param>
        /// <param name="offset">
        /// The offset to be subtracted from the anchor point, as if the anchor was set
        /// when the first body was at its current position minus the offset.
        /// </param>
        public void SetAnchorOffset(Vector3 anchor, Vector3 offset)
        {
            NativeMethods.dJointSetPistonAnchorOffset(
                id, anchor.X, anchor.Y, anchor.Z,
                offset.X, offset.Y, offset.Z);
        }

        /// <summary>
        /// Applies the specified force in the piston's direction.
        /// </summary>
        /// <param name="force">The magnitude of the applied force.</param>
        public void AddForce(dReal force)
        {
            NativeMethods.dJointAddPistonForce(id, force);
        }

        class PistonLimitMotor : JointLimitMotor
        {
            internal PistonLimitMotor(Piston joint, int axis)
                : base(joint, axis)
            {
            }

            internal override dReal GetParam(dJointID id, dJointParam parameter)
            {
                return NativeMethods.dJointGetPistonParam(id, parameter);
            }

            internal override void SetParam(dJointID id, dJointParam parameter, dReal value)
            {
                NativeMethods.dJointSetPistonParam(id, parameter, value);
            }
        }
    }
}
