using Ode.Net.Native;
#if ODE_DOUBLE_PRECISION
using dReal = System.Double;
#else
using dReal = System.Single;
#endif

namespace Ode.Net.Joints
{
    /// <summary>
    /// Represents a ball and socket joint.
    /// </summary>
    public sealed class Ball : Joint
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Ball"/> class.
        /// </summary>
        /// <param name="world">The world on which to place the joint.</param>
        public Ball(World world)
            : this(world, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Ball"/> class on
        /// the specified <see cref="JointGroup"/>.
        /// </summary>
        /// <param name="world">The world on which to place the joint.</param>
        /// <param name="group">The joint group that will contain the joint.</param>
        public Ball(World world, JointGroup group)
            : base(NativeMethods.dJointCreateBall(world.Id, dJointGroupID.Null), group)
        {
        }

        /// <summary>
        /// Gets or sets the joint anchor point, in world coordinates.
        /// </summary>
        public Vector3 Anchor
        {
            get
            {
                Vector3 anchor;
                NativeMethods.dJointGetBallAnchor(id, out anchor);
                return anchor;
            }
            set
            {
                NativeMethods.dJointSetBallAnchor(id, value.X, value.Y, value.Z);
            }
        }

        /// <summary>
        /// Gets or sets the anchor point on the second body, in world coordinates. If the
        /// joint is perfectly satisfied, this will be the same value as <see cref="Anchor"/>.
        /// </summary>
        public Vector3 Anchor2
        {
            get
            {
                Vector3 anchor;
                NativeMethods.dJointGetBallAnchor2(id, out anchor);
                return anchor;
            }
            set
            {
                NativeMethods.dJointSetBallAnchor2(id, value.X, value.Y, value.Z);
            }
        }

        /// <summary>
        /// Gets or sets the constraint mixing factor used by the joint.
        /// </summary>
        public dReal Cfm
        {
            get { return NativeMethods.dJointGetBallParam(id, dJointParam.dParamCFM); }
            set { NativeMethods.dJointSetBallParam(id, dJointParam.dParamCFM, value); }
        }

        /// <summary>
        /// Gets or sets the error reduction parameter used by the joint.
        /// </summary>
        public dReal Erp
        {
            get { return NativeMethods.dJointGetBallParam(id, dJointParam.dParamERP); }
            set { NativeMethods.dJointSetBallParam(id, dJointParam.dParamERP, value); }
        }
    }
}
