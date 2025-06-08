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
    /// Represents a triangle mesh geom.
    /// </summary>
    public sealed class TriMesh : Geom
    {
        TriMeshData data;
        TriangleCallback callback;
        TriangleArrayCallback arrayCallback;
        TriangleRayCallback rayCallback;
        TriangleTriangleMergeCallback triangleMergeCallback;

        /// <summary>
        /// Initializes a new instance of the <see cref="TriMesh"/> class with the
        /// specified data.
        /// </summary>
        /// <param name="data">The triangle mesh data object.</param>
        public TriMesh(TriMeshData data)
            : this(null, data)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TriMesh"/> class on the
        /// given space and with the specified data.
        /// </summary>
        /// <param name="space">The space that is to contain the geom.</param>
        /// <param name="data">The triangle mesh data object.</param>
        public TriMesh(Space space, TriMeshData data)
            : base(NativeMethods.dCreateTriMesh(space != null ? space.Id : dSpaceID.Null, data.Id, null, null, null))
        {
            this.data = data;
        }

        /// <summary>
        /// Gets or sets the triangle mesh data object.
        /// </summary>
        public TriMeshData Data
        {
            get { return data; }
            set
            {
                data = value;
                NativeMethods.dGeomTriMeshSetData(Id, value.Id);
            }
        }

        /// <summary>
        /// Gets the number of triangles in the triangle mesh.
        /// </summary>
        public int TriangleCount
        {
            get { return NativeMethods.dGeomTriMeshGetTriangleCount(Id); }
        }

        /// <summary>
        /// Gets or sets the optional per triangle callback that determines
        /// whether a collision with a particular triangle is wanted.
        /// </summary>
        public TriangleCallback Callback
        {
            get { return callback; }
            set
            {
                if (value == null) NativeMethods.dGeomTriMeshSetCallback(Id, null);
                else NativeMethods.dGeomTriMeshSetCallback(Id, TriCallback);
                callback = value;
            }
        }

        /// <summary>
        /// Gets or sets the optional per geom callback that will receive the
        /// list of all intersecting triangles.
        /// </summary>
        public TriangleArrayCallback ArrayCallback
        {
            get { return arrayCallback; }
            set
            {
                if (value == null) NativeMethods.dGeomTriMeshSetArrayCallback(Id, null);
                else NativeMethods.dGeomTriMeshSetArrayCallback(Id, TriArrayCallback);
                arrayCallback = value;
            }
        }

        /// <summary>
        /// Gets or sets the optional ray callback that determines whether a ray
        /// collides with a triangle based on the barycentric coordinates of
        /// the intersection.
        /// </summary>
        public TriangleRayCallback RayCallback
        {
            get { return rayCallback; }
            set
            {
                if (value == null) NativeMethods.dGeomTriMeshSetRayCallback(Id, null);
                else NativeMethods.dGeomTriMeshSetRayCallback(Id, TriRayCallback);
                rayCallback = value;
            }
        }

        /// <summary>
        /// Gets or sets the optional triangle merge callback that allows specifying
        /// triangle indices resulting from merging two other triangle contacts.
        /// </summary>
        public TriangleTriangleMergeCallback TriangleMergeCallback
        {
            get { return triangleMergeCallback; }
            set
            {
                if (value == null) NativeMethods.dGeomTriMeshSetTriMergeCallback(Id, null);
                else NativeMethods.dGeomTriMeshSetTriMergeCallback(Id, TriTriMergeCallback);
                triangleMergeCallback = value;
            }
        }

        private int TriCallback(IntPtr triMeshID, IntPtr refObjectID, int triangleIndex)
        {
            var triMesh = (TriMesh)Geom.FromIntPtr(triMeshID);
            var geom = Geom.FromIntPtr(refObjectID);
            return callback(triMesh, geom, triangleIndex) ? 1 : 0;
        }

        private void TriArrayCallback(IntPtr triMeshID, IntPtr refObjectID, IntPtr triIndices, int triCount)
        {
            var triMesh = (TriMesh)Geom.FromIntPtr(triMeshID);
            var geom = Geom.FromIntPtr(refObjectID);
            var triangleIndices = new int[triCount];
            Marshal.Copy(triIndices, triangleIndices, 0, triCount);
            arrayCallback(triMesh, geom, triangleIndices);
        }

        private int TriRayCallback(IntPtr triMeshID, IntPtr rayID, int triangleIndex, dReal u, dReal v)
        {
            var triMesh = (TriMesh)Geom.FromIntPtr(triMeshID);
            var ray = (Ray)Geom.FromIntPtr(rayID);
            return rayCallback(triMesh, ray, triangleIndex, u, v) ? 1 : 0;
        }

        private int TriTriMergeCallback(IntPtr triMeshID, int firstTriangleIndex, int secondTriangleIndex)
        {
            var triMesh = (TriMesh)Geom.FromIntPtr(triMeshID);
            return triangleMergeCallback(triMesh, firstTriangleIndex, secondTriangleIndex);
        }

        /// <summary>
        /// Enables or disables temporal coherence during triangle mesh collisions
        /// with the specified geometry class.
        /// </summary>
        /// <param name="geomClass">The geometry class for which to set temporal coherence.</param>
        /// <param name="value">
        /// <b>true</b> if temporal coherence should be enabled for the specified
        /// geometry class; otherwise, <b>false</b>.
        /// </param>
        public void EnableTemporalCoherence(GeomClass geomClass, bool value)
        {
            NativeMethods.dGeomTriMeshEnableTC(Id, geomClass, value ? 1 : 0);
        }

        /// <summary>
        /// Checks whether temporal coherence is enabled during triangle mesh
        /// collisions with the specified geometry class.
        /// </summary>
        /// <param name="geomClass">The geometry class to check.</param>
        /// <returns>
        /// <b>true</b> if temporal coherence is enabled for the specified
        /// geometry class; otherwise, <b>false</b>.
        /// </returns>
        public bool IsTemporalCoherenceEnabled(GeomClass geomClass)
        {
            return NativeMethods.dGeomTriMeshIsTCEnabled(Id, geomClass) != 0;
        }

        /// <summary>
        /// Clears the internal temporal coherence caches.
        /// </summary>
        public void ClearTemporalCoherenceCache()
        {
            NativeMethods.dGeomTriMeshClearTCCache(Id);
        }

        /// <summary>
        /// Gets the vertices of a triangle in world space.
        /// </summary>
        /// <param name="index">The index of the triangle.</param>
        /// <param name="v0">The first vertex of the triangle, in world space.</param>
        /// <param name="v1">The second vertex of the triangle, in world space.</param>
        /// <param name="v2">The third vertex of the triangle, in world space.</param>
        public void GetTriangle(int index, out Vector3 v0, out Vector3 v1, out Vector3 v2)
        {
            NativeMethods.dGeomTriMeshGetTriangle(Id, index, out v0, out v1, out v2);
        }

        /// <summary>
        /// Gets a position on the surface of a triangle, in world space.
        /// </summary>
        /// <param name="index">The index of the triangle for which to compute the position.</param>
        /// <param name="u">The u component in barycentric coordinates.</param>
        /// <param name="v">The v component in barycentric coordinates.</param>
        /// <param name="result">
        /// The position on the surface of the triangle, in world space.
        /// </param>
        public void GetPoint(int index, dReal u, dReal v, out Vector3 result)
        {
            NativeMethods.dGeomTriMeshGetPoint(Id, index, u, v, out result);
        }

        /// <summary>
        /// Gets a position on the surface of a triangle, in world space.
        /// </summary>
        /// <param name="index">The index of the triangle for which to compute the position.</param>
        /// <param name="u">The u component in barycentric coordinates.</param>
        /// <param name="v">The v component in barycentric coordinates.</param>
        /// <returns>The position on the surface of the triangle, in world space.</returns>
        public Vector3 GetPoint(int index, dReal u, dReal v)
        {
            Vector3 result;
            GetPoint(index, u, v, out result);
            return result;
        }
    }

    /// <summary>
    /// Represents the method that is called per triangle collision to
    /// determine whether a collision with a particular geom is wanted.
    /// </summary>
    /// <param name="triMesh">The triangle mesh containing the colliding triangle.</param>
    /// <param name="geom">The geom colliding with the triangle.</param>
    /// <param name="triangleIndex">The index of the colliding triangle.</param>
    /// <returns>
    /// <b>true</b> if the collision should generate contacts; otherwise, <b>false</b>.
    /// </returns>
    public delegate bool TriangleCallback(TriMesh triMesh, Geom geom, int triangleIndex);

    /// <summary>
    /// Represents the method that is called per geom collision to
    /// handle the list of all intersecting triangles.
    /// </summary>
    /// <param name="triMesh">The triangle mesh containing the colliding triangles.</param>
    /// <param name="geom">The geom colliding with the triangles.</param>
    /// <param name="triangleIndices">The array of indices of the colliding triangles.</param>
    public delegate void TriangleArrayCallback(TriMesh triMesh, Geom geom, int[] triangleIndices);

    /// <summary>
    /// Represents the method that is called per ray collision to
    /// determine whether the ray collides with a triangle in barycentric
    /// coordinates.
    /// </summary>
    /// <param name="triMesh">The triangle mesh containing the colliding triangle.</param>
    /// <param name="ray">The ray colliding with the triangle.</param>
    /// <param name="triangleIndex">The index of the colliding triangle.</param>
    /// <param name="u">The u component in barycentric coordinates of the intersection.</param>
    /// <param name="v">The v component in barycentric coordinates of the intersection.</param>
    /// <returns>
    /// <b>true</b> if the collision should generate contacts; otherwise, <b>false</b>.
    /// </returns>
    public delegate bool TriangleRayCallback(TriMesh triMesh, Ray ray, int triangleIndex, dReal u, dReal v);

    /// <summary>
    /// Represents the method that is called to allow specifying the triangle
    /// indices resulting from merging two other triangle contacts.
    /// </summary>
    /// <param name="triMesh">The triangle mesh containing the merged triangles.</param>
    /// <param name="firstTriangleIndex">The index of the first merged triangle.</param>
    /// <param name="secondTriangleIndex">The index of the second merged triangle.</param>
    /// <returns>The index of the merged triangle.</returns>
    public delegate int TriangleTriangleMergeCallback(TriMesh triMesh, int firstTriangleIndex, int secondTriangleIndex);
}
