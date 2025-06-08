using Ode.Net.Native;
using System;
#if ODE_DOUBLE_PRECISION
using dReal = System.Double;
#else
using dReal = System.Single;
#endif

namespace Ode.Net
{
    /// <summary>
    /// Represents a container for rigid bodies and joints.
    /// </summary>
    public sealed class World : IDisposable
    {
        /// <summary>
        /// Represents an unlimited number of island step threads when setting
        /// thread count properties. This field is constant.
        /// </summary>
        public const int StepThreadCountUnlimited = 0;
        readonly dWorldID id;

        /// <summary>
        /// Initializes a new instance of the <see cref="World"/> class.
        /// </summary>
        public World()
        {
            id = NativeMethods.dWorldCreate();
        }

        internal dWorldID Id
        {
            get { return id; }
        }

        /// <summary>
        /// Gets or sets the world's global gravity vector.
        /// </summary>
        public Vector3 Gravity
        {
            get
            {
                Vector3 gravity;
                NativeMethods.dWorldGetGravity(id, out gravity);
                return gravity;
            }
            set
            {
                NativeMethods.dWorldSetGravity(id, value.X, value.Y, value.Z);
            }
        }

        /// <summary>
        /// Gets or sets the global ERP value, that controls how much error
        /// correction is performed in each time step.
        /// </summary>
        public dReal Erp
        {
            get { return NativeMethods.dWorldGetERP(id); }
            set { NativeMethods.dWorldSetERP(id, value); }
        }

        /// <summary>
        /// Gets or sets the global CFM (constraint force mixing) value.
        /// </summary>
        public dReal Cfm
        {
            get { return NativeMethods.dWorldGetCFM(id); }
            set { NativeMethods.dWorldSetCFM(id, value); }
        }

        /// <summary>
        /// Gets or sets the maximum number of threads to be used for island stepping.
        /// </summary>
        public int StepIslandsProcessingMaxThreadCount
        {
            get { return (int)NativeMethods.dWorldGetStepIslandsProcessingMaxThreadCount(id); }
            set { NativeMethods.dWorldSetStepIslandsProcessingMaxThreadCount(id, (uint)value); }
        }

        /// <summary>
        /// Gets or sets the number of iterations that the QuickStep method
        /// performs per step.
        /// </summary>
        public int QuickStepNumIterations
        {
            get { return NativeMethods.dWorldGetQuickStepNumIterations(id); }
            set { NativeMethods.dWorldSetQuickStepNumIterations(id, value); }
        }

        /// <summary>
        /// Gets or sets the SOR over-relaxation parameter.
        /// </summary>
        public dReal QuickStepW
        {
            get { return NativeMethods.dWorldGetQuickStepW(id); }
            set { NativeMethods.dWorldSetQuickStepW(id, value); }
        }

        /// <summary>
        /// Gets or sets the maximum correcting velocity that contacts are allowed
        /// to generate.
        /// </summary>
        public dReal ContactMaxCorrectingVelocity
        {
            get { return NativeMethods.dWorldGetContactMaxCorrectingVel(id); }
            set { NativeMethods.dWorldSetContactMaxCorrectingVel(id, value); }
        }

        /// <summary>
        /// Gets or sets the depth of the surface layer around all geometry objects.
        /// </summary>
        public dReal ContactSurfaceLayer
        {
            get { return NativeMethods.dWorldGetContactSurfaceLayer(id); }
            set { NativeMethods.dWorldSetContactSurfaceLayer(id, value); }
        }

        /// <summary>
        /// Gets or sets the auto-disable linear velocity threshold for newly created bodies.
        /// </summary>
        public dReal AutoDisableLinearThreshold
        {
            get { return NativeMethods.dWorldGetAutoDisableLinearThreshold(id); }
            set { NativeMethods.dWorldSetAutoDisableLinearThreshold(id, value); }
        }

        /// <summary>
        /// Gets or sets the auto-disable angular velocity threshold for newly created bodies.
        /// </summary>
        public dReal AutoDisableAngularThreshold
        {
            get { return NativeMethods.dWorldGetAutoDisableAngularThreshold(id); }
            set { NativeMethods.dWorldSetAutoDisableAngularThreshold(id, value); }
        }

        /// <summary>
        /// Gets or sets the auto-disable average velocity sample count for newly created bodies.
        /// </summary>
        public int AutoDisableAverageSamplesCount
        {
            get { return NativeMethods.dWorldGetAutoDisableAverageSamplesCount(id); }
            set { NativeMethods.dWorldSetAutoDisableAverageSamplesCount(id, (uint)value); }
        }

