using Microsoft.Win32.SafeHandles;

namespace Ode.Net.Native
{
    class dWorldID : SafeHandleZeroOrMinusOneIsInvalid
    {
        internal static readonly dWorldID Null = new NulldWorldID();

        internal dWorldID()
            : base(true)
        {
        }

        private dWorldID(bool ownsHandle)
            : base(ownsHandle)
        {
        }

        protected override bool ReleaseHandle()
        {
            NativeMethods.dWorldDestroy(handle);
            return true;
        }

        class NulldWorldID : dWorldID
        {
            public NulldWorldID()
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
