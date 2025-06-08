using System;
using System.Runtime.InteropServices;

namespace Ode.Net.Native
{
    static partial class NativeMethods
    {
        #region Error Handlers

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dSetErrorHandler(dMessageFunction fn);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dSetDebugHandler(dMessageFunction fn);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dSetMessageHandler(dMessageFunction fn);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr dGetErrorHandler();

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr dGetDebugHandler();

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr dGetMessageHandler();

        #endregion
    }
}
