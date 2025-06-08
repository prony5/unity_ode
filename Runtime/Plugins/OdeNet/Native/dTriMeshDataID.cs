using Microsoft.Win32.SafeHandles;

namespace Ode.Net.Native
{
    class dTriMeshDataID : SafeHandleZeroOrMinusOneIsInvalid
    {
        internal dTriMeshDataID()
            : base(true)
        {
        }

        private dTriMeshDataID(bool ownsHandle)
            : base(ownsHandle)
        {
        }

        protected override bool ReleaseHandle()
        {
            NativeMethods.dGeomTriMeshDataDestroy(handle);
            return true;
        }
    }
}
