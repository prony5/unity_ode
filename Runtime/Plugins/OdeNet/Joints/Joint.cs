using Ode.Net.Native;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Ode.Net.Joints
{
    /// <summary>
    /// Represents a joint, or constraint, enforced between two bodies.
    /// </summary>
    public abstract class Joint : IDisposable
    {
        internal readonly dJointID id;
        readonly GCHandle handle;
        JointFeedback feedback;

        internal Joint(dJointID joint, JointGroup group)
        {
            id = joint;
            handle = GCHandle.Alloc(this);
            NativeMethods.dJointSetData(id, GCHandle.ToIntPtr(handle));

            if (group != null)
            {
                group.Add(this);
            }
        }

        internal static Joint FromIntPtr(IntPtr value)
        {
            if (value == IntPtr.Zero) return null;
            var handlePtr = NativeMethods.dJointGetData(value);
            if (handlePtr == IntPtr.Zero) return null;
            var handle = GCHandle.FromIntPtr(handlePtr);
            return (Joint)handle.Target;
        }

        /// <summary>
        /// Gets the number of bodies attached to the joint.
        /// </summary>
        public int NumBodies
        {
            get { return NativeMethods.dJointGetNumBodies(id); }
        }

        /// <summary>
        /// Gets the bodies that this joint connects.
        /// </summary>
        public IEnumerable<Body> Bodies
        {
            get
            {
                var numBodies = NativeMethods.dJointGetNumBodies(id);
                for (int i = 0; i < numBodies; i++)
                {
                    var body = Body.FromIntPtr(NativeMethods.dJointGetBody(id, i));
                    if (body == null) continue;
                    yield return body;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the joint is enabled.
        /// </summary>
        public bool Enabled
        {
            get { return NativeMethods.dJointIsEnabled(id) != 0; }
            set
            {
                if (value) NativeMethods.dJointEnable(id);
                else NativeMethods.dJointDisable(id);
            }
        }

        /// <summary>
        /// Gets or sets the object that contains data about the joint.
        /// </summary>
        public object Tag { get; set; }

        /// <summary>
        /// Gets the type of the joint.
        /// </summary>
        public JointType Type
        {
            get { return NativeMethods.dJointGetType(id); }
        }

        /// <summary>
        /// Gets or sets the object that is to receive the joint feedback.
        /// </summary>
        public JointFeedback Feedback
        {
            get { return feedback; }
            set
            {
                var handle = value != null ? value.Handle : dJointFeedbackHandle.Null;
                NativeMethods.dJointSetFeedback(id, handle);
                feedback = value;
            }
        }

        /// <summary>
        /// Attaches the joint to some new bodies. Setting both bodies to <b>null</b>
        /// makes the joint have no effect in the simulation.
        /// </summary>
        /// <param name="body1">
        /// The first body. A <b>null</b> value refers to the static environment.
        /// </param>
        /// <param name="body2">
        /// The second body. A <b>null</b> value refers to the static environment.
        /// </param>
        public void Attach(Body body1, Body body2)
        {
            var b1 = body1 != null ? body1.Id : dBodyID.Null;
            var b2 = body2 != null ? body2.Id : dBodyID.Null;
            NativeMethods.dJointAttach(id, b1, b2);
        }

        /// <summary>
        /// Gets the bodies that this joint connects.
        /// </summary>
        /// <param name="index">The index of the body to retrieve.</param>
        /// <returns>
        /// The attached joint body with the specified index. A <b>null</b> value
        /// refers to the static environment.
        /// </returns>
        public Body GetBody(int index)
        {
            var body = NativeMethods.dJointGetBody(id, index);
            return Body.FromIntPtr(body);
        }

        /// <summary>
        /// Destroys the joint.
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
