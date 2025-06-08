using Ode.Net.Native;
#if ODE_DOUBLE_PRECISION
using dReal = System.Double;
#else
using dReal = System.Single;
#endif

namespace Ode.Net.Joints
{
    /// <summary>
    /// Represents a transmission joint.
    /// </summary>
    public sealed class Transmission : Joint
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Transmission"/> class.
        /// </summary>
        /// <param name="world">The world on which to place the joint.</param>
        public Transmission(World world)
            : this(world, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Transmission"/> class on
        /// the specified <see cref="JointGroup"/>.
        /// </summary>
        /// <param name="world">The world on which to place the joint.</param>
        /// <param name="group">The joint group that will contain the joint.</param>
        public Transmission(World world, JointGroup group)
            : base(NativeMethods.dJointCreateTransmission(world.Id, dJointGroupID.Null), group)
        {
        }

        /// <summary>
        /// Gets or sets the first transmission anchor. This is the point of attachment
        /// of the wheel on the first body, in world coordinates.
        /// </summary>
        public Vector3 Anchor1
        {
            get
            {
                Vector3 anchor;
                NativeMethods.dJointGetTransmissionAnchor1(id, out anchor);
                return anchor;
            }
            set
            {
                NativeMethods.dJointSetTransmissionAnchor1(id, value.X, value.Y, value.Z);
            }
        }

        /// <summary>
        /// Gets or sets the second transmission anchor. This is the point of attachment
        /// of the wheel on the second body, in world coordinates.
        /// </summary>
        public Vector3 Anchor2
        {
            get
            {
                Vector3 anchor;
                NativeMethods.dJointGetTransmissionAnchor2(id, out anchor);
                return anchor;
            }
            set
            {
                NativeMethods.dJointSetTransmissionAnchor2(id, value.X, value.Y, value.Z);
            }
        }

        /// <summary>
        /// Gets or sets the common axis for both wheels of the transmission joint,
        /// in world coordinates.
        /// </summary>
        public Vector3 Axis
        {
            get
            {
                Vector3 axis;
                NativeMethods.dJointGetTransmissionAxis(id, out axis);
                return axis;
            }
            set
            {
                NativeMethods.dJointSetTransmissionAxis(id, value.X, value.Y, value.Z);
            }
        }

        /// <summary>
        /// Gets or sets the first axis for the transmission joint, in world coordinates.
        /// This is the axis around which the first body is allowed to revolve and is
        /// attached to.
        /// </summary>
        /// <remarks>
        /// This axis can only be set explicitly in intersecting-axes mode. For the
        /// parallel-axes and chain modes which share one common axis of revolution for
        /// both gears, the <see cref="Axis"/> property should be used.
        /// </remarks>
        public Vector3 Axis1
        {
            get
            {
                Vector3 axis;
                NativeMethods.dJointGetTransmissionAxis1(id, out axis);
                return axis;
            }
            set
            {
                NativeMethods.dJointSetTransmissionAxis1(id, value.X, value.Y, value.Z);
            }
        }

        /// <summary>
        /// Gets or sets the second axis for the transmission joint, in world coordinates.
        /// This is the axis around which the second body is allowed to revolve and is
        /// attached to.
        /// </summary>
        /// <remarks>
        /// This axis can only be set explicitly in intersecting-axes mode. For the
        /// parallel-axes and chain modes which share one common axis of revolution for
        /// both gears, the <see cref="Axis"/> property should be used.
        /// </remarks>
        public Vector3 Axis2
        {
            get
            {
                Vector3 axis;
                NativeMethods.dJointGetTransmissionAxis2(id, out axis);
                return axis;
            }
            set
            {
                NativeMethods.dJointSetTransmissionAxis2(id, value.X, value.Y, value.Z);
            }
        }

        /// <summary>
        /// Gets the contact point of the first wheel of the transmission joint.
        /// </summary>
        public Vector3 ContactPoint1
        {
            get
            {
                Vector3 contactPoint;
                NativeMethods.dJointGetTransmissionContactPoint1(id, out contactPoint);
                return contactPoint;
            }
        }

        /// <summary>
        /// Gets the contact point of the second wheel of the transmission joint.
        /// </summary>
        public Vector3 ContactPoint2
        {
            get
            {
                Vector3 contactPoint;
                NativeMethods.dJointGetTransmissionContactPoint2(id, out contactPoint);
                return contactPoint;
            }
        }

        /// <summary>
        /// Gets the traversed angle for the first wheel of the transmission joint.
        /// </summary>
        public dReal Angle1
        {
            get { return NativeMethods.dJointGetTransmissionAngle1(id); }
        }

        /// <summary>
        /// Gets the traversed angle for the second wheel of the transmission joint.
        /// </summary>
        public dReal Angle2
        {
            get { return NativeMethods.dJointGetTransmissionAngle2(id); }
        }

        /// <summary>
        /// Gets or sets the working mode of the transmission joint.
        /// </summary>
        public TransmissionMode Mode
        {
            get { return NativeMethods.dJointGetTransmissionMode(id); }
            set { NativeMethods.dJointSetTransmissionMode(id, value); }
        }

        /// <summary>
        /// Gets or sets the transmission ratio, defined as the ratio of the
        /// angular speed of the first gear to that of the second gear.
        /// </summary>
        /// <remarks>
        /// This property can only be set explicitly in parallel-axes mode.
        /// In intersecting-axes mode the ratio is defined implicitly by the
        /// initial configuration of the wheels and in chain mode it is
        /// defined implicitly by the wheel radii.
        /// </remarks>
        public dReal Ratio
        {
            get { return NativeMethods.dJointGetTransmissionRatio(id); }
            set { NativeMethods.dJointSetTransmissionRatio(id, value); }
        }

        /// <summary>
        /// Gets or sets the radius of the first wheel of the transmission joint.
        /// </summary>
        public dReal Radius1
        {
            get { return NativeMethods.dJointGetTransmissionRadius1(id); }
            set { NativeMethods.dJointSetTransmissionRadius1(id, value); }
        }

        /// <summary>
        /// Gets or sets the radius of the second wheel of the transmission joint.
        /// </summary>
        public dReal Radius2
        {
            get { return NativeMethods.dJointGetTransmissionRadius2(id); }
            set { NativeMethods.dJointSetTransmissionRadius2(id, value); }
        }

        /// <summary>
        /// Gets or sets the backlash of the transmission joint, in units of length.
        /// </summary>
        /// <remarks>
        /// Backlash is the clearance in the mesh of the wheels of the transmission
        /// and is defined as the maximum distance that the geometric contact point
        /// can travel without any actual contact or transfer of power between the wheels.
        /// This can be converted in degrees of revolution for each wheel by dividing by
        /// the wheel's radius. To further illustrate this consider the situation where a
        /// wheel of radius r_1 is driving another wheel of radius r_2 and there is an
        /// amount of backlash equal to b in their mesh. If the driving wheel were to
        /// instantaneously stop there would be no contact and hence the driven wheel
        /// would continue to turn for another b / r_2 radians until all the backlash
        /// in the mesh was taken up and contact restored with the relationship of
        /// driving and driven wheel reversed. The backlash is therefore given in units of
        /// length.
        /// </remarks>
        public dReal Backlash
        {
            get { return NativeMethods.dJointGetTransmissionBacklash(id); }
            set { NativeMethods.dJointSetTransmissionBacklash(id, value); }
        }

        /// <summary>
        /// Gets or sets the constraint mixing factor used by the joint.
        /// </summary>
        public dReal Cfm
        {
            get { return NativeMethods.dJointGetTransmissionParam(id, dJointParam.dParamCFM); }
            set { NativeMethods.dJointSetTransmissionParam(id, dJointParam.dParamCFM, value); }
        }

        /// <summary>
        /// Gets or sets the error reduction parameter used by the joint.
        /// </summary>
        public dReal Erp
        {
            get { return NativeMethods.dJointGetTransmissionParam(id, dJointParam.dParamERP); }
            set { NativeMethods.dJointSetTransmissionParam(id, dJointParam.dParamERP, value); }
        }
    }

    /// <summary>
    /// Specifies the working mode of the transmission joint.
    /// </summary>
    public enum TransmissionMode
    {
        /// <summary>
        /// Specifies that the transmission joint should simulate a set of
        /// parallel-axes gears.
        /// </summary>
        ParallelAxes,

        /// <summary>
        /// Specifies that the transmission joint should simulate a set of
        /// intersecting-axes beveled gears.
        /// </summary>
        IntersectingAxes,

        /// <summary>
        /// Specifies that the transmission joint should simulate a set of
        /// chain and sprockets.
        /// </summary>
        ChainDrive
    }
}