        /// <summary>
        /// Gets or sets the number of simulation steps newly created bodies have to be idle for
        /// in order to automatically disable themselves.
        /// </summary>
        public int AutoDisableSteps
        {
            get { return NativeMethods.dWorldGetAutoDisableSteps(id); }
            set { NativeMethods.dWorldSetAutoDisableSteps(id, value); }
        }

        /// <summary>
        /// Gets or sets the amount of simulation time newly created bodies have to be idle for
        /// in order to automatically disable themselves.
        /// </summary>
        public dReal AutoDisableTime
        {
            get { return NativeMethods.dWorldGetAutoDisableTime(id); }
            set { NativeMethods.dWorldSetAutoDisableTime(id, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether newly created bodies should automatically
        /// disable themselves when they have been idle for some specified period of time.
        /// </summary>
        public bool AutoDisable
        {
            get { return NativeMethods.dWorldGetAutoDisableFlag(id) != 0; }
            set { NativeMethods.dWorldSetAutoDisableFlag(id, value ? 1 : 0); }
        }

        /// <summary>
        /// Gets or sets the world's linear velocity damping threshold.
        /// </summary>
        public dReal LinearDampingThreshold
        {
            get { return NativeMethods.dWorldGetLinearDampingThreshold(id); }
            set { NativeMethods.dWorldSetLinearDampingThreshold(id, value); }
        }

        /// <summary>
        /// Gets or sets the world's angular velocity damping threshold.
        /// </summary>
        public dReal AngularDampingThreshold
        {
            get { return NativeMethods.dWorldGetAngularDampingThreshold(id); }
            set { NativeMethods.dWorldSetAngularDampingThreshold(id, value); }
        }

        /// <summary>
        /// Gets or sets the world's linear velocity damping scale.
        /// </summary>
        public dReal LinearDamping
        {
            get { return NativeMethods.dWorldGetLinearDamping(id); }
            set { NativeMethods.dWorldSetLinearDamping(id, value); }
        }

        /// <summary>
        /// Gets or sets the world's angular velocity damping scale.
        /// </summary>
        public dReal AngularDamping
        {
            get { return NativeMethods.dWorldGetAngularDamping(id); }
            set { NativeMethods.dWorldSetAngularDamping(id, value); }
        }

        /// <summary>
        /// Gets or sets the default maximum angular speed for new bodies.
        /// </summary>
        public dReal MaxAngularSpeed
        {
            get { return NativeMethods.dWorldGetMaxAngularSpeed(id); }
            set { NativeMethods.dWorldSetMaxAngularSpeed(id, value); }
        }

        /// <summary>
        /// Sets the world to use shared working memory along with another world.
        /// </summary>
        /// <param name="fromWorld">The world to use shared memory with.</param>
        /// <exception cref="InvalidOperationException">
        /// Failed to setup shared working memory between the two worlds.
        /// </exception>
        public void UseSharedWorkingMemory(World fromWorld)
        {
            var fromId = fromWorld == null ? dWorldID.Null : fromWorld.id;
            var result = NativeMethods.dWorldUseSharedWorkingMemory(id, fromId);
            if (result == 0)
            {
                throw new InvalidOperationException("Could not set shared working memory between the two worlds.");
            }
        }

        /// <summary>
        /// Releases internal working memory allocated for the world.
        /// </summary>
        public void CleanupWorkingMemory()
        {
            NativeMethods.dWorldCleanupWorkingMemory(id);
        }

        /// <summary>
        /// Sets the memory reservation policy to be used with world stepping operations.
        /// </summary>
        /// <param name="policyInfo">
        /// The memory reservation policy descriptor. If the descriptor is <b>null</b>,
        /// the current reservation policy will be reset to default parameters.
        /// </param>
        /// <exception cref="InvalidOperationException">
        /// Failed to set memory reservation policy for world stepping operations.
        /// </exception>
        public void SetStepMemoryReservationPolicy(WorldStepMemoryReservationPolicy policyInfo)
        {
            var result = policyInfo == null
                ? NativeMethods.dWorldSetStepMemoryReservationPolicy(id, IntPtr.Zero)
                : NativeMethods.dWorldSetStepMemoryReservationPolicy(id, ref policyInfo.info);
            if (result == 0)
            {
                throw new InvalidOperationException("Could not set memory reservation policy for stepping operations.");
            }
        }

        /// <summary>
        /// Sets the memory manager to be used with world stepping operations.
        /// </summary>
        /// <param name="memoryManager">
        /// The memory manager descriptor. If the descriptor is <b>null</b>,
        /// the default memory manager is used.
        /// </param>
        /// <exception cref="InvalidOperationException">
        /// Failed to set memory manager for world stepping operations.
        /// </exception>
        public void SetStepMemoryManager(WorldStepMemoryManager memoryManager)
        {
            var result = memoryManager == null
                ? NativeMethods.dWorldSetStepMemoryManager(id, IntPtr.Zero)
                : NativeMethods.dWorldSetStepMemoryManager(id, ref memoryManager.info);
            if (result == 0)
            {
                throw new InvalidOperationException("Could not set memory manager for stepping operations.");
            }
        }

        /// <summary>
        /// Sets the threading implementation to be used with world stepping operations.
        /// </summary>
        public void SetStepThreadingImplementation(WorldStepThreadingImplementation impl, ThreadingImplementation threadingImpl)
        {
            if (impl == null) NativeMethods.dWorldSetStepThreadingImplementation(id, IntPtr.Zero, IntPtr.Zero);
            else NativeMethods.dWorldSetStepThreadingImplementation(id, impl.info, threadingImpl.Id);
        }

        /// <summary>
        /// Steps the world using the specified time delta.
        /// </summary>
        /// <param name="stepSize">
        /// The number of seconds that the simulation has to advance.
        /// </param>
        /// <exception cref="InsufficientMemoryException">
        /// The memory allocation has failed for operation. In such a case all the
        /// objects remain in unchanged state and simulation can be retried as soon
        /// as more memory is available.
        /// </exception>
        public void Step(dReal stepSize)
        {
            var result = NativeMethods.dWorldStep(id, stepSize);
            if (result == 0)
            {
                throw new InsufficientMemoryException("Memory allocation has failed for stepping operation.");
            }
        }

        /// <summary>
        /// Quick-steps the world using the specified time delta. For large systems
        /// this is much faster than <see cref="Step"/>, but it is less accurate.
        /// </summary>
        /// <param name="stepSize">
        /// The number of seconds that the simulation has to advance.
        /// </param>
        /// <exception cref="InsufficientMemoryException">
        /// The memory allocation has failed for operation. In such a case all the
        /// objects remain in unchanged state and simulation can be retried as soon
        /// as more memory is available.
        /// </exception>
        public void QuickStep(dReal stepSize)
        {
            var result = NativeMethods.dWorldQuickStep(id, stepSize);
            if (result == 0)
            {
                throw new InsufficientMemoryException("Memory allocation has failed for stepping operation.");
            }
        }

        /// <summary>
        /// Converts an impulse to a force.
        /// </summary>
        /// <param name="stepSize">The step size for the next step that will be taken.</param>
        /// <param name="impulse">A linear or angular impulse to a rigid body.</param>
        /// <returns>
        /// The scaled force or torque vector to be added to the rigid body.
        /// </returns>
        public Vector3 ImpulseToForce(dReal stepSize, Vector3 impulse)
        {
            Vector3 force;
            NativeMethods.dWorldImpulseToForce(
                id, stepSize,
                impulse.X, impulse.Y, impulse.Z,
                out force);
            return force;
        }

        /// <summary>
        /// Converts an impulse to a force.
        /// </summary>
        /// <param name="stepSize">The step size for the next step that will be taken.</param>
        /// <param name="impulse">A linear or angular impulse to a rigid body.</param>
        /// <param name="force">
        /// When this method returns, contains the scaled force or torque vector
        /// to be added to the rigid body.
        /// </param>
        public void ImpulseToForce(dReal stepSize, ref Vector3 impulse, out Vector3 force)
        {
            NativeMethods.dWorldImpulseToForce(
                id, stepSize,
                impulse.X, impulse.Y, impulse.Z,
                out force);
        }

        /// <summary>
        /// Sets both linear and angular damping scales.
        /// </summary>
        /// <param name="linearScale">
        /// The linear damping scale that is to be applied to newly created bodies.
        /// </param>
        /// <param name="angularScale">
        /// The angular damping scale that is to be applied to newly created bodies.
        /// </param>
        public void SetDamping(dReal linearScale, dReal angularScale)
        {
            NativeMethods.dWorldSetDamping(id, linearScale, angularScale);
        }

        /// <summary>
        /// Destroys the world and everything in it.
        /// </summary>
        public void Dispose()
        {
            id.Close();
        }
    }
}
