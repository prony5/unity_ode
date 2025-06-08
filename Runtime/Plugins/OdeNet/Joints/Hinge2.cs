using Ode.Net.Native;
#if ODE_DOUBLE_PRECISION
using dReal = System.Double;
#else
using dReal = System.Single;
#endif

namespace Ode.Net.Joints
{
    /// <summary>
    /// Represents two hinge joints connected in series, with different hinge axes.
    /// </summary>
    public sealed class Hinge2 : Joint
    {
        readonly JointLimitMotor limitMotor1;
        readonly JointLimitMotor limitMotor2;

        /// <summary>
        /// Initializes a new instance of the <see cref="Hinge2"/> class.
        /// </summary>
        /// <param name="world">The world on which to place the joint.</param>
        public Hinge2(World world)
            : this(world, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Hinge2"/> class on
        /// the specified <see cref="JointGroup"/>.
        /// </summary>
        /// <param name="world">The world on which to place the joint.</param>
        /// <param name="group">The joint group that will contain the joint.</param>
        public Hinge2(World world, JointGroup group)
            : base(NativeMethods.dJointCreateHinge2(world.Id, dJointGroupID.Null), group)
        {
            limitMotor1 = new Hinge2LimitMotor(this, 0);
            limitMotor2 = new Hinge2LimitMotor(this, 1);
        }

        /// <summary>
        /// Gets or sets the joint anchor point, in world coordinates.
        /// </summary>
        public Vector3 Anchor
        {
            get
            {
                Vector3 anchor;
                NativeMethods.dJointGetHinge2Anchor(id, out anchor);
                return anchor;
            }
            set
            {
                NativeMethods.dJointSetHinge2Anchor(id, value.X, value.Y, value.Z);
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
                NativeMethods.dJointGetHinge2Anchor2(id, out anchor);
                return anchor;
            }
        }

        /// <summary>
        /// Gets or sets the axis for the first hinge, in world coordinates.
        /// </summary>
        public Vector3 Axis1
        {
            get
            {
                Vector3 axis;
                NativeMethods.dJointGetHinge2Axis1(id, out axis);
                return axis;
            }
            set
            {
                NativeMethods.dJointSetHinge2Axis1(id, value.X, value.Y, value.Z);
            }
        }

        /// <summary>
        /// Gets or sets the axis for the second hinge, in world coordinates.
        /// </summary>
        public Vector3 Axis2
        {
            get
            {
                Vector3 axis;
                NativeMethods.dJointGetHinge2Axis2(id, out axis);
                return axis;
            }
            set
            {
                NativeMethods.dJointSetHinge2Axis2(id, value.X, value.Y, value.Z);
            }
        }

        /// <summary>
        /// Gets the angle measured between the two bodies with respect
        /// to the first hinge axis.
        /// </summary>
        public dReal Angle1
        {
            get { return NativeMethods.dJointGetHinge2Angle1(id); }
        }

        /// <summary>
        /// Gets the first hinge angle time derivative.
        /// </summary>
        public dReal Angle1Rate
        {
            get { return NativeMethods.dJointGetHinge2Angle1Rate(id); }
        }

        /// <summary>
        /// Gets the second hinge angle time derivative.
        /// </summary>
        public dReal Angle2Rate
        {
            get { return NativeMethods.dJointGetHinge2Angle2Rate(id); }
        }

        /// <summary>
        /// Gets the limit and motor parameters of the first hinge.
        /// </summary>
        public JointLimitMotor LimitMotor1
        {
            get { return limitMotor1; }
        }

        /// <summary>
        /// Gets the limit and motor parameters of the second hinge.
        /// </summary>
        public JointLimitMotor LimitMotor2
        {
            get { return limitMotor2; }
        }

        /// <summary>
        /// Gets or sets the suspension error reduction parameter.
        /// </summary>
        public dReal SuspensionErp
        {
            get { return NativeMethods.dJointGetHinge2Param(id, dJointParam.dParamSuspensionERP); }
            set { NativeMethods.dJointSetHinge2Param(id, dJointParam.dParamSuspensionERP, value); }
        }

        /// <summary>
        /// Gets or sets the suspension constraint force mixing value.
        /// </summary>
        public dReal SuspensionCfm
        {
            get { return NativeMethods.dJointGetHinge2Param(id, dJointParam.dParamSuspensionCFM); }
            set { NativeMethods.dJointSetHinge2Param(id, dJointParam.dParamSuspensionCFM, value); }
        }

        /// <summary>
        /// Applies torque about both hinge axes.
        /// </summary>
        /// <param name="torque1">
        /// The magnitude of the torque to be applied about the first hinge axis.
        /// </param>
        /// <param name="torque2">
        /// The magnitude of the torque to be applied about the second hinge axis.
        /// </param>
        public void AddTorques(dReal torque1, dReal torque2)
        {
            NativeMethods.dJointAddHinge2Torques(id, torque1, torque2);
        }

        class Hinge2LimitMotor : JointLimitMotor
        {
            internal Hinge2LimitMotor(Hinge2 joint, int axis)
                : base(joint, axis)
            {
            }

            internal override dReal GetParam(dJointID id, dJointParam parameter)
            {
                return NativeMethods.dJointGetHinge2Param(id, parameter);
            }

            internal override void SetParam(dJointID id, dJointParam parameter, dReal value)
            {
                NativeMethods.dJointSetHinge2Param(id, parameter, value);
            }
        }
    }
}
