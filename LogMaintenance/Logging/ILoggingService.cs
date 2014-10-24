using System;

namespace LogMaintenance.Logging
{
    public interface ILoggingService
    {
        void HandleException(Exception ex);
        void HandleMessage(string message);
    }
}