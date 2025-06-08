using Ode.Net.Native;
#if ODE_DOUBLE_PRECISION
using dReal = System.Double;
#else
using dReal = System.Single;
#endif

namespace Ode.Net.Joints
{
    /// <summary>
    /// Represents a prismatic and universal joint.
    /// </summary>
    public sealed class PrismaticUniversal : Joint
    {
        readonly JointLimitMotor limitMotor1;
        readonly JointLimitMotor limitMotor2;
        readonly JointLimitMotor limitMotor3;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrismaticUniversal"/> class.
        /// </summary>
        /// <param name="world">The world on which to place the joint.</param>
        public PrismaticUniversal(World world)
            : this(world, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrismaticUniversal"/> class on
        /// the specified <see cref="JointGroup"/>.
        /// </summary>
        /// <param name="world">The world on which to place the joint.</param>
        /// <param name="group">The joint group that will contain the joint.</param>
        public PrismaticUniversal(World world, JointGroup group)
            : base(NativeMethods.dJointCreatePU(world.Id, dJointGroupID.Null), group)
        {
            limitMotor1 = new PrismaticUniversalLimitMotor(this, 0);
            limitMotor2 = new PrismaticUniversalLimitMotor(this, 1);
            limitMotor3 = new PrismaticUniversalLimitMotor(this, 2);
        }

        /// <summary>
        /// Gets or sets the joint anchor point, in world coordinates.
        /// </summary>
        public Vector3 Anchor
        {
            get
            {
                Vector3 anchor;
                NativeMethods.dJointGetPUAnchor(id, out anchor);
                return anchor;
            }
            set
            {
                NativeMethods.dJointSetPUAnchor(id, value.X, value.Y, value.Z);
            }
        }

        /// <summary>
        /// Gets or sets the first, universal PU axis, in world coordinates.
        /// </summary>
        public Vector3 Axis1
        {
            get
            {
                Vector3 axis;
                NativeMethods.dJointGetPUAxis1(id, out axis);
                return axis;
            }
            set
            {
                NativeMethods.dJointSetPUAxis1(id, value.X, value.Y, value.Z);
            }
        }

        /// <summary>
        /// Gets or sets the second, universal PU axis, in world coordinates.
        /// </summary>
        public Vector3 Axis2
        {
            get
            {
                Vector3 axis;
                NativeMethods.dJointGetPUAxis2(id, out axis);
                return axis;
            }
            set
            {
                NativeMethods.dJointSetPUAxis2(id, value.X, value.Y, value.Z);
            }
        }

        /// <summary>
        /// Gets or sets the third, prismatic PU axis, in world coordinates.
        /// </summary>
        public Vector3 Axis3
        {
            get
            {
                Vector3 axis;
                NativeMethods.dJointGetPUAxis3(id, out axis);
                return axis;
            }
            set
            {
                NativeMethods.dJointSetPUAxis3(id, value.X, value.Y, value.Z);
            }
        }

        /// <summary>
        /// Gets the PU linear position (i.e. the prismatic's extension).
        /// </summary>
        public dReal Position
        {
            get { return NativeMethods.dJointGetPUPosition(id); }
        }

        /// <summary>
        /// Gets the PU linear position time derivative.
        /// </summary>
        public dReal PositionRate
        {
            get { return NativeMethods.dJointGetPUPositionRate(id); }
        }

        /// <summary>
        /// Gets the angle measured between the first body and the first universal axis.
        /// </summary>
        public dReal Angle1
        {
            get { return NativeMethods.dJointGetPUAngle1(id); }
        }

        /// <summary>
        /// Gets the first angle time derivative.
        /// </summary>
        public dReal Angle1Rate
        {
            get { return NativeMethods.dJointGetPUAngle1Rate(id); }
        }

        /// <summary>
        /// Gets the angle measured between the second body and the second universal axis.
        /// </summary>
        public dReal Angle2
        {
            get { return NativeMethods.dJointGetPUAngle2(id); }
        }

        /// <summary>
        /// Gets the second angle time derivative.
        /// </summary>
        public dReal Angle2Rate
        {
            get { return NativeMethods.dJointGetPUAngle2Rate(id); }
        }

        /// <summary>
        /// Gets the limit and motor parameters of the first PU axis.
        /// </summary>
        public JointLimitMotor LimitMotor1
        {
            get { return limitMotor1; }
        }

        /// <summary>
        /// Gets the limit and motor parameters of the second PU axis.
        /// </summary>
        public JointLimitMotor LimitMotor2
        {
            get { return limitMotor2; }
        }

        /// <summary>
        /// Gets the limit and motor parameters of the prismatic PU axis.
        /// </summary>
        public JointLimitMotor LimitMotor3
        {
            get { return limitMotor3; }
        }

        /// <summary>
        /// Gets both universal joint angles at the same time.
        /// </summary>
        /// <param name="angle1">
        /// When the method returns, contains the angle between the first body and the first axis.
        /// </param>
        /// <param name="angle2">
        /// When the method returns, contains the angle between the second body and the second axis.
        /// </param>
        public void GetAngles(out dReal angle1, out dReal angle2)
        {
            NativeMethods.dJointGetPUAngles(id, out angle1, out angle2);
        }

        /// <summary>
        /// Sets the PU anchor and the relative position of each body as if the
        /// two bodies were already separated by the specified offset.
        /// </summary>
        /// <param name="anchor">The joint anchor point, in world coordinates.</param>
        /// <param name="offset">
        /// The offset to be subtracted from the anchor point, as if the anchor was set
        /// when the first body was at its current position minus the offset.
        /// </param>
        public void SetAnchorOffset(Vector3 anchor, Vector3 offset)
        {
            NativeMethods.dJointSetPUAnchorOffset(
                id, anchor.X, anchor.Y, anchor.Z,
                offset.X, offset.Y, offset.Z);
        }

        /// <summary>
        /// Applies torque about the rotoide axis of the PU joint.
        /// </summary>
        /// <param name="torque">
        /// The magnitude of the torque to apply in the direction of the rotoide axis
        /// to the first body and in the opposite direction to the second body.
        /// </param>
        public void AddTorque(dReal torque)
        {
            NativeMethods.dJointAddPUTorque(id, torque);
        }

        class PrismaticUniversalLimitMotor : JointLimitMotor
        {
            internal PrismaticUniversalLimitMotor(PrismaticUniversal joint, int axis)
                : base(joint, axis)
            {
            }

            internal override dReal GetParam(dJointID id, dJointParam parameter)
            {
                return NativeMethods.dJointGetPUParam(id, parameter);
            }

            internal override void SetParam(dJointID id, dJointParam parameter, dReal value)
            {
                NativeMethods.dJointSetPUParam(id, parameter, value);
            }
        }
    }
}
