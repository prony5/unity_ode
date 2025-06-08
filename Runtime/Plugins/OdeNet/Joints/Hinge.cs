using Ode.Net.Native;
#if ODE_DOUBLE_PRECISION
using dReal = System.Double;
#else
using dReal = System.Single;
#endif

namespace Ode.Net.Joints
{
    /// <summary>
    /// Represents a hinge joint.
    /// </summary>
    public sealed class Hinge : Joint
    {
        readonly JointLimitMotor limitMotor;

        /// <summary>
        /// Initializes a new instance of the <see cref="Hinge"/> class.
        /// </summary>
        /// <param name="world">The world on which to place the joint.</param>
        public Hinge(World world)
            : this(world, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Hinge"/> class on
        /// the specified <see cref="JointGroup"/>.
        /// </summary>
        /// <param name="world">The world on which to place the joint.</param>
        /// <param name="group">The joint group that will contain the joint.</param>
        public Hinge(World world, JointGroup group)
            : base(NativeMethods.dJointCreateHinge(world.Id, dJointGroupID.Null), group)
        {
            limitMotor = new HingeLimitMotor(this, 0);
        }

        /// <summary>
        /// Gets or sets the joint anchor point, in world coordinates.
        /// </summary>
        public Vector3 Anchor
        {
            get
            {
                Vector3 anchor;
                NativeMethods.dJointGetHingeAnchor(id, out anchor);
                return anchor;
            }
            set
            {
                NativeMethods.dJointSetHingeAnchor(id, value.X, value.Y, value.Z);
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
                NativeMethods.dJointGetHingeAnchor2(id, out anchor);
                return anchor;
            }
        }

        /// <summary>
        /// Gets or sets the hinge rotation axis, in world coordinates.
        /// </summary>
        public Vector3 Axis
        {
            get
            {
                Vector3 axis;
                NativeMethods.dJointGetHingeAxis(id, out axis);
                return axis;
            }
            set
            {
                NativeMethods.dJointSetHingeAxis(id, value.X, value.Y, value.Z);
            }
        }

        /// <summary>
        /// Gets the angle measured between the two bodies with respect
        /// to the hinge axis.
        /// </summary>
        public dReal Angle
        {
            get { return NativeMethods.dJointGetHingeAngle(id); }
        }

        /// <summary>
        /// Gets the hinge angle time derivative.
        /// </summary>
        public dReal AngleRate
        {
            get { return NativeMethods.dJointGetHingeAngleRate(id); }
        }

        /// <summary>
        /// Gets the limit and motor parameters of the hinge.
        /// </summary>
        public JointLimitMotor LimitMotor
        {
            get { return limitMotor; }
        }

        /// <summary>
        /// Sets the hinge anchor point with a relative displacement vector.
        /// </summary>
        /// <param name="anchor">The hinge anchor point, in world frame coordinates.</param>
        /// <param name="displacement">
        /// The relative displacement vector between the passive body and the anchor.
        /// </param>
        public void SetAnchorDelta(Vector3 anchor, Vector3 displacement)
        {
            NativeMethods.dJointSetHingeAnchorDelta(
                id, anchor.X, anchor.Y, anchor.Z,
                displacement.X, displacement.Y, displacement.Z);
        }

        /// <summary>
        /// Sets the hinge axis as if the two bodies were already apart by the
        /// specified angle.
        /// </summary>
        /// <param name="axis">The hinge axis, in world frame coordinates.</param>
        /// <param name="angle">
        /// The angle for the offset of the relative orientation between the two bodies.
        /// The rotation is specified about the new hinge axis.
        /// </param>
        public void SetAxisOffset(Vector3 axis, dReal angle)
        {
            NativeMethods.dJointSetHingeAxisOffset(id, axis.X, axis.Y, axis.Z, angle);
        }

        /// <summary>
        /// Applies the specified torque about the hinge axis.
        /// </summary>
        /// <param name="torque">The magnitude of the applied torque.</param>
        public void AddTorque(dReal torque)
        {
            NativeMethods.dJointAddHingeTorque(id, torque);
        }

        class HingeLimitMotor : JointLimitMotor
        {
            internal HingeLimitMotor(Hinge joint, int axis)
                : base(joint, axis)
            {
            }

            internal override dReal GetParam(dJointID id, dJointParam parameter)
            {
                return NativeMethods.dJointGetHingeParam(id, parameter);
            }

            internal override void SetParam(dJointID id, dJointParam parameter, dReal value)
            {
                NativeMethods.dJointSetHingeParam(id, parameter, value);
            }
        }
    }
}
