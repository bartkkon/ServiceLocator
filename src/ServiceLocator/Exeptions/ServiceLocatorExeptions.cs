using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ServiceLocator.Exeptions
{
    public class ServiceLocatorExeptions : Exception
    {
        public ServiceLocatorExeptions()
        {
        }

        public ServiceLocatorExeptions(string message) : base(message)
        {
        }

        public ServiceLocatorExeptions(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ServiceLocatorExeptions(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
