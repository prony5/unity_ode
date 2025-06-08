using Ode.Net.Native;

namespace Ode.Net.Joints
{
    /// <summary>
    /// Represents a joint used to constrain a body to a bidimensional plane.
    /// </summary>
    public sealed class Plane2D : Joint
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Plane2D"/> class.
        /// </summary>
        /// <param name="world">The world on which to place the joint.</param>
        public Plane2D(World world)
            : this(world, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Plane2D"/> class on
        /// the specified <see cref="JointGroup"/>.
        /// </summary>
        /// <param name="world">The world on which to place the joint.</param>
        /// <param name="group">The joint group that will contain the joint.</param>
        public Plane2D(World world, JointGroup group)
            : base(NativeMethods.dJointCreatePlane2D(world.Id, dJointGroupID.Null), group)
        {
        }

        /// <summary>
        /// Attaches the joint to the specified body.
        /// </summary>
        /// <param name="body">The body to constrain to the bidimensional plane.</param>
        public void Attach(Body body)
        {
            Attach(body, null);
        }
    }
}
