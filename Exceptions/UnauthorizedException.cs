using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace FlatsAPI.Exceptions
{
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException() : base("Access denied")
        {
        }

        public UnauthorizedException(string message) : base(message)
        {
        }

        public UnauthorizedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UnauthorizedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
