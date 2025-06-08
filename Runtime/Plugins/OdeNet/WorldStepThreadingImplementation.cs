using System;
using Ode.Net.Native;

namespace Ode.Net
{
    /// <summary>
    /// A multi-threaded implementation is a type of implementation that has to be 
    /// served with a thread pool. The thread pool can be either the built-in ODE object
    /// or set of external threads that dedicate themselves to this purpose and stay
    /// in ODE until implementation releases them.
    /// </summary>
    public class ThreadingImplementation : IDisposable
    {
        private dThreadingImplementationID id;

        internal dThreadingImplementationID Id
        {
            get { return id; }
        }

        private ThreadingImplementation()
        {
        }

        /// <summary>
        /// Allocates built-in self-threaded threading implementation object.
        ///
        /// A self-threaded implementation is a type of implementation that performs 
        /// processing of posted calls by means of caller thread itself. This type of 
        /// implementation does not need thread pool to serve it.
        ///
        /// The processing is arranged in a way to prevent call stack depth growth 
        /// as more and more nested calls are posted.
        ///
        /// Note that it is not necessary to create and assign a self-threaded 
        /// implementation to a world, as there is a global one used by default 
        /// if no implementation is explicitly assigned. You should only assign 
        /// each world an individual threading implementation instance if simulations 
        /// need to be run in parallel in multiple threads for the worlds.
        /// </summary>
        /// <returns>New instance of object allocated or NULL on failure</returns>
        public static ThreadingImplementation NewSelfThreaded()
        {
            var id = NativeMethods.dThreadingAllocateSelfThreadedImplementation();
            if (id.IsInvalid)
                return null;
            return new ThreadingImplementation { id = id };
        }

        /// <summary>
        /// Allocates built-in multi-threaded threading implementation object.
        ///
        /// Use ODE.dll compiled with "--with-builtin-threading-impl"
        /// </summary>
        /// <returns>New instance of object allocated or NULL on failure</returns>
        public static ThreadingImplementation NewMultiThreaded()
        {
            var id = NativeMethods.dThreadingAllocateMultiThreadedImplementation();
            if (id.IsInvalid)
                return null;
            return new ThreadingImplementation { id = id };
        }

        /// <summary>
        /// Retrieves the functions record of a built-in threading implementation.
        ///
        /// The implementation can be the one allocated by ODE (from @c dThreadingAllocateMultiThreadedImplementation). 
        /// Do not use this function with self-made custom implementations - 
        /// they should be bundled with their own set of functions.
        /// </summary>
        public WorldStepThreadingImplementation Functions
        {
            get
            {
                var handlePtr = NativeMethods.dThreadingImplementationGetFunctions(id);
                return new WorldStepThreadingImplementation { info = handlePtr };
            }
        }

        /// <summary>
        /// Requests a built-in implementation to release threads serving it.
        ///
        /// The function unblocks threads employed in implementation serving and lets them 
        /// return to from where they originate. It's the responsibility of external code 
        /// to make sure all the calls to ODE that might be dependent on given threading 
        /// implementation object had already returned before this call is made. If threading 
        /// implementation is still processing some posted calls while this function is 
        /// invoked the behavior is implementation dependent.
        ///
        /// This call is to be used to request the threads to be released before waiting 
        /// for them in host pool or before waiting for them to exit. Implementation object 
        /// must not be destroyed before it is known that all the serving threads have already 
        /// returned from it. If implementation needs to be reused after this function is called
        /// and all the threads have exited from it a call to @c dThreadingImplementationCleanupForRestart
        /// must be made to restore internal state of the object.
        ///
        /// If this function is called for self-threaded built-in threading implementation
        /// the call has no effect.
        /// </summary>
        public void ShutdownProcessing()
        {
            NativeMethods.dThreadingImplementationShutdownProcessing(id);
        }

        /// <summary>
        /// Destroy the implementation object.
        /// </summary>
        public void Dispose()
        {
            id.Close();
        }
    }

    /// <summary>
    /// Built-in thread pool object that can be used to serve multi-threaded threading implementations.
    /// </summary>
    public class ThreadingThreadPool : IDisposable
    {
        readonly dThreadingThreadPoolID id;

        /// <summary>
        /// Creates an instance of built-in thread pool object that can be used to serve multi-threaded threading implementations.
        ///
        /// The threads allocated inherit priority of caller thread. Their affinity is not
        /// explicitly adjusted and gets the value the system assigns by default. Threads 
        /// have their stack memory fully committed immediately on start. On POSIX platforms 
        /// threads are started with all the possible signals blocked. Threads execute 
        /// calls to @c dAllocateODEDataForThread with @p ode_data_allocate_flags 
        /// on initialization.
        ///
        /// On POSIX platforms this function must be called with signals masked 
        /// or other measures must be taken to prevent reception of signals by calling thread 
        /// for the duration of the call.
        /// </summary>
        /// <param name="threadCount">Number of threads to start in pool</param>
        /// <param name="allocateFlags">Flags to be passed to @c dAllocateODEDataForThread on behalf of each thread</param>
        public ThreadingThreadPool(uint threadCount, AllocateDataFlags allocateFlags)
        {
            id = NativeMethods.dThreadingAllocateThreadPool(threadCount, UIntPtr.Zero, allocateFlags, IntPtr.Zero);
        }

        internal dThreadingThreadPoolID Id
        {
            get { return id; }
        }

        /// <summary>
        /// Destroy the object.
        /// </summary>
        public void Dispose()
        {
            id.Close();
        }

        /// <summary>
        /// Commands an instance of built-in thread pool to serve a built-in multi-threaded 
        /// threading implementation.
        ///
        /// A pool can only serve one threading implementation at a time. 
        /// Call @c dThreadingImplementationShutdownProcessing to release pool threads 
        /// from implementation serving and make them idle. Pool threads must be released 
        /// from any implementations before pool is attempted to be deleted.
        ///
        /// This function waits for threads to register within implementation before returning.
        /// So, after the function call exits the implementation can be used immediately.
        /// </summary>
        /// <param name="threading">Implementation to be served</param>
        public void ServeMultiThreadedImplementation(ThreadingImplementation threading)
        {
            NativeMethods.dThreadingThreadPoolServeMultiThreadedImplementation(id, threading.Id);
        }
    }

    /// <summary>
    /// Represents the threading implementation used by world stepping functions.
    /// </summary>
    public class WorldStepThreadingImplementation
    {
        internal IntPtr info;
    }
}
