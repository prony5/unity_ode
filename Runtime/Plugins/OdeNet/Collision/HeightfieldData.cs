using Ode.Net.Native;
using System;
#if ODE_DOUBLE_PRECISION
using dReal = System.Double;
#else
using dReal = System.Single;
#endif

namespace Ode.Net.Collision
{
    /// <summary>
    /// Represents a data object which is used to store heightfield data.
    /// </summary>
    public sealed class HeightfieldData : IDisposable
    {
        readonly dHeightfieldDataID id;
        private dHeightfieldGetHeight _pCallback;

        /// <summary>
        /// Initializes a new instance of the <see cref="HeightfieldData"/> class.
        /// </summary>
        public HeightfieldData()
        {
            id = NativeMethods.dGeomHeightfieldDataCreate();
        }

        internal dHeightfieldDataID Id
        {
            get { return id; }
        }

        /// <summary>
        /// Builds the heightfield data by using a callback to determine the height of the
        /// field elements.
        /// </summary>
        /// <param name="callback">The function which will determine the height data.</param>
        /// <param name="width">The total width of the heightfield along the local x axis.</param>
        /// <param name="depth">The total depth of the heightfield along the local z axis.</param>
        /// <param name="widthSamples">
        /// The number of vertices to sample along the width of the heightfield.
        /// </param>
        /// <param name="depthSamples">
        /// The number of vertices to sample along the depth of the heightfield.
        /// </param>
        /// <param name="scale">A uniform scale applied to all raw height data.</param>
        /// <param name="offset">An offset applied to the scaled height data.</param>
        /// <param name="thickness">
        /// A value subtracted from the lowest height which in effect adds an
        /// additional cuboid to the base of the heightfield. This is used to
        /// prevent geoms from looping under the desired terrain and not registering
        /// as a collision. Note that the thickness is not affected by the scale or
        /// offset parameters.
        /// </param>
        /// <param name="wrap">
        /// If <b>true</b> then the heightfield will infinitely tile in both directions
        /// along the local x and z axes. Otherwise, the heightfield is bounded from
        /// zero to <paramref name="width"/> in the local x axis, and zero to
        /// <paramref name="depth"/> in the local z axis.
        /// </param>
        public void BuildCallback(
            HeightfieldGetHeight callback,
            dReal width, dReal depth, int widthSamples, int depthSamples,
            dReal scale, dReal offset, dReal thickness, bool wrap)
        {
            if (callback == null)
            {
                throw new ArgumentNullException("callback");
            }

            _pCallback = (data, x, z) => callback(x, z); // Keep the reference so the delegate doesn't get garbage collected
            NativeMethods.dGeomHeightfieldDataBuildCallback(
                id, IntPtr.Zero, _pCallback,
                width, depth, widthSamples, depthSamples,
                scale, offset, thickness, wrap ? 1 : 0);
        }

        /// <summary>
        /// Builds the heightfield data from a byte array.
        /// </summary>
        /// <param name="heightData">The array containing the height data.</param>
        /// <param name="width">The total width of the heightfield along the local x axis.</param>
        /// <param name="depth">The total depth of the heightfield along the local z axis.</param>
        /// <param name="widthSamples">
        /// The number of vertices to sample along the width of the heightfield.
        /// </param>
        /// <param name="depthSamples">
        /// The number of vertices to sample along the depth of the heightfield.
        /// </param>
        /// <param name="scale">A uniform scale applied to all raw height data.</param>
        /// <param name="offset">An offset applied to the scaled height data.</param>
        /// <param name="thickness">
        /// A value subtracted from the lowest height which in effect adds an
        /// additional cuboid to the base of the heightfield. This is used to
        /// prevent geoms from looping under the desired terrain and not registering
        /// as a collision. Note that the thickness is not affected by the scale or
        /// offset parameters.
        /// </param>
        /// <param name="wrap">
        /// If <b>true</b> then the heightfield will infinitely tile in both directions
        /// along the local x and z axes. Otherwise, the heightfield is bounded from
        /// zero to <paramref name="width"/> in the local x axis, and zero to
        /// <paramref name="depth"/> in the local z axis.
        /// </param>
        public void BuildByte(
            byte[] heightData,
            dReal width, dReal depth, int widthSamples, int depthSamples,
            dReal scale, dReal offset, dReal thickness, bool wrap)
        {
            if (heightData == null)
            {
                throw new ArgumentNullException("heightData");
            }

            NativeMethods.dGeomHeightfieldDataBuildByte(
                id, heightData, 1,
                width, depth, widthSamples, depthSamples,
                scale, offset, thickness, wrap ? 1 : 0);
        }

