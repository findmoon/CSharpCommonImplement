using System.Globalization;

namespace WebAPI_CURD.Helpers
{

    // custom exception class for throwing application specific exceptions (e.g. for validation) 
    // that can be caught and handled within the application
    public class AppException : Exception
    {
        public AppException() : base() { }

        public AppException(string message) : base(message) { }

        public AppException(string formatMsg, params object[] args)
            : base(String.Format(CultureInfo.CurrentCulture,
                formatMsg, args))
        {
        }
    }
}
