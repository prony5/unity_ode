using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Ode.Net
{
    /// <summary>
    /// The exception that is thrown when errors occur in the ODE framework.
    /// </summary>
    [Serializable]
    public class OdeException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OdeException"/> class.
        /// </summary>
        public OdeException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OdeException"/> class
        /// with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public OdeException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OdeException"/> class with a
        /// specified error message and a reference to the inner exception that is the
        /// cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception. If the <paramref name="innerException"/>
        /// parameter is not <b>null</b>, the current exception is raised in a <b>catch</b> block
        /// that handles the inner exception.
        /// </param>
        public OdeException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OdeException"/> class from
        /// serialization data.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected OdeException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
