using Ode.Net.Native;
#if ODE_DOUBLE_PRECISION
using dReal = System.Double;
#else
using dReal = System.Single;
#endif

namespace Ode.Net.Joints
{
    /// <summary>
    /// Represents a universal joint.
    /// </summary>
    public sealed class Universal : Joint
    {
        readonly JointLimitMotor limitMotor1;
        readonly JointLimitMotor limitMotor2;

        /// <summary>
        /// Initializes a new instance of the <see cref="Universal"/> class.
        /// </summary>
        /// <param name="world">The world on which to place the joint.</param>
        public Universal(World world)
            : this(world, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Universal"/> class on
        /// the specified <see cref="JointGroup"/>.
        /// </summary>
        /// <param name="world">The world on which to place the joint.</param>
        /// <param name="group">The joint group that will contain the joint.</param>
        public Universal(World world, JointGroup group)
            : base(NativeMethods.dJointCreateUniversal(world.Id, dJointGroupID.Null), group)
        {
            limitMotor1 = new UniversalLimitMotor(this, 0);
            limitMotor2 = new UniversalLimitMotor(this, 1);
        }

        /// <summary>
        /// Gets or sets the joint anchor point, in world coordinates.
        /// </summary>
        public Vector3 Anchor
        {
            get
            {
                Vector3 anchor;
                NativeMethods.dJointGetUniversalAnchor(id, out anchor);
                return anchor;
            }
            set
            {
                NativeMethods.dJointSetUniversalAnchor(id, value.X, value.Y, value.Z);
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
                NativeMethods.dJointGetUniversalAnchor2(id, out anchor);
                return anchor;
            }
        }

        /// <summary>
        /// Gets or sets the first universal joint axis, in world coordinates.
        /// </summary>
        public Vector3 Axis1
        {
            get
            {
                Vector3 axis;
                NativeMethods.dJointGetUniversalAxis1(id, out axis);
                return axis;
            }
            set
            {
                NativeMethods.dJointSetUniversalAxis1(id, value.X, value.Y, value.Z);
            }
        }

        /// <summary>
        /// Gets or sets the second universal joint axis, in world coordinates.
        /// </summary>
        public Vector3 Axis2
        {
            get
            {
                Vector3 axis;
                NativeMethods.dJointGetUniversalAxis2(id, out axis);
                return axis;
            }
            set
            {
                NativeMethods.dJointSetUniversalAxis2(id, value.X, value.Y, value.Z);
            }
        }

        /// <summary>
        /// Gets the angle measured between the first body and the first universal axis.
        /// </summary>
        public dReal Angle1
        {
            get { return NativeMethods.dJointGetUniversalAngle1(id); }
        }

        /// <summary>
        /// Gets the first angle time derivative.
        /// </summary>
        public dReal Angle1Rate
        {
            get { return NativeMethods.dJointGetUniversalAngle1Rate(id); }
        }

        /// <summary>
        /// Gets the angle measured between the second body and the second universal axis.
        /// </summary>
        public dReal Angle2
        {
            get { return NativeMethods.dJointGetUniversalAngle2(id); }
        }

        /// <summary>
        /// Gets the second angle time derivative.
        /// </summary>
        public dReal Angle2Rate
        {
            get { return NativeMethods.dJointGetUniversalAngle2Rate(id); }
        }

        /// <summary>
        /// Gets the limit and motor parameters of the first universal axis.
        /// </summary>
        public JointLimitMotor LimitMotor1
        {
            get { return limitMotor1; }
        }

        /// <summary>
        /// Gets the limit and motor parameters of the second universal axis.
        /// </summary>
        public JointLimitMotor LimitMotor2
        {
            get { return limitMotor2; }
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
            NativeMethods.dJointGetUniversalAngles(id, out angle1, out angle2);
        }

        /// <summary>
        /// Sets the first universal axis and the relative orientation of each body
        /// as if the first body was rotated around the first axis by <paramref name="offset1"/>
        /// and the second body was rotated around the second axis by <paramref name="offset2"/>.
        /// </summary>
        /// <param name="axis">The first universal axis, in world frame coordinates.</param>
        /// <param name="offset1">
        /// The angle for the offset of the relative orientation between the first body
        /// and the first axis. The rotation is specified about the new universal axes.
        /// </param>
        /// <param name="offset2">
        /// The angle for the offset of the relative orientation between the second body
        /// and the second axis. The rotation is specified about the new universal axes.
        /// </param>
        public void SetAxis1Offset(Vector3 axis, dReal offset1, dReal offset2)
        {
            NativeMethods.dJointSetUniversalAxis1Offset(
                id, axis.X, axis.Y, axis.Z,
                offset1, offset2);
        }

        /// <summary>
        /// Sets the second universal axis and the relative orientation of each body
        /// as if the first body was rotated around the first axis by <paramref name="offset1"/>
        /// and the second body was rotated around the second axis by <paramref name="offset2"/>.
        /// </summary>
        /// <param name="axis">The second universal axis, in world frame coordinates.</param>
        /// <param name="offset1">
        /// The angle for the offset of the relative orientation between the first body
        /// and the first axis. The rotation is specified about the new universal axes.
        /// </param>
        /// <param name="offset2">
        /// The angle for the offset of the relative orientation between the second body
        /// and the second axis. The rotation is specified about the new universal axes.
        /// </param>
        public void SetAxis2Offset(Vector3 axis, dReal offset1, dReal offset2)
        {
            NativeMethods.dJointSetUniversalAxis2Offset(
                id, axis.X, axis.Y, axis.Z,
                offset1, offset2);
        }

        /// <summary>
        /// Applies torque about both universal joint axes.
        /// </summary>
        /// <param name="torque1">
        /// The magnitude of the torque to be applied about the first universal axis.
        /// </param>
        /// <param name="torque2">
        /// The magnitude of the torque to be applied about the second universal axis.
        /// </param>
        public void AddTorques(dReal torque1, dReal torque2)
        {
            NativeMethods.dJointAddUniversalTorques(id, torque1, torque2);
        }

        class UniversalLimitMotor : JointLimitMotor
        {
            internal UniversalLimitMotor(Universal joint, int axis)
                : base(joint, axis)
            {
            }

            internal override dReal GetParam(dJointID id, dJointParam parameter)
            {
                return NativeMethods.dJointGetUniversalParam(id, parameter);
            }

            internal override void SetParam(dJointID id, dJointParam parameter, dReal value)
            {
                NativeMethods.dJointSetUniversalParam(id, parameter, value);
            }
        }
    }
}