        /// <summary>
        /// Builds the heightfield data from a memory pointer.
        /// </summary>
        /// <param name="heightData">A pointer to the buffer containing the height data.</param>
        /// <param name="copy">
        /// If <b>true</b> an internal copy of the data is performed. Otherwise, only a reference
        /// to the data is stored.
        /// </param>
        /// <param name="width">The total width of the heightfield along the local x axis.</param>
        /// <param name="depth">The total depth of the heightfield along the local z axis.</param>
        /// <param name="widthSamples">
        /// The number of vertices to sample along the width of the heightfield.
        /// </param>
        /// <param name="depthSamples">
        /// The number of vertices to sample along the depth of the heightfield.
        /// </param>
        /// <param name="scale">A uniform scale applied to all raw height data.</param>
        /// <param name="offset">An offset applied to the scaled height data.</param>
        /// <param name="thickness">
        /// A value subtracted from the lowest height which in effect adds an
        /// additional cuboid to the base of the heightfield. This is used to
        /// prevent geoms from looping under the desired terrain and not registering
        /// as a collision. Note that the thickness is not affected by the scale or
        /// offset parameters.
        /// </param>
        /// <param name="wrap">
        /// If <b>true</b> then the heightfield will infinitely tile in both directions
        /// along the local x and z axes. Otherwise, the heightfield is bounded from
        /// zero to <paramref name="width"/> in the local x axis, and zero to
        /// <paramref name="depth"/> in the local z axis.
        /// </param>
        public void BuildByte(
            IntPtr heightData, bool copy,
            dReal width, dReal depth, int widthSamples, int depthSamples,
            dReal scale, dReal offset, dReal thickness, bool wrap)
        {
            NativeMethods.dGeomHeightfieldDataBuildByte(
                id, heightData, copy ? 1 : 0,
                width, depth, widthSamples, depthSamples,
                scale, offset, thickness, wrap ? 1 : 0);
        }

        /// <summary>
        /// Builds the heightfield data from a 16-bit signed integer array.
        /// </summary>
        /// <param name="heightData">The array containing the height data.</param>
        /// <param name="width">The total width of the heightfield along the local x axis.</param>
        /// <param name="depth">The total depth of the heightfield along the local z axis.</param>
        /// <param name="widthSamples">
        /// The number of vertices to sample along the width of the heightfield.
        /// </param>
        /// <param name="depthSamples">
        /// The number of vertices to sample along the depth of the heightfield.
        /// </param>
        /// <param name="scale">A uniform scale applied to all raw height data.</param>
        /// <param name="offset">An offset applied to the scaled height data.</param>
        /// <param name="thickness">
        /// A value subtracted from the lowest height which in effect adds an
        /// additional cuboid to the base of the heightfield. This is used to
        /// prevent geoms from looping under the desired terrain and not registering
        /// as a collision. Note that the thickness is not affected by the scale or
        /// offset parameters.
        /// </param>
        /// <param name="wrap">
        /// If <b>true</b> then the heightfield will infinitely tile in both directions
        /// along the local x and z axes. Otherwise, the heightfield is bounded from
        /// zero to <paramref name="width"/> in the local x axis, and zero to
        /// <paramref name="depth"/> in the local z axis.
        /// </param>
        public void BuildShort(
            short[] heightData,
            dReal width, dReal depth, int widthSamples, int depthSamples,
            dReal scale, dReal offset, dReal thickness, bool wrap)
        {
            if (heightData == null)
            {
                throw new ArgumentNullException("heightData");
            }

            NativeMethods.dGeomHeightfieldDataBuildShort(
                id, heightData, 1,
                width, depth, widthSamples, depthSamples,
                scale, offset, thickness, wrap ? 1 : 0);
        }

        /// <summary>
        /// Builds the heightfield data from a 16-bit signed integer memory pointer.
        /// </summary>
        /// <param name="heightData">A pointer to the buffer containing the height data.</param>
        /// <param name="copy">
        /// If <b>true</b> an internal copy of the data is performed. Otherwise, only a reference
        /// to the data is stored.
        /// </param>
        /// <param name="width">The total width of the heightfield along the local x axis.</param>
        /// <param name="depth">The total depth of the heightfield along the local z axis.</param>
        /// <param name="widthSamples">
        /// The number of vertices to sample along the width of the heightfield.
        /// </param>
        /// <param name="depthSamples">
        /// The number of vertices to sample along the depth of the heightfield.
        /// </param>
        /// <param name="scale">A uniform scale applied to all raw height data.</param>
        /// <param name="offset">An offset applied to the scaled height data.</param>
        /// <param name="thickness">
        /// A value subtracted from the lowest height which in effect adds an
        /// additional cuboid to the base of the heightfield. This is used to
        /// prevent geoms from looping under the desired terrain and not registering
        /// as a collision. Note that the thickness is not affected by the scale or
        /// offset parameters.
        /// </param>
        /// <param name="wrap">
        /// If <b>true</b> then the heightfield will infinitely tile in both directions
        /// along the local x and z axes. Otherwise, the heightfield is bounded from
        /// zero to <paramref name="width"/> in the local x axis, and zero to
        /// <paramref name="depth"/> in the local z axis.
        /// </param>
        public void BuildShort(
            IntPtr heightData, bool copy,
            dReal width, dReal depth, int widthSamples, int depthSamples,
            dReal scale, dReal offset, dReal thickness, bool wrap)
        {
            NativeMethods.dGeomHeightfieldDataBuildShort(
                id, heightData, copy ? 1 : 0,
                width, depth, widthSamples, depthSamples,
                scale, offset, thickness, wrap ? 1 : 0);
        }

