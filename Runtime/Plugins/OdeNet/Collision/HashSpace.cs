using Ode.Net.Native;

namespace Ode.Net.Collision
{
    /// <summary>
    /// Represents a multi-resolution hash table collision space.
    /// </summary>
    public sealed class HashSpace : Space
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HashSpace"/> class.
        /// </summary>
        public HashSpace()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HashSpace"/> class.
        /// inside the specified space.
        /// </summary>
        /// <param name="space">The space which will contain the new hash space.</param>
        public HashSpace(Space space)
            : base(NativeMethods.dHashSpaceCreate(space != null ? space.Id : dSpaceID.Null))
        {
        }

        /// <summary>
        /// Gets or sets the minimum cell size level of the hash space.
        /// </summary>
        public int MinLevel
        {
            get
            {
                int minLevel, maxLevel;
                NativeMethods.dHashSpaceGetLevels(Id, out minLevel, out maxLevel);
                return minLevel;
            }
            set
            {
                int minlevel, maxlevel;
                NativeMethods.dHashSpaceGetLevels(Id, out minlevel, out maxlevel);
                NativeMethods.dHashSpaceSetLevels(Id, value, maxlevel);
            }
        }

        /// <summary>
        /// Gets or sets the maximum cell size level of the hash space.
        /// </summary>
        public int MaxLevel
        {
            get
            {
                int minLevel, maxLevel;
                NativeMethods.dHashSpaceGetLevels(Id, out minLevel, out maxLevel);
                return maxLevel;
            }
            set
            {
                int minlevel, maxlevel;
                NativeMethods.dHashSpaceGetLevels(Id, out minlevel, out maxlevel);
                NativeMethods.dHashSpaceSetLevels(Id, minlevel, value);
            }
        }
    }
}
