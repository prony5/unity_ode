using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;

namespace Ode.Net.Native
{
    class dJointFeedbackHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        internal static readonly dJointFeedbackHandle Null = new NulldJointFeedbackHandle();

        internal dJointFeedbackHandle()
            : base(true)
        {
            var h = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(dJointFeedback)));
            SetHandle(h);
        }

        private dJointFeedbackHandle(bool ownsHandle)
            : base(ownsHandle)
        {
        }

        private dJointFeedback Feedback { get { return (dJointFeedback)Marshal.PtrToStructure(handle, typeof(dJointFeedback)); } }

        internal Vector3 ForceOnBody1 { get { return Feedback.f1; } }
        internal Vector3 TorqueOnBody1 { get { return Feedback.t1; } }
        internal Vector3 ForceOnBody2 { get { return Feedback.f2; } }
        internal Vector3 TorqueOnBody2 { get { return Feedback.t2; } }

        protected override bool ReleaseHandle()
        {
            Marshal.FreeHGlobal(handle);
            return true;
        }

        class NulldJointFeedbackHandle : dJointFeedbackHandle
        {
            public NulldJointFeedbackHandle()
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
