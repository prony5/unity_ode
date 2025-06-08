using Ode.Net.Native;
#if ODE_DOUBLE_PRECISION
using dReal = System.Double;
#else
using dReal = System.Single;
#endif

namespace Ode.Net
{
    /// <summary>
    /// Random number generator.
    /// </summary>
    public static class Rand
    {
        /// <summary>
        /// Gets and Sets the current random number seed.
        /// </summary>
        public static ulong Seed
        {
            get { return NativeMethods.dRandGetSeed(); }
            set { NativeMethods.dRandSetSeed(value); }
        }

        /// <summary>
        /// Returns 1 if the random number generator is working.
        /// </summary>
        public static int Test()
        {
            return NativeMethods.dTestRand();
        }

        /// <summary>
        /// Returns next 32 bit random number. this uses a not-very-random linear congruential method.
        /// </summary>
        public static ulong Next()
        {
            return NativeMethods.dRand();
        }

        /// <summary>
        /// Returns a random integer between 0..n-1. the distribution will get worse as n approaches 2^32.
        /// </summary>
        public static int NextInt(int n)
        {
            return NativeMethods.dRandInt(n);
        }

        /// <summary>
        /// Returns a random real number between 0..1
        /// </summary>
        /// <returns></returns>
        public static dReal NextReal()
        {
            return NativeMethods.dRandReal();
        }
    }
}