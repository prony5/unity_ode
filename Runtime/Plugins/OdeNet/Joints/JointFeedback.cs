using Ode.Net.Native;
using System;

namespace Ode.Net.Joints
{
    /// <summary>
    /// Represents a data object that stores the feedback forces applied by
    /// a joint on its connected bodies.
    /// </summary>
    /// <remarks>
    /// All memory management of joint feedbacks is, for now, left
    /// entirely up to the user. Be sure not to use additional references
    /// to the same JointFeedback structure after calling Dispose().
    /// The user is also expected to manage the reference which is registered
    /// with a given Joint. If you dispose a JointFeedback without first
    /// setting the reference on the Joint to null, you risk a memory
    /// violation exception.
    /// This behaviour may be refined in the future to adhere to managed
    /// code memory management design patterns. For now it is left as is from
    /// the ODE lower-level library code.
    /// </remarks>
    public sealed class JointFeedback : IDisposable
    {
        readonly dJointFeedbackHandle handle;

        /// <summary>
        /// Initializes a new instance of the <see cref="JointFeedback"/> class.
        /// </summary>
        public JointFeedback()
        {
            handle = new dJointFeedbackHandle();
        }

        internal dJointFeedbackHandle Handle
        {
            get { return handle; }
        }

        /// <summary>
        /// Gets the force that the joint applies to the first body.
        /// </summary>
        public Vector3 ForceOnBody1
        {
            get { return handle.ForceOnBody1; }
        }

        /// <summary>
        /// Gets the torque that the joint applies to the first body.
        /// </summary>
        public Vector3 TorqueOnBody1
        {
            get { return handle.TorqueOnBody1; }
        }

        /// <summary>
        /// Gets the force that the joint applies to the second body.
        /// </summary>
        public Vector3 ForceOnBody2
        {
            get { return handle.ForceOnBody2; }
        }

        /// <summary>
        /// Gets the torque that the joint applies to the second body.
        /// </summary>
        public Vector3 TorqueOnBody2
        {
            get { return handle.TorqueOnBody2; }
        }

        /// <summary>
        /// Destroys the feedback data object.
        /// </summary>
        public void Dispose()
        {
            if (!handle.IsClosed)
            {
                handle.Close();
            }
        }
    }
}
