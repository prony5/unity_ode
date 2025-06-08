using Ode.Net.Native;
#if ODE_DOUBLE_PRECISION
using dReal = System.Double;
#else
using dReal = System.Single;
#endif

namespace Ode.Net.Joints
{
    /// <summary>
    /// Represents a prismatic and rotoide joint.
    /// </summary>
    public sealed class PrismaticRotoide : Joint
    {
        readonly JointLimitMotor limitMotor1;
        readonly JointLimitMotor limitMotor2;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrismaticRotoide"/> class.
        /// </summary>
        /// <param name="world">The world on which to place the joint.</param>
        public PrismaticRotoide(World world)
            : this(world, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrismaticRotoide"/> class on
        /// the specified <see cref="JointGroup"/>.
        /// </summary>
        /// <param name="world">The world on which to place the joint.</param>
        /// <param name="group">The joint group that will contain the joint.</param>
        public PrismaticRotoide(World world, JointGroup group)
            : base(NativeMethods.dJointCreatePR(world.Id, dJointGroupID.Null), group)
        {
            limitMotor1 = new PrismaticRotoideLimitMotor(this, 0);
            limitMotor2 = new PrismaticRotoideLimitMotor(this, 1);
        }

        /// <summary>
        /// Gets or sets the joint anchor point, in world coordinates.
        /// </summary>
        public Vector3 Anchor
        {
            get
            {
                Vector3 anchor;
                NativeMethods.dJointGetPRAnchor(id, out anchor);
                return anchor;
            }
            set
            {
                NativeMethods.dJointSetPRAnchor(id, value.X, value.Y, value.Z);
            }
        }

        /// <summary>
        /// Gets or sets the first PR axis, in world coordinates.
        /// </summary>
        public Vector3 Axis1
        {
            get
            {
                Vector3 axis;
                NativeMethods.dJointGetPRAxis1(id, out axis);
                return axis;
            }
            set
            {
                NativeMethods.dJointSetPRAxis1(id, value.X, value.Y, value.Z);
            }
        }

        /// <summary>
        /// Gets or sets the second PR axis, in world coordinates.
        /// </summary>
        public Vector3 Axis2
        {
            get
            {
                Vector3 axis;
                NativeMethods.dJointGetPRAxis2(id, out axis);
                return axis;
            }
            set
            {
                NativeMethods.dJointSetPRAxis2(id, value.X, value.Y, value.Z);
            }
        }

        /// <summary>
        /// Gets the PR linear position (i.e. the prismatic's extension).
        /// </summary>
        public dReal Position
        {
            get { return NativeMethods.dJointGetPRPosition(id); }
        }

        /// <summary>
        /// Gets the PR linear position time derivative.
        /// </summary>
        public dReal PositionRate
        {
            get { return NativeMethods.dJointGetPRPositionRate(id); }
        }

        /// <summary>
        /// Gets the PR angular position (i.e. the twist between the two bodies).
        /// </summary>
        public dReal Angle
        {
            get { return NativeMethods.dJointGetPRAngle(id); }
        }

        /// <summary>
        /// Gets the PR angular position time derivative.
        /// </summary>
        public dReal AngleRate
        {
            get { return NativeMethods.dJointGetPRAngleRate(id); }
        }

        /// <summary>
        /// Gets the limit and motor parameters of the first PR axis.
        /// </summary>
        public JointLimitMotor LimitMotor1
        {
            get { return limitMotor1; }
        }

        /// <summary>
        /// Gets the limit and motor parameters of the second PR axis.
        /// </summary>
        public JointLimitMotor LimitMotor2
        {
            get { return limitMotor2; }
        }

        /// <summary>
        /// Applies torque about the rotoide axis of the PR joint.
        /// </summary>
        /// <param name="torque">
        /// The magnitude of the torque to apply in the direction of the rotoide axis
        /// to the first body and in the opposite direction to the second body.
        /// </param>
        public void AddTorque(dReal torque)
        {
            NativeMethods.dJointAddPRTorque(id, torque);
        }

        class PrismaticRotoideLimitMotor : JointLimitMotor
        {
            internal PrismaticRotoideLimitMotor(PrismaticRotoide joint, int axis)
                : base(joint, axis)
            {
            }

            internal override dReal GetParam(dJointID id, dJointParam parameter)
            {
                return NativeMethods.dJointGetPRParam(id, parameter);
            }

            internal override void SetParam(dJointID id, dJointParam parameter, dReal value)
            {
                NativeMethods.dJointSetPRParam(id, parameter, value);
            }
        }
    }
}
