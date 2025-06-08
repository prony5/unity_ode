using Ode.Net.Native;
using System;
using System.Runtime.InteropServices;
#if ODE_DOUBLE_PRECISION
using dReal = System.Double;
#else
using dReal = System.Single;
#endif

namespace Ode.Net.Collision
{
    /// <summary>
    /// Represents a convex hull geom.
    /// </summary>
    public sealed class Convex : Geom
    {
        ConvexData data;

        /// <summary>
        /// Initializes a new instance of the <see cref="Convex"/> class with the
        /// specified convex hull data.
        /// </summary>
        /// <param name="planes">
        /// The planes' definition array. Each polygon in the convex hull will be
        /// attached to one of these planes. Each plane is defined by a quadruplet
        /// of the a, b, c and d parameters of the plane equation.
        /// </param>
        /// <param name="points">
        /// The array of points, aligned in triplets of x, y and z components.
        /// </param>
        /// <param name="polygons">
        /// The polygons' definition array. Each element in the array is an index
        /// to a previously defined point in the array of point triplets.
        /// Each polygon definition sequence begins and ends in the same point.
        /// There must be as many polygon definition sequences as the number of planes.
        /// </param>
        public Convex(dReal[] planes, dReal[] points, uint[] polygons)
            : this(null, planes, points, polygons)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Convex"/> class on the
        /// given space and with the specified convex hull data.
        /// </summary>
        /// <param name="space">The space that is to contain the geom.</param>
        /// <param name="planes">
        /// The planes' definition array. Each polygon in the convex hull will be
        /// attached to one of these planes. Each plane is defined by a quadruplet
        /// of the a, b, c and d parameters of the plane equation.
        /// </param>
        /// <param name="points">
        /// The array of points, aligned in triplets of x, y and z components.
        /// </param>
        /// <param name="polygons">
        /// The polygons' definition array. Each element in the array is an index
        /// to a previously defined point in the array of point triplets.
        /// Each polygon definition sequence begins and ends in the same point.
        /// There must be as many polygon definition sequences as the number of planes.
        /// </param>
        public Convex(Space space, dReal[] planes, dReal[] points, uint[] polygons)
            : this(space, new ConvexData(planes, points, polygons))
        {
        }

        private Convex(Space space, ConvexData data)
            : base(NativeMethods.dCreateConvex(space != null ? space.Id : dSpaceID.Null,
                                               data._planes.AddrOfPinnedObject(), data._planeCount,
                                               data._points.AddrOfPinnedObject(), data._pointCount,
                                               data._polygons.AddrOfPinnedObject()))
        {
            this.data = data;
        }

        /// <summary>
        /// Sets the convex hull data.
        /// </summary>
        /// <param name="planes">
        /// The planes' definition array. Each polygon in the convex hull will be
        /// attached to one of these planes. Each plane is defined by a quadruplet
        /// of the a, b, c and d parameters of the plane equation.
        /// </param>
        /// <param name="points">
        /// The array of points, aligned in triplets of x, y and z components.
        /// </param>
        /// <param name="polygons">
        /// The polygons' definition array. Each element in the array is an index
        /// to a previously defined point in the array of point triplets.
        /// Each polygon definition sequence begins and ends in the same point.
        /// There must be as many polygon definition sequences as the number of planes.
        /// </param>
        public void SetConvex(dReal[] planes, dReal[] points, uint[] polygons)
        {
            if (data != null)
            {
                data.Dispose();
            }

            data = new ConvexData(planes, points, polygons);
            NativeMethods.dGeomSetConvex(Id, data._planes.AddrOfPinnedObject(), data._planeCount,
                                         data._points.AddrOfPinnedObject(), data._pointCount,
                                         data._polygons.AddrOfPinnedObject());
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="Convex"/> class
        /// specifying whether to perform a normal dispose operation.
        /// </summary>
        /// <param name="disposing">
        /// <b>true</b> for a normal dispose operation; <b>false</b> to finalize
        /// the geom.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            if (data != null)
            {
                if (disposing)
                {
                    data.Dispose();
                    data = null;
                }
            }

            base.Dispose(disposing);
        }

        class ConvexData : IDisposable
        {
            internal GCHandle _planes;
            internal uint _planeCount;
            internal GCHandle _points;
            internal uint _pointCount;
            internal GCHandle _polygons;

            internal ConvexData(dReal[] planes, dReal[] points, uint[] polygons)
            {
                _planeCount = (uint)planes.Length / 4;
                _pointCount = (uint)points.Length / 3;
                _planes = GCHandle.Alloc(planes, GCHandleType.Pinned);
                _points = GCHandle.Alloc(points, GCHandleType.Pinned);
                _polygons = GCHandle.Alloc(polygons, GCHandleType.Pinned);
            }

            ~ConvexData()
            {
                Dispose(false);
            }

            private static void ReleaseDataStore(GCHandle storeHandle)
            {
                if (storeHandle.IsAllocated)
                    storeHandle.Free();
            }

            private void Dispose(bool disposing)
            {
                ReleaseDataStore(_planes);
                ReleaseDataStore(_points);
                ReleaseDataStore(_polygons);
            }

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
        }
    }
}
