using Ode.Net.Native;
using System;
using System.Collections.Generic;

namespace Ode.Net.Collision
{
    /// <summary>
    /// Represents a collision space.
    /// </summary>
    public abstract class Space : Geom, IEnumerable<Geom>
    {
        readonly dSpaceID id;

        internal Space(dSpaceID space)
            : base(space)
        {
            id = space;
        }

        internal new dSpaceID Id
        {
            get { return id; }
        }

        internal new static Space FromIntPtr(IntPtr value)
        {
            return (Space)Geom.FromIntPtr(value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether to automatically destroy all
        /// contained geoms when the space is destroyed.
        /// </summary>
        public bool Cleanup
        {
            get { return NativeMethods.dSpaceGetCleanup(id) != 0; }
            set { NativeMethods.dSpaceSetCleanup(id, value ? 1 : 0); }
        }

        /// <summary>
        /// Gets or sets the sublevel value for the space.
        /// </summary>
        public int Sublevel
        {
            get { return NativeMethods.dSpaceGetSublevel(id); }
            set { NativeMethods.dSpaceSetSublevel(id, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the space is elligible for
        /// manual thread data cleanup.
        /// </summary>
        public bool ManualCleanup
        {
            get { return NativeMethods.dSpaceGetManualCleanup(id) != 0; }
            set { NativeMethods.dSpaceSetManualCleanup(id, value ? 1 : 0); }
        }

        /// <summary>
        /// Adds a geom to the space.
        /// </summary>
        /// <param name="geom">The geom to add to the space.</param>
        public void Add(Geom geom)
        {
            if (geom == null)
            {
                throw new ArgumentNullException("geom");
            }

            NativeMethods.dSpaceAdd(id, geom.Id);
        }

        /// <summary>
        /// Removes a geom from the space.
        /// </summary>
        /// <param name="geom">The geom to remove from the space.</param>
        public void Remove(Geom geom)
        {
            if (geom == null)
            {
                throw new ArgumentNullException("geom");
            }

            NativeMethods.dSpaceRemove(id, geom.Id);
        }

        /// <summary>
        /// Determines whether the space contains the specified geom.
        /// </summary>
        /// <param name="geom">The geom to locate in the space.</param>
        /// <returns>
        /// <b>true</b> if the space contains the specified geom;
        /// otherwise, <b>false</b>.
        /// </returns>
        public bool Contains(Geom geom)
        {
            return NativeMethods.dSpaceQuery(id, geom.Id) != 0;
        }

        /// <summary>
        /// Removes all the geoms from the space.
        /// </summary>
        public void Clear()
        {
            NativeMethods.dSpaceClean(id);
        }

        /// <summary>
        /// Determines which pairs of geoms in the space may potentially intersect,
        /// and calls the callback function with each candidate pair.
        /// </summary>
        /// <param name="callback">
        /// The callback function that will be called for each candidate pair.
        /// </param>
        public void Collide(NearCallback callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException("callback");
            }

            NativeMethods.dSpaceCollide(id, IntPtr.Zero, (data, o1, o2) =>
            {
                var geom1 = Geom.FromIntPtr(o1);
                var geom2 = Geom.FromIntPtr(o2);
                callback(geom1, geom2);
            });
        }

        /// <summary>
        /// Determines all potentially intersecting pairs between geoms from the space
        /// and the specified geom. The exact behavior depends on the type of the geom.
        /// </summary>
        /// <param name="geom">
        /// The geom to test for potential intersection with the space.
        /// </param>
        /// <param name="callback">
        /// The callback function that will be called for each candidate pair.
        /// </param>
        public void Collide(Geom geom, NearCallback callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException("callback");
            }

            NativeMethods.dSpaceCollide2(id, geom.Id, IntPtr.Zero, (data, o1, o2) =>
            {
                var geom1 = Geom.FromIntPtr(o1);
                var geom2 = Geom.FromIntPtr(o2);
                callback(geom1, geom2);
            });
        }

        /// <summary>
        /// Returns an enumerator that iterates through the space.
        /// </summary>
        /// <returns>
        /// An <see cref="IEnumerator{Geom}"/> that can be used to iterate through
        /// the space.
        /// </returns>
        public IEnumerator<Geom> GetEnumerator()
        {
            var numGeoms = NativeMethods.dSpaceGetNumGeoms(id);
            for (int i = 0; i < numGeoms; i++)
            {
                yield return Geom.FromIntPtr(NativeMethods.dSpaceGetGeom(id, i));
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    /// <summary>
    /// Represents the method that handles pairs of potentially intersecting geoms.
    /// </summary>
    /// <param name="geom1">The first geom that potentially intersects.</param>
    /// <param name="geom2">The second geom that potentially intersects.</param>
    public delegate void NearCallback(Geom geom1, Geom geom2);
}
