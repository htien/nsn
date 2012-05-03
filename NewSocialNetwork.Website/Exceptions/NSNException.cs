using System;

namespace NewSocialNetwork.Website.Exceptions
{
    [Serializable]
    public class NSNException : Exception
    {
        public NSNException() { }
        public NSNException(string message) : base(message) { }
        public NSNException(string message, Exception inner) : base(message, inner) { }
        protected NSNException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}