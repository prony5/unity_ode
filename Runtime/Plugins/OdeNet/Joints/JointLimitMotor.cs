using Ode.Net.Native;
#if ODE_DOUBLE_PRECISION
using dReal = System.Double;
#else
using dReal = System.Single;
#endif

namespace Ode.Net.Joints
{
    /// <summary>
    /// Represents the limit and motor parameters of a joint axis.
    /// </summary>
    public abstract class JointLimitMotor
    {
        const int dParamGroup = 0x100;
        readonly dJointID id;
        readonly int index;

        internal JointLimitMotor(Joint joint, int axis)
        {
            id = joint.id;
            index = axis;
        }

        internal abstract dReal GetParam(dJointID id, dJointParam parameter);

        internal abstract void SetParam(dJointID id, dJointParam parameter, dReal value);

        /// <summary>
        /// Gets or sets the low stop angle or position.
        /// </summary>
        /// <remarks>
        /// Setting this to -Infinity (the default value) turns off the low stop.
        /// For rotational joints, this stop must be greater than -pi to be effective.
        /// </remarks>
        public dReal LowStop
        {
            get { return GetParam(id, index * dParamGroup + dJointParam.dParamLoStop); }
            set { SetParam(id, index * dParamGroup + dJointParam.dParamLoStop, value); }
        }

        /// <summary>
        /// Gets or sets the high stop angle or position.
        /// </summary>
        /// <remarks>
        /// Setting this to dInfinity (the default value) turns off the high stop.
        /// For rotational joints, this stop must be less than pi to be effective.
        /// If the high stop is less than the low stop then both stops will be ineffective.
        /// </remarks>
        public dReal HighStop
        {
            get { return GetParam(id, index * dParamGroup + dJointParam.dParamHiStop); }
            set { SetParam(id, index * dParamGroup + dJointParam.dParamHiStop, value); }
        }

        /// <summary>
        /// Gets or sets the desired motor velocity (this will be an angular or linear velocity).
        /// </summary>
        public dReal Velocity
        {
            get { return GetParam(id, index * dParamGroup + dJointParam.dParamVel); }
            set { SetParam(id, index * dParamGroup + dJointParam.dParamVel, value); }
        }

        /// <summary>
        /// Gets or sets the maximum force or torque that the motor will use to achieve the
        /// desired velocity.
        /// </summary>
        /// <remarks>
        /// This must always be greater than or equal to zero. Setting this to
        /// zero (the default value) turns off the motor.
        /// </remarks>
        public dReal MaxForce
        {
            get { return GetParam(id, index * dParamGroup + dJointParam.dParamFMax); }
            set { SetParam(id, index * dParamGroup + dJointParam.dParamFMax, value); }
        }

        /// <summary>
        /// Gets or sets a scale factor to prevent excess force being applied at the stops.
        /// </summary>
        /// <remarks>
        /// It should have a value between zero and one (the default value).
        /// If the jumping motion is too visible in a joint, the value can be
        /// reduced. Making this value too small can prevent the motor from
        /// being able to move the joint away from a stop.
        /// </remarks>
        public dReal FudgeFactor
        {
            get { return GetParam(id, index * dParamGroup + dJointParam.dParamFudgeFactor); }
            set { SetParam(id, index * dParamGroup + dJointParam.dParamFudgeFactor, value); }
        }

        /// <summary>
        /// Gets or sets a value controlling the restitution strength at the stops.
        /// </summary>
        /// <remarks>
        /// This is a restitution parameter in the range 0..1. 0 means the
        /// stops are not bouncy at all, 1 means maximum bounciness.
        /// </remarks>
        public dReal Bounce
        {
            get { return GetParam(id, index * dParamGroup + dJointParam.dParamBounce); }
            set { SetParam(id, index * dParamGroup + dJointParam.dParamBounce, value); }
        }

        /// <summary>
        /// Gets or sets the constraint force mixing value used when not at a stop.
        /// </summary>
        public dReal Cfm
        {
            get { return GetParam(id, index * dParamGroup + dJointParam.dParamCFM); }
            set { SetParam(id, index * dParamGroup + dJointParam.dParamCFM, value); }
        }

        /// <summary>
        /// Gets or sets the error reduction parameter used at the stops.
        /// </summary>
        public dReal StopErp
        {
            get { return GetParam(id, index * dParamGroup + dJointParam.dParamStopERP); }
            set { SetParam(id, index * dParamGroup + dJointParam.dParamStopERP, value); }
        }

        /// <summary>
        /// Gets or sets the constraint force mixing value used at the stops.
        /// </summary>
        /// <remarks>
        /// Together with the ERP value this can be used to get spongy or soft stops.
        /// Note that this is intended for unpowered joints, it does not really work
        /// as expected when a powered joint reaches its limit.
        /// </remarks>
        public dReal StopCfm
        {
            get { return GetParam(id, index * dParamGroup + dJointParam.dParamStopCFM); }
            set { SetParam(id, index * dParamGroup + dJointParam.dParamStopCFM, value); }
        }
    }
}
