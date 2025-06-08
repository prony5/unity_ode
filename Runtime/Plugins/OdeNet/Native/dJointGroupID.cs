using Microsoft.Win32.SafeHandles;

namespace Ode.Net.Native
{
    class dJointGroupID : SafeHandleZeroOrMinusOneIsInvalid
    {
        internal static readonly dJointGroupID Null = new NulldJointGroupID();

        internal dJointGroupID()
            : base(true)
        {
        }

        private dJointGroupID(bool ownsHandle)
            : base(ownsHandle)
        {
        }

        protected override bool ReleaseHandle()
        {
            NativeMethods.dJointGroupDestroy(handle);
            return true;
        }

        class NulldJointGroupID : dJointGroupID
        {
            public NulldJointGroupID()
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
