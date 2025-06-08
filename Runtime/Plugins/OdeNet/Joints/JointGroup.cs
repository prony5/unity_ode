using System;
using System.Collections.Generic;

namespace Ode.Net.Joints
{
    /// <summary>
    /// Represents a collection of joints.
    /// </summary>
    public sealed class JointGroup : IEnumerable<Joint>
    {
        readonly List<Joint> joints = new List<Joint>();

        internal void Add(Joint joint)
        {
            if (joint == null)
            {
                throw new ArgumentNullException("joint");
            }

            joints.Add(joint);
        }

        /// <summary>
        /// Removes all joints from the <see cref="JointGroup"/>.
        /// </summary>
        public void Clear()
        {
            foreach (var joint in joints)
            {
                joint.Dispose();
            }

            joints.Clear();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="JointGroup"/>.
        /// </summary>
        /// <returns>
        /// An enumerator for the <see cref="JointGroup"/>.
        /// </returns>
        public IEnumerator<Joint> GetEnumerator()
        {
            return joints.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return joints.GetEnumerator();
        }
    }
}
