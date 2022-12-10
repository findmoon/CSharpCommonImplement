using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System
{
    public static class ExceptionDealExten
    {
        public static string LastMessage(this Exception exception)
        {
            var lastException = exception;
            var innerExc = exception.InnerException;
            while (innerExc != null)
            {
                lastException = innerExc;
                innerExc = innerExc.InnerException;
            }
            
            return lastException.Message;
        }
    }
}
