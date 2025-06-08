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
        internal static extern int dMassCheck(ref Mass m);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dMassSetZero(out Mass m);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dMassSetParameters(out Mass m, dReal themass,
                     dReal cgx, dReal cgy, dReal cgz,
                     dReal I11, dReal I22, dReal I33,
                     dReal I12, dReal I13, dReal I23);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dMassSetSphere(out Mass m, dReal density, dReal radius);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dMassSetSphereTotal(out Mass m, dReal total_mass, dReal radius);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dMassSetCapsule(out Mass m, dReal density, DirectionAxis direction,
                    dReal radius, dReal length);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dMassSetCapsuleTotal(out Mass m, dReal total_mass, DirectionAxis direction,
                    dReal radius, dReal length);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dMassSetCylinder(out Mass m, dReal density, DirectionAxis direction,
                       dReal radius, dReal length);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dMassSetCylinderTotal(out Mass m, dReal total_mass, DirectionAxis direction,
                        dReal radius, dReal length);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dMassSetBox(out Mass m, dReal density,
                  dReal lx, dReal ly, dReal lz);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dMassSetBoxTotal(out Mass m, dReal total_mass,
                       dReal lx, dReal ly, dReal lz);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dMassSetTrimesh(out Mass m, dReal density, dGeomID g);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dMassSetTrimeshTotal(out Mass m, dReal total_mass, dGeomID g);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dMassAdjust(ref Mass m, dReal newmass);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dMassTranslate(ref Mass m, dReal x, dReal y, dReal z);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dMassRotate(ref Mass m, ref Matrix3 R);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dMassAdd(ref Mass a, ref Mass b);
    }
}
