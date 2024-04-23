using System;
using System.Collections.Generic;

namespace HotPlay.Utilities
{
    public class RetryExceededException : AggregateException
    {
        public int RetryCount { get; private set; }

        public RetryExceededException(int retryCount, IEnumerable<Exception> innerExceptions) : base(innerExceptions)
        {
            RetryCount = retryCount;
        }
    }
}
