using Ode.Net.Native;
using System;
using System.Runtime.InteropServices;
#if ODE_DOUBLE_PRECISION
using dReal = System.Double;
#else
using dReal = System.Single;
#endif
using dTriIndex = System.UInt32;

namespace Ode.Net.Collision
{
    /// <summary>
    /// Represents a data object which is used to store triangle mesh data.
    /// </summary>
    public sealed class TriMeshData : IDisposable
    {
        readonly dTriMeshDataID id;
        GCHandle verticesData;
        GCHandle indicesData;
        GCHandle normalsData;

        /// <summary>
        /// Initializes a new instance of the <see cref="TriMeshData"/> class.
        /// </summary>
        public TriMeshData()
        {
            id = NativeMethods.dGeomTriMeshDataCreate();
        }

        internal dTriMeshDataID Id
        {
            get { return id; }
        }

        /// <summary>
        /// Builds the triangle mesh data object with single precision vertex data.
        /// </summary>
        /// <param name="vertices">The array of mesh vertices.</param>
        /// <param name="indices">
        /// The array of indices forming the triangle mesh. Each element in the array
        /// represents the index of one of the vertices.
        /// </param>
        public void BuildSingle(float[] vertices, dTriIndex[] indices)
        {
            BuildSingle(vertices, indices, null);
        }

        /// <summary>
        /// Builds the triangle mesh data object with single precision vertex data
        /// and pre-calculated normals.
        /// </summary>
        /// <param name="vertices">The array of mesh vertices.</param>
        /// <param name="indices">
        /// The array of indices forming the triangle mesh. Each element in the array
        /// represents the index of one of the vertices.
        /// </param>
        /// <param name="normals">The array of pre-calculated normals.</param>
        public void BuildSingle(float[] vertices, dTriIndex[] indices, dReal[] normals)
        {
            int vertexCount = vertices.Length / 3;
            int vertexStride = 3 * Marshal.SizeOf(typeof(float));
            int indexCount = indices.Length;
            int triStride = 3 * Marshal.SizeOf(typeof(dTriIndex));
            StoreMeshData(vertices, indices, normals);
            if (normals != null)
            {
                NativeMethods.dGeomTriMeshDataBuildSingle1(
                    id, verticesData.AddrOfPinnedObject(), vertexStride, vertexCount,
                    indicesData.AddrOfPinnedObject(), indexCount, triStride, normalsData.AddrOfPinnedObject());
            }
            else
            {
                NativeMethods.dGeomTriMeshDataBuildSingle(
                id, verticesData.AddrOfPinnedObject(), vertexStride, vertexCount,
                indicesData.AddrOfPinnedObject(), indexCount, triStride);
            }
        }

        /// <summary>
        /// Builds the triangle mesh data object with double precision vertex data.
        /// </summary>
        /// <param name="vertices">The array of mesh vertices.</param>
        /// <param name="indices">
        /// The array of indices forming the triangle mesh. Each element in the array
        /// represents the index of one of the vertices.
        /// </param>
        public void BuildDouble(double[] vertices, dTriIndex[] indices)
        {
            BuildDouble(vertices, indices, null);
        }

        /// <summary>
        /// Builds the triangle mesh data object with double precision vertex data
        /// and pre-calculated normals.
        /// </summary>
        /// <param name="vertices">The array of mesh vertices.</param>
        /// <param name="indices">
        /// The array of indices forming the triangle mesh. Each element in the array
        /// represents the index of one of the vertices.
        /// </param>
        /// <param name="normals">The array of pre-calculated normals.</param>
        public void BuildDouble(double[] vertices, dTriIndex[] indices, dReal[] normals)
        {
            int vertexCount = vertices.Length / 3;
            int vertexStride = 3 * Marshal.SizeOf(typeof(double));
            int indexCount = indices.Length;
            int triStride = 3 * Marshal.SizeOf(typeof(dTriIndex));
            StoreMeshData(vertices, indices, normals);
            if (normals != null)
            {
                NativeMethods.dGeomTriMeshDataBuildDouble1(
                    id, verticesData.AddrOfPinnedObject(), vertexStride, vertexCount,
                    indicesData.AddrOfPinnedObject(), indexCount, triStride, normalsData.AddrOfPinnedObject());
            }
            else
            {
                NativeMethods.dGeomTriMeshDataBuildDouble(
                id, verticesData.AddrOfPinnedObject(), vertexStride, vertexCount,
                indicesData.AddrOfPinnedObject(), indexCount, triStride);
            }
        }

        /// <summary>
        /// Builds the triangle mesh data object with vertex data.
        /// </summary>
        /// <param name="vertices">The array of mesh vertices.</param>
        /// <param name="indices">
        /// The array of indices forming the triangle mesh. Each element in the array
        /// is an index into the vertices array.
        /// </param>
        public void BuildSimple(Vector3[] vertices, dTriIndex[] indices)
        {
            BuildSimple(vertices, indices, null);
        }

        /// <summary>
        /// Builds the triangle mesh data object with vertex data and
        /// pre-calculated normals.
        /// </summary>
        /// <param name="vertices">The array of mesh vertices.</param>
        /// <param name="indices">
        /// The array of indices forming the triangle mesh. Each element in the array
        /// is an index into the vertices array.
        /// </param>
        /// <param name="normals">The array of pre-calculated normals.</param>
        public void BuildSimple(Vector3[] vertices, dTriIndex[] indices, dReal[] normals)
        {
            StoreMeshData(vertices, indices, normals);
            if (normals != null)
            {
                NativeMethods.dGeomTriMeshDataBuildSimple1(
                    id, verticesData.AddrOfPinnedObject(), vertices.Length,
                    indicesData.AddrOfPinnedObject(), indices.Length, normalsData.AddrOfPinnedObject());
            }
            else
            {
                NativeMethods.dGeomTriMeshDataBuildSimple(
                    id, verticesData.AddrOfPinnedObject(), vertices.Length,
                    indicesData.AddrOfPinnedObject(), indices.Length);
            }
        }

        /// <summary>
        /// Preprocesses the trimesh data to remove unnecessary edges and vertices.
        /// </summary>
        public void Preprocess()
        {
            NativeMethods.dGeomTriMeshDataPreprocess(id);
        }

        /// <summary>
        /// Efficiently updates the internal triangle representation when dynamically
        /// deforming mesh vertices.
        /// </summary>
        public void Update()
        {
            NativeMethods.dGeomTriMeshDataUpdate(id);
        }

        private static void ReleaseDataStore(GCHandle storeHandle)
        {
            if (storeHandle.IsAllocated)
                storeHandle.Free();
        }

        private void ReleaseDataStores()
        {
            ReleaseDataStore(verticesData);
            ReleaseDataStore(indicesData);
            ReleaseDataStore(normalsData);
        }

        private void StoreMeshData<TVertex>(TVertex[] vertices, dTriIndex[] indices, dReal[] normals)
        {
            ReleaseDataStores();

            verticesData = GCHandle.Alloc(vertices, GCHandleType.Pinned);
            indicesData = GCHandle.Alloc(indices, GCHandleType.Pinned);
            normalsData = GCHandle.Alloc(normals, GCHandleType.Pinned);
        }

        /// <summary>
        /// Destroys the triangle mesh data.
        /// </summary>
        public void Dispose()
        {
            if (!id.IsClosed)
            {
                ReleaseDataStores();
                id.Close();
            }
        }
    }
}
