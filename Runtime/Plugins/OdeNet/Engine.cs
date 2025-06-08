using Ode.Net.Native;
using System;
using System.Runtime.InteropServices;

namespace Ode.Net
{
    /// <summary>
    /// This class provides methods for ODE initialization and finalization.
    /// </summary>
    public static class Engine
    {
        static bool logMessages = true;
        static readonly dMessageFunction ErrorHandler = OnError;
        static readonly dMessageFunction MessageHandler = OnMessage;

        private static void OnError(int errnum, string msg, IntPtr ap)
        {
            throw new OdeException(msg);
        }

        private static void OnMessage(int errnum, string msg, IntPtr ap)
        {
            if (logMessages)
            {
                Console.Error.WriteLine(msg);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to log ODE messages to
        /// the standard error channel.
        /// </summary>
        public static bool LogMessages
        {
            get { return logMessages; }
            set { logMessages = value; }
        }

        /// <summary>
        /// Gets the specific ODE build configuration as a sequence of tokens.
        /// </summary>
        /// <returns>The ODE configuration string.</returns>
        public static string GetConfiguration()
        {
            var configurationPtr = NativeMethods.dGetConfiguration();
            return Marshal.PtrToStringAnsi(configurationPtr);
        }

        /// <summary>
        /// Checks for a specific token in the ODE configuration string.
        /// This function is case sensitive.
        /// </summary>
        /// <param name="token">The configuration token to check for.</param>
        /// <returns>
        /// <b>true</b> if the configuration token is present in the ODE
        /// configuration string; otherwise, <b>false</b>.
        /// </returns>
        public static bool CheckConfiguration(string token)
        {
            return NativeMethods.dCheckConfiguration(token) != 0;
        }

        /// <summary>
        /// Initializes the ODE library.
        /// </summary>
        public static void Init()
        {
            Init(InitFlags.None);
        }

        /// <summary>
        /// Initializes the ODE library.
        /// </summary>
        /// <param name="initFlags">Specifies ODE library initialization options.</param>
        public static void Init(InitFlags initFlags)
        {
            var result = NativeMethods.dInitODE2(initFlags);
            if (result == 0)
            {
                throw new InvalidOperationException("Failed to initialize ODE library.");
            }

            NativeMethods.dSetErrorHandler(ErrorHandler);
            NativeMethods.dSetMessageHandler(MessageHandler);
            NativeMethods.dSetDebugHandler(ErrorHandler);
        }

        /// <summary>
        /// Allocates thread local data to allow the current thread to call ODE.
        /// </summary>
        /// <param name="allocateFlags">Specifies thread allocation options.</param>
        public static void AllocateDataForThread(AllocateDataFlags allocateFlags)
        {
            var result = NativeMethods.dAllocateODEDataForThread(allocateFlags);
            if (result == 0)
            {
                throw new InsufficientMemoryException("The requested data could not be allocated.");
            }
        }

        /// <summary>
        /// Releases thread local data that was allocated for the current thread.
        /// </summary>
        public static void CleanupAllDataForThread()
        {
            NativeMethods.dCleanupODEAllDataForThread();
        }

        /// <summary>
        /// Releases all the resources allocated for library including all the
        /// thread local data that might be allocated for all the threads that
        /// were using ODE.
        /// </summary>
        public static void Close()
        {
            NativeMethods.dCloseODE();
        }
    }
}
