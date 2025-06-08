using System.Runtime.InteropServices;

namespace Ode.Net.Native
{
    static partial class NativeMethods
    {
        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int dInitODE2(InitFlags uiInitFlags);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int dAllocateODEDataForThread(AllocateDataFlags uiAllocateFlags);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dCleanupODEAllDataForThread();

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dCloseODE();
    }
}
