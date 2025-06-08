using Ode.Net.Native;

namespace Ode.Net.Joints
{
    /// <summary>
    /// Represents a null (logical) joint.
    /// </summary>
    public sealed class Null : Joint
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Null"/> class.
        /// </summary>
        /// <param name="world">The world on which to place the joint.</param>
        public Null(World world)
            : this(world, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Null"/> class on
        /// the specified <see cref="JointGroup"/>.
        /// </summary>
        /// <param name="world">The world on which to place the joint.</param>
        /// <param name="group">The joint group that will contain the joint.</param>
        public Null(World world, JointGroup group)
            : base(NativeMethods.dJointCreateNull(world.Id, dJointGroupID.Null), group)
        {
        }
    }
}
