using Microsoft.Extensions.Logging;

namespace Architecture.Model.Invoke
{
    public class OperationLogSettings
    {
        public bool     IsLogging { get; set; }
        public LogLevel LogLevel  { get; set; }
    }
}