using System;
using System.Runtime.Serialization;

namespace FlatsAPI.Exceptions;

public class ForbiddenException : Exception
{
    public ForbiddenException() : base("Access denied")
    {
    }

    public ForbiddenException(string message) : base(message)
    {
    }

    public ForbiddenException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected ForbiddenException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
