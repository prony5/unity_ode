using Microsoft.Win32.SafeHandles;

namespace Ode.Net.Native
{
    class dJointID : SafeHandleZeroOrMinusOneIsInvalid
    {
        internal dJointID()
            : base(true)
        {
        }

        private dJointID(bool ownsHandle)
            : base(ownsHandle)
        {
        }

        protected override bool ReleaseHandle()
        {
            NativeMethods.dJointDestroy(handle);
            return true;
        }
    }
}
