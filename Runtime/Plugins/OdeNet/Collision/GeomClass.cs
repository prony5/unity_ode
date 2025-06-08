using Ode.Net.Native;

namespace Ode.Net.Collision
{
    /// <summary>
    /// Specifies the class identifier of a geom.
    /// </summary>
    public enum GeomClass
    {
        /// <summary>
        /// A sphere geom.
        /// </summary>
        Sphere = 0,

        /// <summary>
        /// A box geom.
        /// </summary>
        Box,

        /// <summary>
        /// A capsule geom.
        /// </summary>
        Capsule,

        /// <summary>
        /// A regular flat-ended cylinder geom.
        /// </summary>
        Cylinder,

        /// <summary>
        /// An infinite (non-placeable) plane geom.
        /// </summary>
        Plane,

        /// <summary>
        /// A ray geom.
        /// </summary>
        Ray,

        /// <summary>
        /// A convex hull geom.
        /// </summary>
        Convex,

        /// <summary>
        /// A geometry transform.
        /// </summary>
        GeomTransform,

        /// <summary>
        /// A triangle mesh geom.
        /// </summary>
        TriMesh,

        /// <summary>
        /// A heightfield geom.
        /// </summary>
        Heightfield,

        /// <summary>
        /// The identifier of the first space class.
        /// </summary>
        FirstSpace,

        /// <summary>
        /// A simple collision space.
        /// </summary>
        SimpleSpace = FirstSpace,

        /// <summary>
        /// A multi-resolution hash table collision space.
        /// </summary>
        HashSpace,

        /// <summary>
        /// A sweep-and-prune space.
        /// </summary>
        SweepAndPruneSpace,

        /// <summary>
        /// A quadtree space.
        /// </summary>
        QuadTreeSpace,

        /// <summary>
        /// The identifier of the last space class.
        /// </summary>
        LastSpace = QuadTreeSpace,

        /// <summary>
        /// The identifier of the first user class.
        /// </summary>
        FirstUserClass,

        /// <summary>
        /// The identifier of the last user class.
        /// </summary>
        LastUserClass = FirstUserClass + NativeMethods.MaxUserClasses - 1,

        /// <summary>
        /// The number of geometry classes.
        /// </summary>
        GeomNumClasses
    }
}