        /// <summary>
        /// Builds the heightfield data from a single-precision floating point array.
        /// </summary>
        /// <param name="heightData">The array containing the height data.</param>
        /// <param name="width">The total width of the heightfield along the local x axis.</param>
        /// <param name="depth">The total depth of the heightfield along the local z axis.</param>
        /// <param name="widthSamples">
        /// The number of vertices to sample along the width of the heightfield.
        /// </param>
        /// <param name="depthSamples">
        /// The number of vertices to sample along the depth of the heightfield.
        /// </param>
        /// <param name="scale">A uniform scale applied to all raw height data.</param>
        /// <param name="offset">An offset applied to the scaled height data.</param>
        /// <param name="thickness">
        /// A value subtracted from the lowest height which in effect adds an
        /// additional cuboid to the base of the heightfield. This is used to
        /// prevent geoms from looping under the desired terrain and not registering
        /// as a collision. Note that the thickness is not affected by the scale or
        /// offset parameters.
        /// </param>
        /// <param name="wrap">
        /// If <b>true</b> then the heightfield will infinitely tile in both directions
        /// along the local x and z axes. Otherwise, the heightfield is bounded from
        /// zero to <paramref name="width"/> in the local x axis, and zero to
        /// <paramref name="depth"/> in the local z axis.
        /// </param>
        public void BuildSingle(
            float[] heightData,
            dReal width, dReal depth, int widthSamples, int depthSamples,
            dReal scale, dReal offset, dReal thickness, bool wrap)
        {
            if (heightData == null)
            {
                throw new ArgumentNullException("heightData");
            }

            NativeMethods.dGeomHeightfieldDataBuildSingle(
                id, heightData, 1,
                width, depth, widthSamples, depthSamples,
                scale, offset, thickness, wrap ? 1 : 0);
        }

        /// <summary>
        /// Builds the heightfield data from a single-precision floating point memory pointer.
        /// </summary>
        /// <param name="heightData">A pointer to the buffer containing the height data.</param>
        /// <param name="copy">
        /// If <b>true</b> an internal copy of the data is performed. Otherwise, only a reference
        /// to the data is stored.
        /// </param>
        /// <param name="width">The total width of the heightfield along the local x axis.</param>
        /// <param name="depth">The total depth of the heightfield along the local z axis.</param>
        /// <param name="widthSamples">
        /// The number of vertices to sample along the width of the heightfield.
        /// </param>
        /// <param name="depthSamples">
        /// The number of vertices to sample along the depth of the heightfield.
        /// </param>
        /// <param name="scale">A uniform scale applied to all raw height data.</param>
        /// <param name="offset">An offset applied to the scaled height data.</param>
        /// <param name="thickness">
        /// A value subtracted from the lowest height which in effect adds an
        /// additional cuboid to the base of the heightfield. This is used to
        /// prevent geoms from looping under the desired terrain and not registering
        /// as a collision. Note that the thickness is not affected by the scale or
        /// offset parameters.
        /// </param>
        /// <param name="wrap">
        /// If <b>true</b> then the heightfield will infinitely tile in both directions
        /// along the local x and z axes. Otherwise, the heightfield is bounded from
        /// zero to <paramref name="width"/> in the local x axis, and zero to
        /// <paramref name="depth"/> in the local z axis.
        /// </param>
        public void BuildSingle(
            IntPtr heightData, bool copy,
            dReal width, dReal depth, int widthSamples, int depthSamples,
            dReal scale, dReal offset, dReal thickness, bool wrap)
        {
            NativeMethods.dGeomHeightfieldDataBuildSingle(
                id, heightData, copy ? 1 : 0,
                width, depth, widthSamples, depthSamples,
                scale, offset, thickness, wrap ? 1 : 0);
        }

