using Ode.Net.Native;
#if ODE_DOUBLE_PRECISION
using dReal = System.Double;
#else
using dReal = System.Single;
#endif

namespace Ode.Net.Joints
{
    /// <summary>
    /// Represents a linear motor joint.
    /// </summary>
    public sealed class LinearMotor : Joint
    {
        readonly JointLimitMotor limitMotor1;
        readonly JointLimitMotor limitMotor2;
        readonly JointLimitMotor limitMotor3;

        /// <summary>
        /// Initializes a new instance of the <see cref="LinearMotor"/> class.
        /// </summary>
        /// <param name="world">The world on which to place the joint.</param>
        public LinearMotor(World world)
            : this(world, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LinearMotor"/> class on
        /// the specified <see cref="JointGroup"/>.
        /// </summary>
        /// <param name="world">The world on which to place the joint.</param>
        /// <param name="group">The joint group that will contain the joint.</param>
        public LinearMotor(World world, JointGroup group)
            : base(NativeMethods.dJointCreateLMotor(world.Id, dJointGroupID.Null), group)
        {
            limitMotor1 = new LinearMotorLimitMotor(this, 0);
            limitMotor2 = new LinearMotorLimitMotor(this, 1);
            limitMotor3 = new LinearMotorLimitMotor(this, 2);
        }

        /// <summary>
        /// Gets or sets the number of axes that will be controlled by the linear motor.
        /// </summary>
        public int NumAxes
        {
            get { return NativeMethods.dJointGetLMotorNumAxes(id); }
            set { NativeMethods.dJointSetLMotorNumAxes(id, value); }
        }

        /// <summary>
        /// Gets or sets the first linear motor axis.
        /// </summary>
        public Vector3 Axis1
        {
            get
            {
                Vector3 axis;
                NativeMethods.dJointGetLMotorAxis(id, 0, out axis);
                return axis;
            }
            set
            {
                NativeMethods.dJointSetLMotorAxis(id, 0, RelativeOrientation.Global, value.X, value.Y, value.Z);
            }
        }

        /// <summary>
        /// Gets or sets the second linear motor axis.
        /// </summary>
        public Vector3 Axis2
        {
            get
            {
                Vector3 axis;
                NativeMethods.dJointGetLMotorAxis(id, 1, out axis);
                return axis;
            }
            set
            {
                NativeMethods.dJointSetLMotorAxis(id, 1, RelativeOrientation.Global, value.X, value.Y, value.Z);
            }
        }

        /// <summary>
        /// Gets or sets the third linear motor axis.
        /// </summary>
        public Vector3 Axis3
        {
            get
            {
                Vector3 axis;
                NativeMethods.dJointGetLMotorAxis(id, 2, out axis);
                return axis;
            }
            set
            {
                NativeMethods.dJointSetLMotorAxis(id, 2, RelativeOrientation.Global, value.X, value.Y, value.Z);
            }
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
        /// Sets the specified linear motor axis using a specific relative orientation mode.
        /// </summary>
        /// <param name="index">The index of the linear motor axis to set.</param>
        /// <param name="relativeMode">The relative orientation mode of the specified axis.</param>
        /// <param name="axis">The linear motor axis.</param>
        public void SetAxis(int index, RelativeOrientation relativeMode, Vector3 axis)
        {
            NativeMethods.dJointSetLMotorAxis(id, index, relativeMode, axis.X, axis.Y, axis.Z);
        }

        /// <summary>
        /// Sets the specified linear motor axis using a specific relative orientation mode.
        /// </summary>
        /// <param name="index">The index of the linear motor axis to set.</param>
        /// <param name="relativeMode">The relative orientation mode of the specified axis.</param>
        /// <param name="ax">The x-component of the linear motor axis.</param>
        /// <param name="ay">The y-component of the linear motor axis.</param>
        /// <param name="az">The z-component of the linear motor axis.</param>
        public void SetAxis(int index, RelativeOrientation relativeMode, dReal ax, dReal ay, dReal az)
        {
            NativeMethods.dJointSetLMotorAxis(id, index, relativeMode, ax, ay, az);
        }

        class LinearMotorLimitMotor : JointLimitMotor
        {
            internal LinearMotorLimitMotor(LinearMotor joint, int axis)
                : base(joint, axis)
            {
            }

            internal override dReal GetParam(dJointID id, dJointParam parameter)
            {
                return NativeMethods.dJointGetLMotorParam(id, parameter);
            }

            internal override void SetParam(dJointID id, dJointParam parameter, dReal value)
            {
                NativeMethods.dJointSetLMotorParam(id, parameter, value);
            }
        }
    }
}
