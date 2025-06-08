using Ode.Net.Native;
#if ODE_DOUBLE_PRECISION
using dReal = System.Double;
#else
using dReal = System.Single;
#endif

namespace Ode.Net.Joints
{
    /// <summary>
    /// Represents a slider joint.
    /// </summary>
    public sealed class Slider : Joint
    {
        readonly JointLimitMotor limitMotor;

        /// <summary>
        /// Initializes a new instance of the <see cref="Slider"/> class.
        /// </summary>
        /// <param name="world">The world on which to place the joint.</param>
        public Slider(World world)
            : this(world, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Slider"/> class on
        /// the specified <see cref="JointGroup"/>.
        /// </summary>
        /// <param name="world">The world on which to place the joint.</param>
        /// <param name="group">The joint group that will contain the joint.</param>
        public Slider(World world, JointGroup group)
            : base(NativeMethods.dJointCreateSlider(world.Id, dJointGroupID.Null), group)
        {
            limitMotor = new SliderLimitMotor(this, 0);
        }

        /// <summary>
        /// Gets or sets the slider axis, in world coordinates.
        /// </summary>
        public Vector3 Axis
        {
            get
            {
                Vector3 axis;
                NativeMethods.dJointGetSliderAxis(id, out axis);
                return axis;
            }
            set
            {
                NativeMethods.dJointSetSliderAxis(id, value.X, value.Y, value.Z);
            }
        }

        /// <summary>
        /// Gets the slider linear position (i.e. the slider's extension).
        /// </summary>
        public dReal Position
        {
            get { return NativeMethods.dJointGetSliderPosition(id); }
        }

        /// <summary>
        /// Gets the slider linear position time derivative.
        /// </summary>
        public dReal PositionRate
        {
            get { return NativeMethods.dJointGetSliderPositionRate(id); }
        }

        /// <summary>
        /// Gets the limit and motor parameters of the slider.
        /// </summary>
        public JointLimitMotor LimitMotor
        {
            get { return limitMotor; }
        }

        /// <summary>
        /// Sets the slider axis with a relative displacement vector.
        /// </summary>
        /// <param name="axis">The slider axis, in world frame coordinates.</param>
        /// <param name="displacement">The relative displacement vector of the axis.</param>
        public void SetAxisDelta(Vector3 axis, Vector3 displacement)
        {
            NativeMethods.dJointSetSliderAxisDelta(
                id, axis.X, axis.Y, axis.Z,
                displacement.X, displacement.Y, displacement.Z);
        }

        /// <summary>
        /// Applies the specified force in the slider's direction.
        /// </summary>
        /// <param name="force">The magnitude of the applied force.</param>
        public void AddForce(dReal force)
        {
            NativeMethods.dJointAddSliderForce(id, force);
        }

        class SliderLimitMotor : JointLimitMotor
        {
            internal SliderLimitMotor(Slider joint, int axis)
                : base(joint, axis)
            {
            }

            internal override dReal GetParam(dJointID id, dJointParam parameter)
            {
                return NativeMethods.dJointGetSliderParam(id, parameter);
            }

            internal override void SetParam(dJointID id, dJointParam parameter, dReal value)
            {
                NativeMethods.dJointSetSliderParam(id, parameter, value);
            }
        }
    }
}
