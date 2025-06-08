using Ode.Net.Native;
#if ODE_DOUBLE_PRECISION
using dReal = System.Double;
#else
using dReal = System.Single;
#endif

namespace Ode.Net.Joints
{
    /// <summary>
    /// Represents a double hinge joint.
    /// </summary>
    public sealed class DoubleHinge : Joint
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DoubleHinge"/> class.
        /// </summary>
        /// <param name="world">The world on which to place the joint.</param>
        public DoubleHinge(World world)
            : this(world, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DoubleHinge"/> class on
        /// the specified <see cref="JointGroup"/>.
        /// </summary>
        /// <param name="world">The world on which to place the joint.</param>
        /// <param name="group">The joint group that will contain the joint.</param>
        public DoubleHinge(World world, JointGroup group)
            : base(NativeMethods.dJointCreateDHinge(world.Id, dJointGroupID.Null), group)
        {
        }

        /// <summary>
        /// Gets or sets the first anchor for the double hinge joint.
        /// </summary>
        public Vector3 Anchor1
        {
            get
            {
                Vector3 anchor;
                NativeMethods.dJointGetDHingeAnchor1(id, out anchor);
                return anchor;
            }
            set
            {
                NativeMethods.dJointSetDHingeAnchor1(id, value.X, value.Y, value.Z);
            }
        }

        /// <summary>
        /// Gets or sets the second anchor for the double hinge joint.
        /// </summary>
        public Vector3 Anchor2
        {
            get
            {
                Vector3 anchor;
                NativeMethods.dJointGetDHingeAnchor2(id, out anchor);
                return anchor;
            }
            set
            {
                NativeMethods.dJointSetDHingeAnchor2(id, value.X, value.Y, value.Z);
            }
        }

        /// <summary>
        /// Gets or sets the axis for the double hinge joint.
        /// </summary>
        public Vector3 Axis
        {
            get
            {
                Vector3 axis;
                NativeMethods.dJointGetDHingeAxis(id, out axis);
                return axis;
            }
            set
            {
                NativeMethods.dJointSetDHingeAxis(id, value.X, value.Y, value.Z);
            }
        }

        /// <summary>
        /// Gets the set distance from the double hinge joint.
        /// </summary>
        public dReal Distance
        {
            get { return NativeMethods.dJointGetDHingeDistance(id); }
        }

        /// <summary>
        /// Gets or sets the constraint mixing factor used by the joint.
        /// </summary>
        public dReal Cfm
        {
            get { return NativeMethods.dJointGetDHingeParam(id, dJointParam.dParamCFM); }
            set { NativeMethods.dJointSetDHingeParam(id, dJointParam.dParamCFM, value); }
        }

        /// <summary>
        /// Gets or sets the error reduction parameter used by the joint.
        /// </summary>
        public dReal Erp
        {
            get { return NativeMethods.dJointGetDHingeParam(id, dJointParam.dParamERP); }
            set { NativeMethods.dJointSetDHingeParam(id, dJointParam.dParamERP, value); }
        }
    }
}