        /// <summary>
        /// Builds the heightfield data from a double-precision floating point array.
        /// </summary>
        /// <param name="heightData">The array containing the height data.</param>
        /// <param name="width">The total width of the heightfield along the local x axis.</param>
        /// <param name="depth">The total depth of the heightfield along the local z axis.</param>
        /// <param name="widthSamples">
        /// The number of vertices to sample along the width of the heightfield.
        /// </param>
        /// <param name="depthSamples">
        /// The number of vertices to sample along the depth of the heightfield.
        /// </param>
        /// <param name="scale">A uniform scale applied to all raw height data.</param>
        /// <param name="offset">An offset applied to the scaled height data.</param>
        /// <param name="thickness">
        /// A value subtracted from the lowest height which in effect adds an
        /// additional cuboid to the base of the heightfield. This is used to
        /// prevent geoms from looping under the desired terrain and not registering
        /// as a collision. Note that the thickness is not affected by the scale or
        /// offset parameters.
        /// </param>
        /// <param name="wrap">
        /// If <b>true</b> then the heightfield will infinitely tile in both directions
        /// along the local x and z axes. Otherwise, the heightfield is bounded from
        /// zero to <paramref name="width"/> in the local x axis, and zero to
        /// <paramref name="depth"/> in the local z axis.
        /// </param>
        public void BuildDouble(
            double[] heightData,
            dReal width, dReal depth, int widthSamples, int depthSamples,
            dReal scale, dReal offset, dReal thickness, bool wrap)
        {
            if (heightData == null)
            {
                throw new ArgumentNullException("heightData");
            }

            NativeMethods.dGeomHeightfieldDataBuildDouble(
                id, heightData, 1,
                width, depth, widthSamples, depthSamples,
                scale, offset, thickness, wrap ? 1 : 0);
        }

        /// <summary>
        /// Builds the heightfield data from a double-precision floating point memory pointer.
        /// </summary>
        /// <param name="heightData">A pointer to the buffer containing the height data.</param>
        /// <param name="copy">
        /// If <b>true</b> an internal copy of the data is performed. Otherwise, only a reference
        /// to the data is stored.
        /// </param>
        /// <param name="width">The total width of the heightfield along the local x axis.</param>
        /// <param name="depth">The total depth of the heightfield along the local z axis.</param>
        /// <param name="widthSamples">
        /// The number of vertices to sample along the width of the heightfield.
        /// </param>
        /// <param name="depthSamples">
        /// The number of vertices to sample along the depth of the heightfield.
        /// </param>
        /// <param name="scale">A uniform scale applied to all raw height data.</param>
        /// <param name="offset">An offset applied to the scaled height data.</param>
        /// <param name="thickness">
        /// A value subtracted from the lowest height which in effect adds an
        /// additional cuboid to the base of the heightfield. This is used to
        /// prevent geoms from looping under the desired terrain and not registering
        /// as a collision. Note that the thickness is not affected by the scale or
        /// offset parameters.
        /// </param>
        /// <param name="wrap">
        /// If <b>true</b> then the heightfield will infinitely tile in both directions
        /// along the local x and z axes. Otherwise, the heightfield is bounded from
        /// zero to <paramref name="width"/> in the local x axis, and zero to
        /// <paramref name="depth"/> in the local z axis.
        /// </param>
        public void BuildDouble(
            IntPtr heightData, bool copy,
            dReal width, dReal depth, int widthSamples, int depthSamples,
            dReal scale, dReal offset, dReal thickness, bool wrap)
        {
            NativeMethods.dGeomHeightfieldDataBuildDouble(
                id, heightData, copy ? 1 : 0,
                width, depth, widthSamples, depthSamples,
                scale, offset, thickness, wrap ? 1 : 0);
        }

        /// <summary>
        /// Sets the minimum and maximum height bounds.
        /// </summary>
        /// <param name="minHeight">
        /// The new minimum height value. Scale, offset and thickness are applied after
        /// setting the new value.
        /// </param>
        /// <param name="maxHeight">
        /// The new maximum height value. Scale, offset and thickness are applied after
        /// setting the new value.
        /// </param>
        public void SetBounds(dReal minHeight, dReal maxHeight)
        {
            NativeMethods.dGeomHeightfieldDataSetBounds(id, minHeight, maxHeight);
        }

        /// <summary>
        /// Destroys the heightfield data.
        /// </summary>
        public void Dispose()
        {
            if (!id.IsClosed)
            {
                id.Close();
            }
        }
    }

    /// <summary>
    /// Represents the method that determines the height of a given element
    /// in the field.
    /// </summary>
    /// <param name="x">The x-coordinate of the field element.</param>
    /// <param name="z">The z-coordinate of the field element.</param>
    /// <returns>The y-coordinate (height) of the field element.</returns>
    public delegate dReal HeightfieldGetHeight(int x, int z);
}
