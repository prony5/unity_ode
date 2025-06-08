using Ode.Net.Collision;
using Ode.Net.Joints;
using Ode.Net.Native;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
#if ODE_DOUBLE_PRECISION
using dReal = System.Double;
#else
using dReal = System.Single;
#endif

namespace Ode.Net
{
    /// <summary>
    /// Represents a rigid body.
    /// </summary>
    public sealed class Body : IDisposable
    {
        readonly GCHandle handle;
        readonly dBodyID id;
        event EventHandler moved;
        dMovedCallback movedCallback;

        /// <summary>
        /// Initializes a new instance of the <see cref="Body"/> class on the
        /// specified world.
        /// </summary>
        /// <param name="world">The world on which to place the body.</param>
        public Body(World world)
        {
            handle = GCHandle.Alloc(this);
            id = NativeMethods.dBodyCreate(world.Id);
            NativeMethods.dBodySetData(id, GCHandle.ToIntPtr(handle));
            id.Owner = world;
        }

        internal dBodyID Id
        {
            get { return id; }
        }

        internal static Body FromIntPtr(IntPtr value)
        {
            if (value == IntPtr.Zero) return null;
            var handlePtr = NativeMethods.dBodyGetData(value);
            if (handlePtr == IntPtr.Zero) return null;
            var handle = GCHandle.FromIntPtr(handlePtr);
            return (Body)handle.Target;
        }

        /// <summary>
        /// Gets the world on which the body is placed.
        /// </summary>
        public World World
        {
            get { return id.Owner; }
        }

        /// <summary>
        /// Gets or sets the auto-disable linear velocity threshold for the body.
        /// </summary>
        public dReal AutoDisableLinearThreshold
        {
            get { return NativeMethods.dBodyGetAutoDisableLinearThreshold(id); }
            set { NativeMethods.dBodySetAutoDisableLinearThreshold(id, value); }
        }

        /// <summary>
        /// Gets or sets the auto-disable angular velocity threshold for the body.
        /// </summary>
        public dReal AutoDisableAngularThreshold
        {
            get { return NativeMethods.dBodyGetAutoDisableAngularThreshold(id); }
            set { NativeMethods.dBodySetAutoDisableAngularThreshold(id, value); }
        }

        /// <summary>
        /// Gets or sets the auto-disable average velocity sample count for the body.
        /// </summary>
        public int AutoDisableAverageSamplesCount
        {
            get { return NativeMethods.dBodyGetAutoDisableAverageSamplesCount(id); }
            set { NativeMethods.dBodySetAutoDisableAverageSamplesCount(id, (uint)value); }
        }

        /// <summary>
        /// Gets or sets the number of simulation steps the body has to be idle for
        /// in order to automatically disable itself.
        /// </summary>
        public int AutoDisableSteps
        {
            get { return NativeMethods.dBodyGetAutoDisableSteps(id); }
            set { NativeMethods.dBodySetAutoDisableSteps(id, value); }
        }

