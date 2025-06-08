using Microsoft.Win32.SafeHandles;

namespace Ode.Net.Native
{
    class dGeomID : SafeHandleZeroOrMinusOneIsInvalid
    {
        internal dGeomID()
            : base(true)
        {
        }

        internal dGeomID(bool ownsHandle)
            : base(ownsHandle)
        {
        }

        protected override bool ReleaseHandle()
        {
            NativeMethods.dGeomDestroy(handle);
            return true;
        }
    }
}
