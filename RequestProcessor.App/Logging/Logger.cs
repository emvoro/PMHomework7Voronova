using System;
using System.Diagnostics;

namespace RequestProcessor.App.Logging
{
    internal class Logger : ILogger
    {
        public void Log(string message) 
        {
            if(IsValid(message))
                Debug.WriteLine(message);
        }

        public void Log(Exception exception, string message)
        {
            if (exception != null)
                Debug.WriteLine(exception.Message);
            if (IsValid(message))
                Debug.WriteLine(message);
        }

        public bool IsValid(string message)
        {
            return !string.IsNullOrEmpty(message);
        }
    }
}
