using Microsoft.Win32.SafeHandles;

namespace Ode.Net.Native
{
    class dHeightfieldDataID : SafeHandleZeroOrMinusOneIsInvalid
    {
        internal dHeightfieldDataID()
            : base(true)
        {
        }

        private dHeightfieldDataID(bool ownsHandle)
            : base(ownsHandle)
        {
        }

        protected override bool ReleaseHandle()
        {
            NativeMethods.dGeomHeightfieldDataDestroy(handle);
            return true;
        }
    }
}
