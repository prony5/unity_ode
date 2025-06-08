using Ode.Net.Native;
#if ODE_DOUBLE_PRECISION
using dReal = System.Double;
#else
using dReal = System.Single;
#endif

namespace Ode.Net.Joints
{
    /// <summary>
    /// Represents an angular motor joint.
    /// </summary>
    public sealed class AngularMotor : Joint
    {
        readonly JointLimitMotor limitMotor1;
        readonly JointLimitMotor limitMotor2;
        readonly JointLimitMotor limitMotor3;

        /// <summary>
        /// Initializes a new instance of the <see cref="AngularMotor"/> class.
        /// </summary>
        /// <param name="world">The world on which to place the joint.</param>
        public AngularMotor(World world)
            : this(world, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AngularMotor"/> class on
        /// the specified <see cref="JointGroup"/>.
        /// </summary>
        /// <param name="world">The world on which to place the joint.</param>
        /// <param name="group">The joint group that will contain the joint.</param>
        public AngularMotor(World world, JointGroup group)
            : base(NativeMethods.dJointCreateAMotor(world.Id, dJointGroupID.Null), group)
        {
            limitMotor1 = new AngularMotorLimitMotor(this, 0);
            limitMotor2 = new AngularMotorLimitMotor(this, 1);
            limitMotor3 = new AngularMotorLimitMotor(this, 2);
        }

        /// <summary>
        /// Gets or sets the number of angular axes that will be controlled by
        /// the angular motor.
        /// </summary>
        public int NumAxes
        {
            get { return NativeMethods.dJointGetAMotorNumAxes(id); }
            set { NativeMethods.dJointSetAMotorNumAxes(id, value); }
        }

        /// <summary>
        /// Gets or sets the first angular motor axis.
        /// </summary>
        public Vector3 Axis1
        {
            get
            {
                Vector3 axis;
                NativeMethods.dJointGetAMotorAxis(id, 0, out axis);
                return axis;
            }
            set
            {
                var relativeMode = NativeMethods.dJointGetAMotorAxisRel(id, 0);
                NativeMethods.dJointSetAMotorAxis(id, 0, relativeMode, value.X, value.Y, value.Z);
            }
        }

        /// <summary>
        /// Gets or sets the second angular motor axis.
        /// </summary>
        public Vector3 Axis2
        {
            get
            {
                Vector3 axis;
                NativeMethods.dJointGetAMotorAxis(id, 1, out axis);
                return axis;
            }
            set
            {
                var relativeMode = NativeMethods.dJointGetAMotorAxisRel(id, 1);
                NativeMethods.dJointSetAMotorAxis(id, 1, relativeMode, value.X, value.Y, value.Z);
            }
        }

        /// <summary>
        /// Gets or sets the third angular motor axis.
        /// </summary>
        public Vector3 Axis3
        {
            get
            {
                Vector3 axis;
                NativeMethods.dJointGetAMotorAxis(id, 2, out axis);
                return axis;
            }
            set
            {
                var relativeMode = NativeMethods.dJointGetAMotorAxisRel(id, 2);
                NativeMethods.dJointSetAMotorAxis(id, 2, relativeMode, value.X, value.Y, value.Z);
            }
        }

        /// <summary>
        /// Gets or sets the relative orientation mode of the first angular motor axis.
        /// </summary>
        public RelativeOrientation Axis1RelativeOrientation
        {
            get { return NativeMethods.dJointGetAMotorAxisRel(id, 0); }
            set
            {
                Vector3 axis;
                NativeMethods.dJointGetAMotorAxis(id, 0, out axis);
                NativeMethods.dJointSetAMotorAxis(id, 0, value, axis.X, axis.Y, axis.Z);
            }
        }

        /// <summary>
        /// Gets or sets the relative orientation mode of the second angular motor axis.
        /// </summary>
        public RelativeOrientation Axis2RelativeOrientation
        {
            get { return NativeMethods.dJointGetAMotorAxisRel(id, 1); }
            set
            {
                Vector3 axis;
                NativeMethods.dJointGetAMotorAxis(id, 1, out axis);
                NativeMethods.dJointSetAMotorAxis(id, 1, value, axis.X, axis.Y, axis.Z);
            }
        }

        /// <summary>
        /// Gets or sets the relative orientation mode of the third angular motor axis.
        /// </summary>
        public RelativeOrientation Axis3RelativeOrientation
        {
            get { return NativeMethods.dJointGetAMotorAxisRel(id, 2); }
            set
            {
                Vector3 axis;
                NativeMethods.dJointGetAMotorAxis(id, 2, out axis);
                NativeMethods.dJointSetAMotorAxis(id, 2, value, axis.X, axis.Y, axis.Z);
            }
        }

        /// <summary>
        /// Gets or sets the current angle for the first angular motor axis.
        /// </summary>
        public dReal Angle1
        {
            get { return NativeMethods.dJointGetAMotorAngle(id, 0); }
            set { NativeMethods.dJointSetAMotorAngle(id, 0, value); }
        }

        /// <summary>
        /// Gets or sets the current angle for the second angular motor axis.
        /// </summary>
        public dReal Angle2
        {
            get { return NativeMethods.dJointGetAMotorAngle(id, 1); }
            set { NativeMethods.dJointSetAMotorAngle(id, 1, value); }
        }

        /// <summary>
        /// Gets or sets the current angle for the third angular motor axis.
        /// </summary>
        public dReal Angle3
        {
            get { return NativeMethods.dJointGetAMotorAngle(id, 2); }
            set { NativeMethods.dJointSetAMotorAngle(id, 2, value); }
        }

        /// <summary>
        /// Gets the first angle time derivative.
        /// </summary>
        public dReal Angle1Rate
        {
            get { return NativeMethods.dJointGetAMotorAngleRate(id, 0); }
        }

        /// <summary>
        /// Gets the second angle time derivative.
        /// </summary>
        public dReal Angle2Rate
        {
            get { return NativeMethods.dJointGetAMotorAngleRate(id, 1); }
        }

        /// <summary>
        /// Gets the third angle time derivative.
        /// </summary>
        public dReal Angle3Rate
        {
            get { return NativeMethods.dJointGetAMotorAngleRate(id, 2); }
        }

        /// <summary>
        /// Gets or sets the angular motor mode.
        /// </summary>
        public AngularMotorMode Mode
        {
            get { return NativeMethods.dJointGetAMotorMode(id); }
            set { NativeMethods.dJointSetAMotorMode(id, value); }
        }

        /// <summary>
        /// Gets the limit and motor parameters of the first angular motor axis.
        /// </summary>
        public JointLimitMotor LimitMotor1
        {
            get { return limitMotor1; }
        }

        /// <summary>
        /// Gets the limit and motor parameters of the second angular motor axis.
        /// </summary>
        public JointLimitMotor LimitMotor2
        {
            get { return limitMotor2; }
        }

        /// <summary>
        /// Gets the limit and motor parameters of the third angular motor axis.
        /// </summary>
        public JointLimitMotor LimitMotor3
        {
            get { return limitMotor3; }
        }

        /// <summary>
        /// Applies torque about all angular motor axes.
        /// </summary>
        /// <param name="torque1">
        /// The magnitude of the torque to be applied about the first angular motor axis.
        /// </param>
        /// <param name="torque2">
        /// The magnitude of the torque to be applied about the second angular motor axis.
        /// </param>
        /// <param name="torque3">
        /// The magnitude of the torque to be applied about the third angular motor axis.
        /// </param>
        public void AddTorques(dReal torque1, dReal torque2, dReal torque3)
        {
            NativeMethods.dJointAddAMotorTorques(id, torque1, torque2, torque3);
        }

        /// <summary>
        /// Gets or sets the first angular motor axis.
        /// </summary>
        public void SetAxis1(RelativeOrientation rel, Vector3 axis)
        {
            NativeMethods.dJointSetAMotorAxis(id, 0, rel, axis.X, axis.Y, axis.Z);
        }

        /// <summary>
        /// Gets or sets the second angular motor axis.
        /// </summary>
        public void SetAxis2(RelativeOrientation rel, Vector3 axis)
        {
            NativeMethods.dJointSetAMotorAxis(id, 1, rel, axis.X, axis.Y, axis.Z);
        }

        /// <summary>
        /// Gets or sets the third angular motor axis.
        /// </summary>
        public void SetAxis3(RelativeOrientation rel, Vector3 axis)
        {
            NativeMethods.dJointSetAMotorAxis(id, 2, rel, axis.X, axis.Y, axis.Z);
        }

        class AngularMotorLimitMotor : JointLimitMotor
        {
            internal AngularMotorLimitMotor(AngularMotor joint, int axis)
                : base(joint, axis)
            {
            }

            internal override dReal GetParam(dJointID id, dJointParam parameter)
            {
                return NativeMethods.dJointGetAMotorParam(id, parameter);
            }

            internal override void SetParam(dJointID id, dJointParam parameter, dReal value)
            {
                NativeMethods.dJointSetAMotorParam(id, parameter, value);
            }
        }
    }

    /// <summary>
    /// Specifies how the angular motor axes and joint angle settings
    /// are controlled.
    /// </summary>
    public enum AngularMotorMode
    {
        /// <summary>
        /// Specifies that the angular motor axes and joint angle settings are entirely
        /// controlled by the user. This is the default mode.
        /// </summary>
        User = 0,

        /// <summary>
        /// Specifies that both joint euler angles and axis 1 are automatically computed.
        /// The angular motor axes must be set correctly when in this mode.
        /// </summary>
        /// <remarks>
        /// When in euler angle mode, the joint axes must be set as follows:
        /// <list type="bullet">
        /// <item>
        /// <description>
        /// Only axes 0 and 2 need to be set. Axis 1 will be determined automatically
        /// at each time step.
        /// </description>
        /// </item>
        /// <item>
        /// <description>Axes 0 and 2 must be perpendicular to each other.</description>
        /// </item>
        /// <item>
        /// <description>
        /// Axis 0 must be anchored to the first body, axis 2 must be anchored
        /// to the second body.
        /// </description>
        /// </item>
        /// </list>
        /// 
        /// When this mode is initially set the current relative orientations
        /// of the bodies will correspond to all euler angles at zero.
        /// </remarks>
        Euler = 1
    }
}
