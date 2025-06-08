using Ode.Net.Native;
using System;
using System.Runtime.InteropServices;

namespace Ode.Net
{
    /// <summary>
    /// Represents a function to allocate a memory block of a specified size.
    /// </summary>
    /// <param name="blockSize">The size of the memory block.</param>
    /// <returns>A pointer to the newly allocated memory block.</returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate IntPtr AllocateBlock(IntPtr blockSize);

    /// <summary>
    /// Represents a function to shrink an existing memory block to a smaller size.
    /// The contents of the block head must be preserved while shrinking. The new
    /// block size is guaranteed to be always less than the existing one.
    /// </summary>
    /// <param name="blockPointer">A pointer to an existing memory block.</param>
    /// <param name="blockCurrentSize">The current size of the memory block.</param>
    /// <param name="blockSmallerSize">The new size of the memory block.</param>
    /// <returns>A pointer to the reallocated memory block.</returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate IntPtr ShrinkBlock(IntPtr blockPointer, IntPtr blockCurrentSize, IntPtr blockSmallerSize);

    /// <summary>
    /// Represents a function to delete an existing memory block.
    /// </summary>
    /// <param name="blockPointer">A pointer to an existing memory block.</param>
    /// <param name="blockCurrentSize">The current size of the memory block.</param>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void FreeBlock(IntPtr blockPointer, IntPtr blockCurrentSize);

    /// <summary>
    /// Represents the memory manager used by world stepping functions.
    /// </summary>
    public class WorldStepMemoryManager
    {
        internal dWorldStepMemoryFunctionsInfo info;
        readonly AllocateBlock allocate;
        readonly ShrinkBlock shrink;
        readonly FreeBlock free;

        /// <summary>
        /// Initializes a new instance of the <see cref="WorldStepMemoryManager"/> class
        /// with the specified memory handling callbacks.
        /// </summary>
        /// <param name="allocateBlock">
        /// A function to allocate a memory block of a specified size.
        /// </param>
        /// <param name="shrinkBlock">
        /// A function to shrink an existing memory block to a smaller size.
        /// </param>
        /// <param name="freeBlock">
        /// A function to delete an existing memory block.
        /// </param>
        public WorldStepMemoryManager(
            AllocateBlock allocateBlock,
            ShrinkBlock shrinkBlock,
            FreeBlock freeBlock)
        {
            info.struct_size = (uint)Marshal.SizeOf(typeof(dWorldStepMemoryFunctionsInfo));
            info.alloc_block = Marshal.GetFunctionPointerForDelegate(allocateBlock);
            info.shrink_block = Marshal.GetFunctionPointerForDelegate(shrinkBlock);
            info.free_block = Marshal.GetFunctionPointerForDelegate(freeBlock);
            allocate = allocateBlock;
            shrink = shrinkBlock;
            free = freeBlock;
        }

        /// <summary>
        /// Gets the function used to allocate new memory blocks.
        /// </summary>
        public AllocateBlock Allocate
        {
            get { return allocate; }
        }

        /// <summary>
        /// Gets the function used to shrink existing memory blocks to a smaller size.
        /// </summary>
        public ShrinkBlock Shrink
        {
            get { return shrink; }
        }

        /// <summary>
        /// Gets the function used to delete existing memory blocks.
        /// </summary>
        public FreeBlock Free
        {
            get { return free; }
        }
    }
}
