using System;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;
#if ODE_DOUBLE_PRECISION
using dReal = System.Double;
#else
using dReal = System.Single;
#endif

namespace Ode.Net.Native
{
    static partial class NativeMethods
    {
        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong dRandGetSeed();

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dRandSetSeed(ulong s);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int dTestRand();

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong dRand();

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int dRandInt(int n);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern dReal dRandReal();


        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dQFromAxisAndAngle(out Quaternion q, dReal ax, dReal ay, dReal az, dReal angle);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dRFromAxisAndAngle(out Matrix3 R, dReal ax, dReal ay, dReal az, dReal angle);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dRFromZAxis(out Matrix3 R, dReal ax, dReal ay, dReal az);


        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern dThreadingImplementationID dThreadingAllocateSelfThreadedImplementation();

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern dThreadingImplementationID dThreadingAllocateMultiThreadedImplementation();

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr dThreadingImplementationGetFunctions(dThreadingImplementationID impl);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dThreadingImplementationShutdownProcessing(dThreadingImplementationID impl);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dThreadingFreeImplementation(IntPtr impl);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern dThreadingThreadPoolID dThreadingAllocateThreadPool(uint thread_count, UIntPtr stack_size, AllocateDataFlags ode_data_allocate_flags, IntPtr reserved/*=NULL*/);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dThreadingThreadPoolServeMultiThreadedImplementation(dThreadingThreadPoolID pool, dThreadingImplementationID impl);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dThreadingFreeThreadPool(IntPtr pool);
    }

    class dThreadingImplementationID : SafeHandleZeroOrMinusOneIsInvalid
    {
        internal static readonly dThreadingImplementationID Null = new NulldThreadingImplementationID();

        internal dThreadingImplementationID()
            : base(true)
        {
        }

        private dThreadingImplementationID(bool ownsHandle)
            : base(ownsHandle)
        {
        }

        protected override bool ReleaseHandle()
        {
            NativeMethods.dThreadingFreeImplementation(handle);
            return true;
        }

        class NulldThreadingImplementationID : dThreadingImplementationID
        {
            public NulldThreadingImplementationID()
                : base(false)
            {
            }

            protected override bool ReleaseHandle()
            {
                return false;
            }
        }
    }

    class dThreadingThreadPoolID : SafeHandleZeroOrMinusOneIsInvalid
    {
        internal static readonly dThreadingThreadPoolID Null = new NulldThreadingThreadPoolID();

        internal dThreadingThreadPoolID()
            : base(true)
        {
        }

        private dThreadingThreadPoolID(bool ownsHandle)
            : base(ownsHandle)
        {
        }

        protected override bool ReleaseHandle()
        {
            NativeMethods.dThreadingFreeThreadPool(handle);
            return true;
        }

        class NulldThreadingThreadPoolID : dThreadingThreadPoolID
        {
            public NulldThreadingThreadPoolID()
                : base(false)
            {
            }

            protected override bool ReleaseHandle()
            {
                return false;
            }
        }
    }
}
