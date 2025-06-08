using Ode.Net.Collision;
using Ode.Net.Joints;
using System;
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
        #region World

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern dWorldID dWorldCreate();

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dWorldDestroy(IntPtr world);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dWorldSetGravity(dWorldID w, dReal x, dReal y, dReal z);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dWorldGetGravity(dWorldID w, out Vector3 gravity);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dWorldSetERP(dWorldID w, dReal erp);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern dReal dWorldGetERP(dWorldID w);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dWorldSetCFM(dWorldID w, dReal cfm);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern dReal dWorldGetCFM(dWorldID w);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dWorldSetStepIslandsProcessingMaxThreadCount(dWorldID w, uint count);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint dWorldGetStepIslandsProcessingMaxThreadCount(dWorldID w);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int dWorldUseSharedWorkingMemory(dWorldID w, dWorldID from_world);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dWorldCleanupWorkingMemory(dWorldID w);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int dWorldSetStepMemoryReservationPolicy(dWorldID w, IntPtr policyinfo);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int dWorldSetStepMemoryReservationPolicy(dWorldID w, ref dWorldStepReserveInfo policyinfo);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int dWorldSetStepMemoryManager(dWorldID w, IntPtr memfuncs);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int dWorldSetStepMemoryManager(dWorldID w, ref dWorldStepMemoryFunctionsInfo memfuncs);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dWorldSetStepThreadingImplementation(dWorldID w, IntPtr functions_info, IntPtr threading_impl);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dWorldSetStepThreadingImplementation(dWorldID w, IntPtr functions_info, dThreadingImplementationID threading_impl);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int dWorldStep(dWorldID w, dReal stepsize);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int dWorldQuickStep(dWorldID w, dReal stepsize);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dWorldImpulseToForce(
            dWorldID w, dReal stepsize,
            dReal ix, dReal iy, dReal iz, out Vector3 force);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dWorldSetQuickStepNumIterations(dWorldID w, int num);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int dWorldGetQuickStepNumIterations(dWorldID w);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dWorldSetQuickStepW(dWorldID w, dReal over_relaxation);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern dReal dWorldGetQuickStepW(dWorldID w);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dWorldSetContactMaxCorrectingVel(dWorldID w, dReal vel);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern dReal dWorldGetContactMaxCorrectingVel(dWorldID w);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dWorldSetContactSurfaceLayer(dWorldID w, dReal depth);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern dReal dWorldGetContactSurfaceLayer(dWorldID w);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern dReal dWorldGetAutoDisableLinearThreshold(dWorldID w);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dWorldSetAutoDisableLinearThreshold(dWorldID w, dReal linear_threshold);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern dReal dWorldGetAutoDisableAngularThreshold(dWorldID w);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dWorldSetAutoDisableAngularThreshold(dWorldID w, dReal angular_threshold);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int dWorldGetAutoDisableAverageSamplesCount(dWorldID w);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dWorldSetAutoDisableAverageSamplesCount(dWorldID w, uint average_samples_count);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int dWorldGetAutoDisableSteps(dWorldID w);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dWorldSetAutoDisableSteps(dWorldID w, int steps);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern dReal dWorldGetAutoDisableTime(dWorldID w);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dWorldSetAutoDisableTime(dWorldID w, dReal time);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int dWorldGetAutoDisableFlag(dWorldID w);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dWorldSetAutoDisableFlag(dWorldID w, int do_auto_disable);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern dReal dWorldGetLinearDampingThreshold(dWorldID w);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dWorldSetLinearDampingThreshold(dWorldID w, dReal threshold);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern dReal dWorldGetAngularDampingThreshold(dWorldID w);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dWorldSetAngularDampingThreshold(dWorldID w, dReal threshold);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern dReal dWorldGetLinearDamping(dWorldID w);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dWorldSetLinearDamping(dWorldID w, dReal scale);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern dReal dWorldGetAngularDamping(dWorldID w);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dWorldSetAngularDamping(dWorldID w, dReal scale);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dWorldSetDamping(
            dWorldID w,
            dReal linear_scale,
            dReal angular_scale);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern dReal dWorldGetMaxAngularSpeed(dWorldID w);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dWorldSetMaxAngularSpeed(dWorldID w, dReal max_speed);

        #endregion

        #region Rigid Bodies

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern dReal dBodyGetAutoDisableLinearThreshold(dBodyID b);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dBodySetAutoDisableLinearThreshold(dBodyID b, dReal linear_average_threshold);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern dReal dBodyGetAutoDisableAngularThreshold(dBodyID b);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dBodySetAutoDisableAngularThreshold(dBodyID b, dReal angular_average_threshold);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int dBodyGetAutoDisableAverageSamplesCount(dBodyID b);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dBodySetAutoDisableAverageSamplesCount(dBodyID b, uint average_samples_count);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int dBodyGetAutoDisableSteps(dBodyID b);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dBodySetAutoDisableSteps(dBodyID b, int steps);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern dReal dBodyGetAutoDisableTime(dBodyID b);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dBodySetAutoDisableTime(dBodyID b, dReal time);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int dBodyGetAutoDisableFlag(dBodyID b);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dBodySetAutoDisableFlag(dBodyID b, int do_auto_disable);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dBodySetAutoDisableDefaults(dBodyID b);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern dWorldID dBodyGetWorld(dBodyID b);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern dBodyID dBodyCreate(dWorldID w);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dBodyDestroy(IntPtr b);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dBodySetData(dBodyID b, IntPtr data);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr dBodyGetData(IntPtr b);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr dBodyGetData(dBodyID b);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dBodySetPosition(dBodyID b, dReal x, dReal y, dReal z);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dBodySetRotation(dBodyID b, ref Matrix3 R);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dBodySetQuaternion(dBodyID b, ref Quaternion q);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dBodySetLinearVel(dBodyID b, dReal x, dReal y, dReal z);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dBodySetAngularVel(dBodyID b, dReal x, dReal y, dReal z);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr dBodyGetPosition(dBodyID b);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dBodyCopyPosition(dBodyID body, out Vector3 pos);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr dBodyGetRotation(dBodyID b);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dBodyCopyRotation(dBodyID b, out Matrix3 R);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr dBodyGetQuaternion(dBodyID b);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dBodyCopyQuaternion(dBodyID body, out Quaternion quat);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr dBodyGetLinearVel(dBodyID b);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr dBodyGetAngularVel(dBodyID b);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dBodySetMass(dBodyID b, ref Mass mass);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dBodyGetMass(dBodyID b, out Mass mass);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dBodyAddForce(dBodyID b, dReal fx, dReal fy, dReal fz);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dBodyAddTorque(dBodyID b, dReal fx, dReal fy, dReal fz);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dBodyAddRelForce(dBodyID b, dReal fx, dReal fy, dReal fz);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dBodyAddRelTorque(dBodyID b, dReal fx, dReal fy, dReal fz);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dBodyAddForceAtPos(dBodyID b, dReal fx, dReal fy, dReal fz,
                                    dReal px, dReal py, dReal pz);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dBodyAddForceAtRelPos(dBodyID b, dReal fx, dReal fy, dReal fz,
                                    dReal px, dReal py, dReal pz);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dBodyAddRelForceAtPos(dBodyID b, dReal fx, dReal fy, dReal fz,
                                    dReal px, dReal py, dReal pz);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dBodyAddRelForceAtRelPos(dBodyID b, dReal fx, dReal fy, dReal fz,
                                    dReal px, dReal py, dReal pz);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr dBodyGetForce(dBodyID b);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr dBodyGetTorque(dBodyID b);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dBodySetForce(dBodyID b, dReal x, dReal y, dReal z);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dBodySetTorque(dBodyID b, dReal x, dReal y, dReal z);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dBodyGetRelPointPos
        (
          dBodyID b, dReal px, dReal py, dReal pz,
          out Vector3 result
        );

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dBodyGetRelPointVel
        (
          dBodyID b, dReal px, dReal py, dReal pz,
          out Vector3 result
        );

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dBodyGetPointVel
        (
          dBodyID b, dReal px, dReal py, dReal pz,
          out Vector3 result
        );

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dBodyGetPosRelPoint
        (
          dBodyID b, dReal px, dReal py, dReal pz,
          out Vector3 result
        );

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dBodyVectorToWorld
        (
          dBodyID b, dReal px, dReal py, dReal pz,
          out Vector3 result
        );

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dBodyVectorFromWorld
        (
          dBodyID b, dReal px, dReal py, dReal pz,
          out Vector3 result
        );

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dBodySetFiniteRotationMode(dBodyID b, RotationMode mode);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dBodySetFiniteRotationAxis(dBodyID b, dReal x, dReal y, dReal z);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern RotationMode dBodyGetFiniteRotationMode(dBodyID b);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dBodyGetFiniteRotationAxis(dBodyID b, out Vector3 result);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int dBodyGetNumJoints(dBodyID b);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr dBodyGetJoint(dBodyID b, int index);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dBodySetDynamic(dBodyID b);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dBodySetKinematic(dBodyID b);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int dBodyIsKinematic(dBodyID b);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dBodyEnable(dBodyID b);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dBodyDisable(dBodyID b);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int dBodyIsEnabled(dBodyID b);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dBodySetGravityMode(dBodyID b, int mode);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int dBodyGetGravityMode(dBodyID b);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dBodySetMovedCallback(dBodyID b, dMovedCallback callback);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr dBodyGetFirstGeom(dBodyID b);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr dBodyGetNextGeom(IntPtr g);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dBodySetDampingDefaults(dBodyID b);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern dReal dBodyGetLinearDamping(dBodyID b);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dBodySetLinearDamping(dBodyID b, dReal scale);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern dReal dBodyGetAngularDamping(dBodyID b);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dBodySetAngularDamping(dBodyID b, dReal scale);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dBodySetDamping(dBodyID b, dReal linear_scale, dReal angular_scale);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern dReal dBodyGetLinearDampingThreshold(dBodyID b);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dBodySetLinearDampingThreshold(dBodyID b, dReal threshold);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern dReal dBodyGetAngularDampingThreshold(dBodyID b);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dBodySetAngularDampingThreshold(dBodyID b, dReal threshold);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern dReal dBodyGetMaxAngularSpeed(dBodyID b);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dBodySetMaxAngularSpeed(dBodyID b, dReal max_speed);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int dBodyGetGyroscopicMode(dBodyID b);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dBodySetGyroscopicMode(dBodyID b, int enabled);

        #endregion

        #region Joints

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern dJointID dJointCreateBall(dWorldID w, dJointGroupID g);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern dJointID dJointCreateHinge(dWorldID w, dJointGroupID g);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern dJointID dJointCreateSlider(dWorldID w, dJointGroupID g);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern dJointID dJointCreateContact(dWorldID w, dJointGroupID g, ref ContactInfo c);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern dJointID dJointCreateHinge2(dWorldID w, dJointGroupID g);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern dJointID dJointCreateUniversal(dWorldID w, dJointGroupID g);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern dJointID dJointCreatePR(dWorldID w, dJointGroupID g);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern dJointID dJointCreatePU(dWorldID w, dJointGroupID g);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern dJointID dJointCreatePiston(dWorldID w, dJointGroupID g);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern dJointID dJointCreateFixed(dWorldID w, dJointGroupID g);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern dJointID dJointCreateNull(dWorldID w, dJointGroupID g);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern dJointID dJointCreateAMotor(dWorldID w, dJointGroupID g);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern dJointID dJointCreateLMotor(dWorldID w, dJointGroupID g);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern dJointID dJointCreatePlane2D(dWorldID w, dJointGroupID g);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern dJointID dJointCreateDBall(dWorldID w, dJointGroupID g);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern dJointID dJointCreateDHinge(dWorldID w, dJointGroupID g);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern dJointID dJointCreateTransmission(dWorldID w, dJointGroupID g);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointDestroy(IntPtr j);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern dJointGroupID dJointGroupCreate(int max_size);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointGroupDestroy(IntPtr g);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointGroupEmpty(dJointGroupID g);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int dJointGetNumBodies(dJointID j);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointAttach(dJointID j, dBodyID body1, dBodyID body2);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointEnable(dJointID j);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointDisable(dJointID j);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int dJointIsEnabled(dJointID j);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointSetData(dJointID j, IntPtr data);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr dJointGetData(IntPtr j);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr dJointGetData(dJointID j);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern JointType dJointGetType(dJointID j);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr dJointGetBody(dJointID j, int index);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointSetFeedback(dJointID j, dJointFeedbackHandle f);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr dJointGetFeedback(dJointID j);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointSetBallAnchor(dJointID j, dReal x, dReal y, dReal z);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointSetBallAnchor2(dJointID j, dReal x, dReal y, dReal z);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointSetBallParam(dJointID j, dJointParam parameter, dReal value);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointSetHingeAnchor(dJointID j, dReal x, dReal y, dReal z);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointSetHingeAnchorDelta(dJointID j, dReal x, dReal y, dReal z, dReal ax, dReal ay, dReal az);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointSetHingeAxis(dJointID j, dReal x, dReal y, dReal z);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointSetHingeAxisOffset(dJointID j, dReal x, dReal y, dReal z, dReal angle);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointSetHingeParam(dJointID j, dJointParam parameter, dReal value);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointAddHingeTorque(dJointID joint, dReal torque);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointSetSliderAxis(dJointID j, dReal x, dReal y, dReal z);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointSetSliderAxisDelta(dJointID j, dReal x, dReal y, dReal z, dReal ax, dReal ay, dReal az);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointSetSliderParam(dJointID j, dJointParam parameter, dReal value);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointAddSliderForce(dJointID joint, dReal force);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointSetHinge2Anchor(dJointID j, dReal x, dReal y, dReal z);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointSetHinge2Axis1(dJointID j, dReal x, dReal y, dReal z);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointSetHinge2Axis2(dJointID j, dReal x, dReal y, dReal z);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointSetHinge2Param(dJointID j, dJointParam parameter, dReal value);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointAddHinge2Torques(dJointID joint, dReal torque1, dReal torque2);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointSetUniversalAnchor(dJointID j, dReal x, dReal y, dReal z);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointSetUniversalAxis1(dJointID j, dReal x, dReal y, dReal z);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointSetUniversalAxis1Offset(dJointID j, dReal x, dReal y, dReal z,
                                                    dReal offset1, dReal offset2);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointSetUniversalAxis2(dJointID j, dReal x, dReal y, dReal z);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointSetUniversalAxis2Offset(dJointID j, dReal x, dReal y, dReal z,
                                                    dReal offset1, dReal offset2);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointSetUniversalParam(dJointID j, dJointParam parameter, dReal value);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointAddUniversalTorques(dJointID joint, dReal torque1, dReal torque2);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointSetPRAnchor(dJointID j, dReal x, dReal y, dReal z);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointSetPRAxis1(dJointID j, dReal x, dReal y, dReal z);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointSetPRAxis2(dJointID j, dReal x, dReal y, dReal z);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointSetPRParam(dJointID j, dJointParam parameter, dReal value);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointAddPRTorque(dJointID j, dReal torque);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointSetPUAnchor(dJointID j, dReal x, dReal y, dReal z);

        [Obsolete]
        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointSetPUAnchorDelta(dJointID j, dReal x, dReal y, dReal z,
                                                                dReal dx, dReal dy, dReal dz);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointSetPUAnchorOffset(dJointID j, dReal x, dReal y, dReal z,
                                             dReal dx, dReal dy, dReal dz);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointSetPUAxis1(dJointID j, dReal x, dReal y, dReal z);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointSetPUAxis2(dJointID j, dReal x, dReal y, dReal z);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointSetPUAxis3(dJointID j, dReal x, dReal y, dReal z);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointSetPUAxisP(dJointID id, dReal x, dReal y, dReal z);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointSetPUParam(dJointID j, dJointParam parameter, dReal value);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointAddPUTorque(dJointID j, dReal torque);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointSetPistonAnchor(dJointID j, dReal x, dReal y, dReal z);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointSetPistonAnchorOffset(dJointID j, dReal x, dReal y, dReal z,
                                                 dReal dx, dReal dy, dReal dz);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointSetPistonAxis(dJointID j, dReal x, dReal y, dReal z);

        [Obsolete]
        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointSetPistonAxisDelta(dJointID j, dReal x, dReal y, dReal z, dReal ax, dReal ay, dReal az);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointSetPistonParam(dJointID j, dJointParam parameter, dReal value);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointAddPistonForce(dJointID joint, dReal force);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointSetFixed(dJointID j);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointSetFixedParam(dJointID j, dJointParam parameter, dReal value);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointSetAMotorNumAxes(dJointID j, int num);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointSetAMotorAxis(dJointID j, int anum, RelativeOrientation rel,
                      dReal x, dReal y, dReal z);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointSetAMotorAngle(dJointID j, int anum, dReal angle);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointSetAMotorParam(dJointID j, dJointParam parameter, dReal value);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointSetAMotorMode(dJointID j, AngularMotorMode mode);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointAddAMotorTorques(dJointID j, dReal torque1, dReal torque2, dReal torque3);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointSetLMotorNumAxes(dJointID j, int num);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointSetLMotorAxis(dJointID j, int anum, RelativeOrientation rel, dReal x, dReal y, dReal z);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointSetLMotorParam(dJointID j, dJointParam parameter, dReal value);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointSetPlane2DXParam(dJointID j, dJointParam parameter, dReal value);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointSetPlane2DYParam(dJointID j, dJointParam parameter, dReal value);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointSetPlane2DAngleParam(dJointID j, dJointParam parameter, dReal value);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointGetBallAnchor(dJointID j, out Vector3 result);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointGetBallAnchor2(dJointID j, out Vector3 result);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern dReal dJointGetBallParam(dJointID j, dJointParam parameter);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointGetHingeAnchor(dJointID j, out Vector3 result);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointGetHingeAnchor2(dJointID j, out Vector3 result);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointGetHingeAxis(dJointID j, out Vector3 result);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern dReal dJointGetHingeParam(dJointID j, dJointParam parameter);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern dReal dJointGetHingeAngle(dJointID j);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern dReal dJointGetHingeAngleRate(dJointID j);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern dReal dJointGetSliderPosition(dJointID j);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern dReal dJointGetSliderPositionRate(dJointID j);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointGetSliderAxis(dJointID j, out Vector3 result);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern dReal dJointGetSliderParam(dJointID j, dJointParam parameter);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointGetHinge2Anchor(dJointID j, out Vector3 result);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointGetHinge2Anchor2(dJointID j, out Vector3 result);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointGetHinge2Axis1(dJointID j, out Vector3 result);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointGetHinge2Axis2(dJointID j, out Vector3 result);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern dReal dJointGetHinge2Param(dJointID j, dJointParam parameter);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern dReal dJointGetHinge2Angle1(dJointID j);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern dReal dJointGetHinge2Angle1Rate(dJointID j);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern dReal dJointGetHinge2Angle2Rate(dJointID j);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointGetUniversalAnchor(dJointID j, out Vector3 result);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointGetUniversalAnchor2(dJointID j, out Vector3 result);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointGetUniversalAxis1(dJointID j, out Vector3 result);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointGetUniversalAxis2(dJointID j, out Vector3 result);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern dReal dJointGetUniversalParam(dJointID j, dJointParam parameter);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointGetUniversalAngles(dJointID j, out dReal angle1, out dReal angle2);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern dReal dJointGetUniversalAngle1(dJointID j);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern dReal dJointGetUniversalAngle2(dJointID j);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern dReal dJointGetUniversalAngle1Rate(dJointID j);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern dReal dJointGetUniversalAngle2Rate(dJointID j);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointGetPRAnchor(dJointID j, out Vector3 result);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern dReal dJointGetPRPosition(dJointID j);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern dReal dJointGetPRPositionRate(dJointID j);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern dReal dJointGetPRAngle(dJointID j);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern dReal dJointGetPRAngleRate(dJointID j);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointGetPRAxis1(dJointID j, out Vector3 result);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointGetPRAxis2(dJointID j, out Vector3 result);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern dReal dJointGetPRParam(dJointID j, dJointParam parameter);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointGetPUAnchor(dJointID j, out Vector3 result);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern dReal dJointGetPUPosition(dJointID j);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern dReal dJointGetPUPositionRate(dJointID j);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointGetPUAxis1(dJointID j, out Vector3 result);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointGetPUAxis2(dJointID j, out Vector3 result);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointGetPUAxis3(dJointID j, out Vector3 result);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointGetPUAxisP(dJointID id, out Vector3 result);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointGetPUAngles(dJointID j, out dReal angle1, out dReal angle2);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern dReal dJointGetPUAngle1(dJointID j);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern dReal dJointGetPUAngle1Rate(dJointID j);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern dReal dJointGetPUAngle2(dJointID j);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern dReal dJointGetPUAngle2Rate(dJointID j);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern dReal dJointGetPUParam(dJointID j, dJointParam parameter);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern dReal dJointGetPistonPosition(dJointID j);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern dReal dJointGetPistonPositionRate(dJointID j);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern dReal dJointGetPistonAngle(dJointID j);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern dReal dJointGetPistonAngleRate(dJointID j);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointGetPistonAnchor(dJointID j, out Vector3 result);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointGetPistonAnchor2(dJointID j, out Vector3 result);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointGetPistonAxis(dJointID j, out Vector3 result);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern dReal dJointGetPistonParam(dJointID j, dJointParam parameter);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int dJointGetAMotorNumAxes(dJointID j);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointGetAMotorAxis(dJointID j, int anum, out Vector3 result);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern RelativeOrientation dJointGetAMotorAxisRel(dJointID j, int anum);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern dReal dJointGetAMotorAngle(dJointID j, int anum);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern dReal dJointGetAMotorAngleRate(dJointID j, int anum);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern dReal dJointGetAMotorParam(dJointID j, dJointParam parameter);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern AngularMotorMode dJointGetAMotorMode(dJointID j);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int dJointGetLMotorNumAxes(dJointID j);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointGetLMotorAxis(dJointID j, int anum, out Vector3 result);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern dReal dJointGetLMotorParam(dJointID j, dJointParam parameter);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern dReal dJointGetFixedParam(dJointID j, dJointParam parameter);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointGetTransmissionContactPoint1(dJointID j, out Vector3 result);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointGetTransmissionContactPoint2(dJointID j, out Vector3 result);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointSetTransmissionAxis1(dJointID j, dReal x, dReal y, dReal z);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointGetTransmissionAxis1(dJointID j, out Vector3 result);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointSetTransmissionAxis2(dJointID j, dReal x, dReal y, dReal z);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointGetTransmissionAxis2(dJointID j, out Vector3 result);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointSetTransmissionAnchor1(dJointID j, dReal x, dReal y, dReal z);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointGetTransmissionAnchor1(dJointID j, out Vector3 result);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointSetTransmissionAnchor2(dJointID j, dReal x, dReal y, dReal z);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointGetTransmissionAnchor2(dJointID j, out Vector3 result);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointSetTransmissionParam(dJointID j, dJointParam parameter, dReal value);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern dReal dJointGetTransmissionParam(dJointID j, dJointParam parameter);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointSetTransmissionMode(dJointID j, TransmissionMode mode);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern TransmissionMode dJointGetTransmissionMode(dJointID j);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointSetTransmissionRatio(dJointID j, dReal ratio);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern dReal dJointGetTransmissionRatio(dJointID j);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointSetTransmissionAxis(dJointID j, dReal x, dReal y, dReal z);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointGetTransmissionAxis(dJointID j, out Vector3 result);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern dReal dJointGetTransmissionAngle1(dJointID j);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern dReal dJointGetTransmissionAngle2(dJointID j);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern dReal dJointGetTransmissionRadius1(dJointID j);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern dReal dJointGetTransmissionRadius2(dJointID j);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointSetTransmissionRadius1(dJointID j, dReal radius);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointSetTransmissionRadius2(dJointID j, dReal radius);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern dReal dJointGetTransmissionBacklash(dJointID j);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointSetTransmissionBacklash(dJointID j, dReal backlash);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointSetDBallAnchor1(dJointID j, dReal x, dReal y, dReal z);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointSetDBallAnchor2(dJointID j, dReal x, dReal y, dReal z);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointGetDBallAnchor1(dJointID j, out Vector3 result);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointGetDBallAnchor2(dJointID j, out Vector3 result);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern dReal dJointGetDBallDistance(dJointID j);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointSetDBallParam(dJointID j, dJointParam parameter, dReal value);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern dReal dJointGetDBallParam(dJointID j, dJointParam parameter);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointSetDHingeAxis(dJointID j, dReal x, dReal y, dReal z);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointGetDHingeAxis(dJointID j, out Vector3 result);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointSetDHingeAnchor1(dJointID j, dReal x, dReal y, dReal z);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointSetDHingeAnchor2(dJointID j, dReal x, dReal y, dReal z);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointGetDHingeAnchor1(dJointID j, out Vector3 result);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointGetDHingeAnchor2(dJointID j, out Vector3 result);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern dReal dJointGetDHingeDistance(dJointID j);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void dJointSetDHingeParam(dJointID j, dJointParam parameter, dReal value);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern dReal dJointGetDHingeParam(dJointID j, dJointParam parameter);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr dConnectingJoint(dBodyID body1, dBodyID body2);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int dConnectingJointList(dBodyID body1, dBodyID body2, [Out]IntPtr[] joint_list);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int dAreConnected(dBodyID body1, dBodyID body2);

        [DllImport(libName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int dAreConnectedExcluding(dBodyID body1, dBodyID body2, JointType joint_type);

        #endregion
    }
}
