using Ode.Net.Native;
#if ODE_DOUBLE_PRECISION
using dReal = System.Double;
#else
using dReal = System.Single;
#endif

namespace Ode.Net.Joints
{
    /// <summary>
    /// Represents a double ball joint.
    /// </summary>
    public sealed class DoubleBall : Joint
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DoubleBall"/> class.
        /// </summary>
        /// <param name="world">The world on which to place the joint.</param>
        public DoubleBall(World world)
            : this(world, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DoubleBall"/> class on
        /// the specified <see cref="JointGroup"/>.
        /// </summary>
        /// <param name="world">The world on which to place the joint.</param>
        /// <param name="group">The joint group that will contain the joint.</param>
        public DoubleBall(World world, JointGroup group)
            : base(NativeMethods.dJointCreateDBall(world.Id, dJointGroupID.Null), group)
        {
        }

        /// <summary>
        /// Gets or sets the first anchor for the double ball joint.
        /// </summary>
        public Vector3 Anchor1
        {
            get
            {
                Vector3 anchor;
                NativeMethods.dJointGetDBallAnchor1(id, out anchor);
                return anchor;
            }
            set
            {
                NativeMethods.dJointSetDBallAnchor1(id, value.X, value.Y, value.Z);
            }
        }

        /// <summary>
        /// Gets or sets the second anchor for the double ball joint.
        /// </summary>
        public Vector3 Anchor2
        {
            get
            {
                Vector3 anchor;
                NativeMethods.dJointGetDBallAnchor2(id, out anchor);
                return anchor;
            }
            set
            {
                NativeMethods.dJointSetDBallAnchor2(id, value.X, value.Y, value.Z);
            }
        }

        /// <summary>
        /// Gets the set distance from the double ball joint.
        /// </summary>
        public dReal Distance
        {
            get { return NativeMethods.dJointGetDBallDistance(id); }
        }

        /// <summary>
        /// Gets or sets the constraint mixing factor used by the joint.
        /// </summary>
        public dReal Cfm
        {
            get { return NativeMethods.dJointGetDBallParam(id, dJointParam.dParamCFM); }
            set { NativeMethods.dJointSetDBallParam(id, dJointParam.dParamCFM, value); }
        }

        /// <summary>
        /// Gets or sets the error reduction parameter used by the joint.
        /// </summary>
        public dReal Erp
        {
            get { return NativeMethods.dJointGetDBallParam(id, dJointParam.dParamERP); }
            set { NativeMethods.dJointSetDBallParam(id, dJointParam.dParamERP, value); }
        }
    }
}
