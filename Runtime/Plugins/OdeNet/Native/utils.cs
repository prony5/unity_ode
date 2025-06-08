using System;
using System.Runtime.InteropServices;
#if ODE_DOUBLE_PRECISION
using dReal = System.Double;
#else
using dReal = System.Single;
#endif

namespace Ode.Net.Native
{
    static class utils
    {
        internal static Vector3 PtrToVector3(IntPtr vecPtr)
        {
            var vec = new dReal[3];
            Marshal.Copy(vecPtr, vec, 0, vec.Length);
            return new Vector3(vec[0], vec[1], vec[2]);
        }

        internal static void PtrToVector3(IntPtr vecPtr, out Vector3 vector)
        {
            var vec = new dReal[3];
            Marshal.Copy(vecPtr, vec, 0, vec.Length);
            vector.X = vec[0];
            vector.Y = vec[1];
            vector.Z = vec[2];
            vector.W = 0;
        }
    }
}
