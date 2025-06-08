using System;
using System.Runtime.InteropServices;

namespace Ode.Net.Native
{
    static partial class NativeMethods
    {
#if UNITY_IOS && !UNITY_EDITOR
        public const string libName = "__Internal";
#else
#if ODE_DOUBLE_PRECISION
        public const string libName = "ode_double";
#else
        public const string libName = "ode_single";
#endif

#endif

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr dGetConfiguration();

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern int dCheckConfiguration(string token);
    }
}