        /// <summary>
        /// Gets or sets the amount of simulation time the body has to be idle for
        /// in order to automatically disable itself.
        /// </summary>
        public dReal AutoDisableTime
        {
            get { return NativeMethods.dBodyGetAutoDisableTime(id); }
            set { NativeMethods.dBodySetAutoDisableTime(id, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the body should automatically
        /// disable itself when it has been idle for some specified period of time.
        /// </summary>
        public bool AutoDisable
        {
            get { return NativeMethods.dBodyGetAutoDisableFlag(id) != 0; }
            set { NativeMethods.dBodySetAutoDisableFlag(id, value ? 1 : 0); }
        }

        /// <summary>
        /// Gets or sets the object that contains data about the rigid body.
        /// </summary>
        public object Tag { get; set; }

        /// <summary>
        /// Gets or sets the position of the rigid body.
        /// </summary>
        public Vector3 Position
        {
            get
            {
                Vector3 position;
                NativeMethods.dBodyCopyPosition(id, out position);
                return position;
            }
            set
            {
                NativeMethods.dBodySetPosition(id, value.X, value.Y, value.Z);
            }
        }

        /// <summary>
        /// Gets or sets the 3x3 rotation matrix of the rigid body.
        /// </summary>
        public Matrix3 Rotation
        {
            get
            {
                Matrix3 rotation;
                NativeMethods.dBodyCopyRotation(id, out rotation);
                return rotation;
            }
            set
            {
                NativeMethods.dBodySetRotation(id, ref value);
            }
        }

        /// <summary>
        /// Gets or sets the orientation quaternion of the rigid body.
        /// </summary>
        public Quaternion Quaternion
        {
            get
            {
                Quaternion quaternion;
                NativeMethods.dBodyCopyQuaternion(id, out quaternion);
                return quaternion;
            }
            set
            {
                NativeMethods.dBodySetQuaternion(id, ref value);
            }
        }

        /// <summary>
        /// Gets or sets the linear velocity of the rigid body.
        /// </summary>
        public Vector3 LinearVelocity
        {
            get
            {
                var linearVelPtr = NativeMethods.dBodyGetLinearVel(id);
                return utils.PtrToVector3(linearVelPtr);
            }
            set
            {
                NativeMethods.dBodySetLinearVel(id, value.X, value.Y, value.Z);
            }
        }

        /// <summary>
        /// Gets or sets the angular velocity of the rigid body.
        /// </summary>
        public Vector3 AngularVelocity
        {
            get
            {
                var angularVelPtr = NativeMethods.dBodyGetAngularVel(id);
                return utils.PtrToVector3(angularVelPtr);
            }
            set
            {
                NativeMethods.dBodySetAngularVel(id, value.X, value.Y, value.Z);
            }
        }

        /// <summary>
        /// Gets or sets the mass of the rigid body.
        /// </summary>
        public Mass Mass
        {
            get
            {
                Mass mass;
                NativeMethods.dBodyGetMass(id, out mass);
                return mass;
            }
            set
            {
                NativeMethods.dBodySetMass(id, ref value);
            }
        }

        /// <summary>
        /// Gets or sets the current accumulated force vector.
        /// </summary>
        public Vector3 Force
        {
            get
            {
                var forcePtr = NativeMethods.dBodyGetForce(id);
                return utils.PtrToVector3(forcePtr);
            }
            set
            {
                NativeMethods.dBodySetForce(id, value.X, value.Y, value.Z);
            }
        }

        /// <summary>
        /// Gets or sets the current accumulated torque vector.
        /// </summary>
        public Vector3 Torque
        {
            get
            {
                var torquePtr = NativeMethods.dBodyGetTorque(id);
                return utils.PtrToVector3(torquePtr);
            }
            set
            {
                NativeMethods.dBodySetTorque(id, value.X, value.Y, value.Z);
            }
        }

        /// <summary>
        /// Gets or sets the mode in which the body's orientation is updated at each time step.
        /// </summary>
        public RotationMode FiniteRotationMode
        {
            get { return NativeMethods.dBodyGetFiniteRotationMode(id); }
            set { NativeMethods.dBodySetFiniteRotationMode(id, value); }
        }

        /// <summary>
        /// Gets or sets the finite rotation axis for the rigid body.
        /// </summary>
        public Vector3 FiniteRotationAxis
        {
            get
            {
                Vector3 axis;
                NativeMethods.dBodyGetFiniteRotationAxis(id, out axis);
                return axis;
            }
            set
            {
                NativeMethods.dBodySetFiniteRotationAxis(id, value.X, value.Y, value.Z);
            }
        }

        /// <summary>
        /// Gets the collection of joints attached to the rigid body.
        /// </summary>
        public IEnumerable<Joint> Joints
        {
            get
            {
                var numJoints = NativeMethods.dBodyGetNumJoints(id);
                for (int i = 0; i < numJoints; i++)
                {
                    var joint = Joint.FromIntPtr(NativeMethods.dBodyGetJoint(id, i));
                    if (joint == null) continue;
                    yield return joint;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the rigid body is in
        /// kinematic state.
        /// </summary>
        public bool Kinematic
        {
            get
            {
                return NativeMethods.dBodyIsKinematic(id) != 0;
            }
            set
            {
                if (value) NativeMethods.dBodySetKinematic(id);
                else NativeMethods.dBodySetDynamic(id);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the rigid body is enabled.
        /// </summary>
        public bool Enabled
        {
            get
            {
                return NativeMethods.dBodyIsEnabled(id) != 0;
            }
            set
            {
                if (value) NativeMethods.dBodyEnable(id);
                else NativeMethods.dBodyDisable(id);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the rigid body is influenced
        /// by the world's gravity.
        /// </summary>
        public bool GravityMode
        {
            get { return NativeMethods.dBodyGetGravityMode(id) != 0; }
            set { NativeMethods.dBodySetGravityMode(id, value ? 1 : 0); }
        }

        /// <summary>
        /// Gets the collection of geoms associated with the rigid body.
        /// </summary>
        public IEnumerable<Geom> Geoms
        {
            get
            {
                var geom = NativeMethods.dBodyGetFirstGeom(id);
                while (geom != IntPtr.Zero)
                {
                    yield return Geom.FromIntPtr(geom);
                    geom = NativeMethods.dBodyGetNextGeom(geom);
                }
            }
        }

        /// <summary>
        /// Gets or sets the rigid body's linear velocity damping scale.
        /// </summary>
        public dReal LinearDamping
        {
            get { return NativeMethods.dBodyGetLinearDamping(id); }
            set { NativeMethods.dBodySetLinearDamping(id, value); }
        }

        /// <summary>
        /// Gets or sets the rigid body's angular velocity damping scale.
        /// </summary>
        public dReal AngularDamping
        {
            get { return NativeMethods.dBodyGetAngularDamping(id); }
            set { NativeMethods.dBodySetAngularDamping(id, value); }
        }

        /// <summary>
        /// Gets or sets the rigid body's linear velocity damping threshold.
        /// </summary>
        public dReal LinearDampingThreshold
        {
            get { return NativeMethods.dBodyGetLinearDampingThreshold(id); }
            set { NativeMethods.dBodySetLinearDampingThreshold(id, value); }
        }

        /// <summary>
        /// Gets or sets the rigid body's angular velocity damping threshold.
        /// </summary>
        public dReal AngularDampingThreshold
        {
            get { return NativeMethods.dBodyGetAngularDampingThreshold(id); }
            set { NativeMethods.dBodySetAngularDampingThreshold(id, value); }
        }

        /// <summary>
        /// Gets or sets the default maximum angular speed for the rigid body.
        /// </summary>
        public dReal MaxAngularSpeed
        {
            get { return NativeMethods.dBodyGetMaxAngularSpeed(id); }
            set { NativeMethods.dBodySetMaxAngularSpeed(id, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether gyroscopic term computation is enabled.
        /// </summary>
        public bool GyroscopicMode
        {
            get { return NativeMethods.dBodyGetGyroscopicMode(id) != 0; }
            set { NativeMethods.dBodySetGyroscopicMode(id, value ? 1 : 0); }
        }

        /// <summary>
        /// Occurs whenever the rigid body has its position or rotation changed
        /// during a world timestep.
        /// </summary>
        public event EventHandler Moved
        {
            add
            {
                if (Interlocked.CompareExchange(
                        ref movedCallback,
                        OnMoved, null) == null)
                {
                    NativeMethods.dBodySetMovedCallback(id, movedCallback);
                }

                moved += value;
            }
            remove { moved -= value; }
        }

        private void OnMoved(IntPtr ptr)
        {
            var handler = moved;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Sets the auto-disable parameters to those set as default for the world.
        /// </summary>
        public void SetAutoDisableDefaults()
        {
            NativeMethods.dBodySetAutoDisableDefaults(id);
        }

        /// <summary>
        /// Sets the damping parameters to those set as default for the world.
        /// </summary>
        public void SetDampingDefaults()
        {
            NativeMethods.dBodySetDampingDefaults(id);
        }

        /// <summary>
        /// Sets both linear and angular damping scales.
        /// </summary>
        /// <param name="linearScale">
        /// The linear damping scale that is to be applied to the rigid body.
        /// </param>
        /// <param name="angularScale">
        /// The angular damping scale that is to be applied to the rigid body.
        /// </param>
        public void SetDamping(dReal linearScale, dReal angularScale)
        {
            NativeMethods.dBodySetDamping(id, linearScale, angularScale);
        }

        /// <summary>
        /// Gets the position of the rigid body.
        /// </summary>
        /// <param name="position">The position of the rigid body.</param>
        public void GetPosition(out Vector3 position)
        {
            NativeMethods.dBodyCopyPosition(id, out position);
        }

        /// <summary>
        /// Sets the position of the rigid body.
        /// </summary>
        /// <param name="position">The position of the rigid body.</param>
        public void SetPosition(ref Vector3 position)
        {
            NativeMethods.dBodySetPosition(id, position.X, position.Y, position.Z);
        }

        /// <summary>
        /// Gets the 3x3 rotation matrix of the rigid body.
        /// </summary>
        /// <param name="rotation">The 3x3 rotation matrix of the rigid body.</param>
        public void GetRotation(out Matrix3 rotation)
        {
            NativeMethods.dBodyCopyRotation(id, out rotation);
        }

        /// <summary>
        /// Sets the 3x3 rotation matrix of the rigid body.
        /// </summary>
        /// <param name="rotation">The 3x3 rotation matrix of the rigid body.</param>
        public void SetRotation(ref Matrix3 rotation)
        {
            NativeMethods.dBodySetRotation(id, ref rotation);
        }

        /// <summary>
        /// Gets the orientation quaternion of the rigid body.
        /// </summary>
        /// <param name="quaternion">The orientation quaternion of the rigid body.</param>
        public void GetQuaternion(out Quaternion quaternion)
        {
            NativeMethods.dBodyCopyQuaternion(id, out quaternion);
        }

        /// <summary>
        /// Sets the orientation quaternion of the rigid body.
        /// </summary>
        /// <param name="quaternion">The orientation quaternion of the rigid body.</param>
        public void SetQuaternion(ref Quaternion quaternion)
        {
            NativeMethods.dBodySetQuaternion(id, ref quaternion);
        }

        /// <summary>
        /// Gets the linear velocity of the rigid body.
        /// </summary>
        /// <param name="linearVelocity">The linear velocity of the rigid body.</param>
        public void GetLinearVelocity(out Vector3 linearVelocity)
        {
            var linearVelPtr = NativeMethods.dBodyGetLinearVel(id);
            utils.PtrToVector3(linearVelPtr, out linearVelocity);
        }

        /// <summary>
        /// Sets the linear velocity of the rigid body.
        /// </summary>
        /// <param name="linearVelocity">The linear velocity of the rigid body.</param>
        public void SetLinearVelocity(ref Vector3 linearVelocity)
        {
            NativeMethods.dBodySetLinearVel(id, linearVelocity.X, linearVelocity.Y, linearVelocity.Z);
        }

        /// <summary>
        /// Gets the angular velocity of the rigid body.
        /// </summary>
        /// <param name="angularVelocity">The angular velocity of the rigid body.</param>
        public void GetAngularVelocity(out Vector3 angularVelocity)
        {
            var angularVelPtr = NativeMethods.dBodyGetAngularVel(id);
            utils.PtrToVector3(angularVelPtr, out angularVelocity);
        }

        /// <summary>
        /// Sets the angular velocity of the rigid body.
        /// </summary>
        /// <param name="angularVelocity">The angular velocity of the rigid body.</param>
        public void SetAngularVelocity(ref Vector3 angularVelocity)
        {
            NativeMethods.dBodySetAngularVel(id, angularVelocity.X, angularVelocity.Y, angularVelocity.Z);
        }

        /// <summary>
        /// Adds force at the center of mass of the body.
        /// </summary>
        /// <param name="force">The force vector in the world coordinate system.</param>
        public void AddForce(Vector3 force)
        {
            NativeMethods.dBodyAddForce(id, force.X, force.Y, force.Z);
        }

        /// <summary>
        /// Adds force at the center of mass of the body.
        /// </summary>
        /// <param name="fx">The x-component of the force vector in the world coordinate system.</param>
        /// <param name="fy">The y-component of the force vector in the world coordinate system.</param>
        /// <param name="fz">The z-component of the force vector in the world coordinate system.</param>
        public void AddForce(dReal fx, dReal fy, dReal fz)
        {
            NativeMethods.dBodyAddForce(id, fx, fy, fz);
        }

        /// <summary>
        /// Adds torque at the center of mass of the body.
        /// </summary>
        /// <param name="torque">The torque vector in the world coordinate system.</param>
        public void AddTorque(Vector3 torque)
        {
            NativeMethods.dBodyAddTorque(id, torque.X, torque.Y, torque.Z);
        }

        /// <summary>
        /// Adds torque at the center of mass of the body.
        /// </summary>
        /// <param name="fx">The x-component of the torque vector in the world coordinate system.</param>
        /// <param name="fy">The y-component of the torque vector in the world coordinate system.</param>
        /// <param name="fz">The z-component of the torque vector in the world coordinate system.</param>
        public void AddTorque(dReal fx, dReal fy, dReal fz)
        {
            NativeMethods.dBodyAddTorque(id, fx, fy, fz);
        }

        /// <summary>
        /// Adds force at the center of mass of the body.
        /// </summary>
        /// <param name="force">The force vector in the body's coordinate system.</param>
        public void AddRelativeForce(Vector3 force)
        {
            NativeMethods.dBodyAddRelForce(id, force.X, force.Y, force.Z);
        }

        /// <summary>
        /// Adds force at the center of mass of the body.
        /// </summary>
        /// <param name="fx">The x-component of the force vector in the body's coordinate system.</param>
        /// <param name="fy">The y-component of the force vector in the body's coordinate system.</param>
        /// <param name="fz">The z-component of the force vector in the body's coordinate system.</param>
        public void AddRelativeForce(dReal fx, dReal fy, dReal fz)
        {
            NativeMethods.dBodyAddRelForce(id, fx, fy, fz);
        }

        /// <summary>
        /// Adds torque at the center of mass of the body.
        /// </summary>
        /// <param name="torque">The torque vector in the body's coordinate system.</param>
        public void AddRelativeTorque(Vector3 torque)
        {
            NativeMethods.dBodyAddRelTorque(id, torque.X, torque.Y, torque.Z);
        }

        /// <summary>
        /// Adds torque at the center of mass of the body.
        /// </summary>
        /// <param name="fx">The x-component of the torque vector in the body's coordinate system.</param>
        /// <param name="fy">The y-component of the torque vector in the body's coordinate system.</param>
        /// <param name="fz">The z-component of the torque vector in the body's coordinate system.</param>
        public void AddRelativeTorque(dReal fx, dReal fy, dReal fz)
        {
            NativeMethods.dBodyAddRelTorque(id, fx, fy, fz);
        }

        /// <summary>
        /// Adds force to the body applied at the specified position.
        /// </summary>
        /// <param name="force">The force vector in the world coordinate system.</param>
        /// <param name="position">The point of application in global coordinates.</param>
        public void AddForceAtPosition(Vector3 force, Vector3 position)
        {
            NativeMethods.dBodyAddForceAtPos(
                id, force.X, force.Y, force.Z,
                position.X, position.Y, position.Z);
        }

        /// <summary>
        /// Adds force to the body applied at the specified position.
        /// </summary>
        /// <param name="fx">The x-component of the force vector in the world coordinate system.</param>
        /// <param name="fy">The y-component of the force vector in the world coordinate system.</param>
        /// <param name="fz">The z-component of the force vector in the world coordinate system.</param>
        /// <param name="px">The x-component of the application point in global coordinates.</param>
        /// <param name="py">The y-component of the application point in global coordinates.</param>
        /// <param name="pz">The z-component of the application point in global coordinates.</param>
        public void AddForceAtPosition(
            dReal fx, dReal fy, dReal fz,
            dReal px, dReal py, dReal pz)
        {
            NativeMethods.dBodyAddForceAtPos(id, fx, fy, fz, px, py, pz);
        }

        /// <summary>
        /// Adds force to the body applied at the specified position.
        /// </summary>
        /// <param name="force">The force vector in the world coordinate system.</param>
        /// <param name="position">The point of application in body relative coordinates.</param>
        public void AddForceAtRelativePosition(Vector3 force, Vector3 position)
        {
            NativeMethods.dBodyAddForceAtRelPos(
                id, force.X, force.Y, force.Z,
                position.X, position.Y, position.Z);
        }

        /// <summary>
        /// Adds force to the body applied at the specified position.
        /// </summary>
        /// <param name="fx">The x-component of the force vector in the world coordinate system.</param>
        /// <param name="fy">The y-component of the force vector in the world coordinate system.</param>
        /// <param name="fz">The z-component of the force vector in the world coordinate system.</param>
        /// <param name="px">The x-component of the application point in body relative coordinates.</param>
        /// <param name="py">The y-component of the application point in body relative coordinates.</param>
        /// <param name="pz">The z-component of the application point in body relative coordinates.</param>
        public void AddForceAtRelativePosition(
            dReal fx, dReal fy, dReal fz,
            dReal px, dReal py, dReal pz)
        {
            NativeMethods.dBodyAddForceAtRelPos(id, fx, fy, fz, px, py, pz);
        }

        /// <summary>
        /// Adds force to the body applied at the specified position.
        /// </summary>
        /// <param name="force">The force vector in the body's coordinate system.</param>
        /// <param name="position">The point of application in global coordinates.</param>
        public void AddRelativeForceAtPosition(Vector3 force, Vector3 position)
        {
            NativeMethods.dBodyAddRelForceAtPos(
                id, force.X, force.Y, force.Z,
                position.X, position.Y, position.Z);
        }

        /// <summary>
        /// Adds force to the body applied at the specified position.
        /// </summary>
        /// <param name="fx">The x-component of the force vector in the body's coordinate system.</param>
        /// <param name="fy">The y-component of the force vector in the body's coordinate system.</param>
        /// <param name="fz">The z-component of the force vector in the body's coordinate system.</param>
        /// <param name="px">The x-component of the application point in global coordinates.</param>
        /// <param name="py">The y-component of the application point in global coordinates.</param>
        /// <param name="pz">The z-component of the application point in global coordinates.</param>
        public void AddRelativeForceAtPosition(
            dReal fx, dReal fy, dReal fz,
            dReal px, dReal py, dReal pz)
        {
            NativeMethods.dBodyAddRelForceAtPos(id, fx, fy, fz, px, py, pz);
        }

        /// <summary>
        /// Adds force to the body applied at the specified position.
        /// </summary>
        /// <param name="force">The force vector in the body's coordinate system.</param>
        /// <param name="position">The point of application in body relative coordinates.</param>
        public void AddRelativeForceAtRelativePosition(Vector3 force, Vector3 position)
        {
            NativeMethods.dBodyAddRelForceAtRelPos(
                id, force.X, force.Y, force.Z,
                position.X, position.Y, position.Z);
        }

        /// <summary>
        /// Adds force to the body applied at the specified position.
        /// </summary>
        /// <param name="fx">The x-component of the force vector in the body's coordinate system.</param>
        /// <param name="fy">The y-component of the force vector in the body's coordinate system.</param>
        /// <param name="fz">The z-component of the force vector in the body's coordinate system.</param>
        /// <param name="px">The x-component of the application point in body relative coordinates.</param>
        /// <param name="py">The y-component of the application point in body relative coordinates.</param>
        /// <param name="pz">The z-component of the application point in body relative coordinates.</param>
        public void AddRelativeForceAtRelativePosition(
            dReal fx, dReal fy, dReal fz,
            dReal px, dReal py, dReal pz)
        {
            NativeMethods.dBodyAddRelForceAtRelPos(id, fx, fy, fz, px, py, pz);
        }

        /// <summary>
        /// Gets the current accumulated force vector.
        /// </summary>
        /// <param name="force">The current accumulated force vector.</param>
        public void GetForce(out Vector3 force)
        {
            var forcePtr = NativeMethods.dBodyGetForce(id);
            utils.PtrToVector3(forcePtr, out force);
        }

        /// <summary>
        /// Sets the current accumulated force vector.
        /// </summary>
        /// <param name="force">The current accumulated force vector.</param>
        public void SetForce(ref Vector3 force)
        {
            NativeMethods.dBodySetForce(id, force.X, force.Y, force.Z);
        }

        /// <summary>
        /// Gets the current accumulated torque vector.
        /// </summary>
        /// <param name="torque">The current accumulated torque vector.</param>
        public void GetTorque(out Vector3 torque)
        {
            var torquePtr = NativeMethods.dBodyGetTorque(id);
            utils.PtrToVector3(torquePtr, out torque);
        }

        /// <summary>
        /// Sets the current accumulated torque vector.
        /// </summary>
        /// <param name="torque">The current accumulated torque vector.</param>
        public void SetTorque(ref Vector3 torque)
        {
            NativeMethods.dBodySetTorque(id, torque.X, torque.Y, torque.Z);
        }

        /// <summary>
        /// Computes the global position of a point specified in body relative coordinates.
        /// </summary>
        /// <param name="point">A point specified in body relative coordinates.</param>
        /// <param name="result">The position of the point in global coordinates.</param>
        public void GetRelativePointPosition(ref Vector3 point, out Vector3 result)
        {
            NativeMethods.dBodyGetRelPointPos(id, point.X, point.Y, point.Z, out result);
        }

        /// <summary>
        /// Computes the global position of a point specified in body relative coordinates.
        /// </summary>
        /// <param name="point">A point specified in body relative coordinates.</param>
        /// <returns>The position of the point in global coordinates.</returns>
        public Vector3 GetRelativePointPosition(Vector3 point)
        {
            Vector3 result;
            GetRelativePointPosition(ref point, out result);
            return result;
        }

        /// <summary>
        /// Computes the velocity of a point specified in body relative coordinates.
        /// </summary>
        /// <param name="point">A point specified in body relative coordinates.</param>
        /// <param name="result">The velocity of the point.</param>
        public void GetRelativePointVelocity(ref Vector3 point, out Vector3 result)
        {
            NativeMethods.dBodyGetRelPointVel(id, point.X, point.Y, point.Z, out result);
        }

        /// <summary>
        /// Computes the velocity of a point specified in body relative coordinates.
        /// </summary>
        /// <param name="point">A point specified in body relative coordinates.</param>
        /// <returns>The velocity of the point.</returns>
        public Vector3 GetRelativePointVelocity(Vector3 point)
        {
            Vector3 result;
            GetRelativePointVelocity(ref point, out result);
            return result;
        }

        /// <summary>
        /// Computes the velocity of a point specified in global coordinates.
        /// </summary>
        /// <param name="point">A point specified in global coordinates.</param>
        /// <param name="result">The velocity of the point.</param>
        public void GetPointVelocity(ref Vector3 point, out Vector3 result)
        {
            NativeMethods.dBodyGetPointVel(id, point.X, point.Y, point.Z, out result);
        }

        /// <summary>
        /// Computes the velocity of a point specified in global coordinates.
        /// </summary>
        /// <param name="point">A point specified in global coordinates.</param>
        /// <returns>The velocity of the point.</returns>
        public Vector3 GetPointVelocity(Vector3 point)
        {
            Vector3 result;
            GetPointVelocity(ref point, out result);
            return result;
        }

        /// <summary>
        /// Computes the body relative position of a point specified in global coordinates.
        /// </summary>
        /// <param name="position">A point specified in global coordinates.</param>
        /// <param name="result">The position of the point in body relative coordinates.</param>
        public void GetPositionRelativePoint(ref Vector3 position, out Vector3 result)
        {
            NativeMethods.dBodyGetPosRelPoint(id, position.X, position.Y, position.Z, out result);
        }

        /// <summary>
        /// Computes the body relative position of a point specified in global coordinates.
        /// </summary>
        /// <param name="position">A point specified in global coordinates.</param>
        /// <returns>The position of the point in body relative coordinates.</returns>
        public Vector3 GetPositionRelativePoint(Vector3 position)
        {
            Vector3 result;
            GetPositionRelativePoint(ref position, out result);
            return result;
        }

        /// <summary>
        /// Given a vector expressed in the body's coordinate system, rotate it to the
        /// world coordinate system.
        /// </summary>
        /// <param name="vector">The vector to rotate in the body's coordinate system.</param>
        /// <param name="result">The rotated vector in the world coordinate system.</param>
        public void VectorToWorld(ref Vector3 vector, out Vector3 result)
        {
            NativeMethods.dBodyVectorToWorld(id, vector.X, vector.Y, vector.Z, out result);
        }

        /// <summary>
        /// Given a vector expressed in the body's coordinate system, rotate it to the
        /// world coordinate system.
        /// </summary>
        /// <param name="vector">The vector to rotate in the body's coordinate system.</param>
        /// <returns>The rotated vector in the world coordinate system.</returns>
        public Vector3 VectorToWorld(Vector3 vector)
        {
            Vector3 result;
            VectorToWorld(ref vector, out result);
            return result;
        }

        /// <summary>
        /// Given a vector expressed in the world coordinate system, rotate it to the
        /// body's coordinate system.
        /// </summary>
        /// <param name="vector">The vector to rotate in the world coordinate system.</param>
        /// <param name="result">The rotated vector in the body's coordinate system.</param>
        public void VectorFromWorld(ref Vector3 vector, out Vector3 result)
        {
            NativeMethods.dBodyVectorFromWorld(id, vector.X, vector.Y, vector.Z, out result);
        }

        /// <summary>
        /// Given a vector expressed in the world coordinate system, rotate it to the
        /// body's coordinate system.
        /// </summary>
        /// <param name="vector">The vector to rotate in the world coordinate system.</param>
        /// <returns>The rotated vector in the body's coordinate system.</returns>
        public Vector3 VectorFromWorld(Vector3 vector)
        {
            Vector3 result;
            VectorFromWorld(ref vector, out result);
            return result;
        }

        /// <summary>
        /// Sets the finite rotation axis for the rigid body.
        /// </summary>
        /// <param name="axis">The finite rotation axis for the rigid body.</param>
        public void SetFiniteRotationAxis(ref Vector3 axis)
        {
            NativeMethods.dBodySetFiniteRotationAxis(id, axis.X, axis.Y, axis.Z);
        }

        /// <summary>
        /// Gets the finite rotation axis for the rigid body.
        /// </summary>
        /// <param name="axis">The finite rotation axis for the rigid body.</param>
        public void GetFiniteRotationAxis(out Vector3 axis)
        {
            NativeMethods.dBodyGetFiniteRotationAxis(id, out axis);
        }

        /// <summary>
        /// Gets the first joint connecting this body to another specified rigid body.
        /// </summary>
        /// <param name="body">The body to get a connecting joint for.</param>
        /// <returns>
        /// The first connecting joint between the two bodies, if one exists;
        /// otherwise, <b>null</b>.
        /// </returns>
        public Joint GetConnectingJoint(Body body)
        {
            var joint = NativeMethods.dConnectingJoint(id, body.id);
            return Joint.FromIntPtr(joint);
        }

        /// <summary>
        /// Gets the collection of joints connecting this body to another
        /// specified rigid body.
        /// </summary>
        /// <param name="body">The body to get connecting joints for.</param>
        /// <returns>
        /// The collection of joints connecting the two bodies.
        /// </returns>
        public IEnumerable<Joint> GetConnectingJoints(Body body)
        {
            var numJoints = NativeMethods.dBodyGetNumJoints(id);
            var connectingJoints = new IntPtr[numJoints];

            var numConnectingJoints = NativeMethods.dConnectingJointList(id, body.id, connectingJoints);
            for (int i = 0; i < numConnectingJoints; i++)
            {
                var connectingJoint = Joint.FromIntPtr(connectingJoints[i]);
                if (connectingJoint == null) continue;
                yield return connectingJoint;
            }
        }

        /// <summary>
        /// Tests whether two bodies are connected together by a joint.
        /// </summary>
        /// <param name="body1">The first body to test.</param>
        /// <param name="body2">The second body to test.</param>
        /// <returns>
        /// <b>true</b> if the two bodies are connected together by a joint;
        /// otherwise, <b>false</b>.
        /// </returns>
        public static bool AreConnected(Body body1, Body body2)
        {
            return NativeMethods.dAreConnected(body1.id, body2.id) != 0;
        }

        /// <summary>
        /// Tests whether two bodies are connected together by a joint that does
        /// not have the specified type.
        /// </summary>
        /// <param name="body1">The first body to test.</param>
        /// <param name="body2">The second body to test.</param>
        /// <param name="jointType">The type of joints to exclude from the test.</param>
        /// <returns>
        /// <b>true</b> if the two bodies are connected together by a joint that
        /// does not have the specified type; otherwise, <b>false</b>.
        /// </returns>
        public static bool AreConnectedExcluding(Body body1, Body body2, JointType jointType)
        {
            return NativeMethods.dAreConnectedExcluding(body1.id, body2.id, jointType) != 0;
        }

        /// <summary>
        /// Destroys the rigid body.
        /// </summary>
        public void Dispose()
        {
            if (!id.IsClosed)
            {
                handle.Free();
                id.Close();
            }
        }
    }
}
