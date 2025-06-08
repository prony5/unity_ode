using Ode.Net.Native;
#if ODE_DOUBLE_PRECISION
using dReal = System.Double;
#else
using dReal = System.Single;
#endif

namespace Ode.Net.Joints
{
    /// <summary>
    /// Represents a joint used to maintain a fixed relative position and orientation
    /// between two bodies, or between a body and the static environment.
    /// </summary>
    public sealed class Fixed : Joint
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Fixed"/> class.
        /// </summary>
        /// <param name="world">The world on which to place the joint.</param>
        public Fixed(World world)
            : this(world, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Fixed"/> class on
        /// the specified <see cref="JointGroup"/>.
        /// </summary>
        /// <param name="world">The world on which to place the joint.</param>
        /// <param name="group">The joint group that will contain the joint.</param>
        public Fixed(World world, JointGroup group)
            : base(NativeMethods.dJointCreateFixed(world.Id, dJointGroupID.Null), group)
        {
        }

        /// <summary>
        /// Gets or sets the constraint mixing factor used by the joint.
        /// </summary>
        public dReal Cfm
        {
            get { return NativeMethods.dJointGetFixedParam(id, dJointParam.dParamCFM); }
            set { NativeMethods.dJointSetFixedParam(id, dJointParam.dParamCFM, value); }
        }

        /// <summary>
        /// Gets or sets the error reduction parameter used by the joint.
        /// </summary>
        public dReal Erp
        {
            get { return NativeMethods.dJointGetFixedParam(id, dJointParam.dParamERP); }
            set { NativeMethods.dJointSetFixedParam(id, dJointParam.dParamERP, value); }
        }

        /// <summary>
        /// Remembers the current desired relative offset and desired relative rotation
        /// between attached bodies.
        /// </summary>
        public void Fix()
        {
            NativeMethods.dJointSetFixed(id);
        }
    }
}
