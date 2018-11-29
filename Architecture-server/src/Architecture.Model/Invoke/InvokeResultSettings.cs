using Microsoft.Extensions.Logging;

namespace Architecture.Model.Invoke
{
    public class InvokeResultSettings : IInvokeResultSettings
    {
        public InvokeResultSettings()
        {
            ValidationLogSettings = new OperationLogSettings
            {
                IsLogging = true,
                LogLevel  = LogLevel.Debug
            };
            
            LogRequestLogSettings = new OperationLogSettings
            {
                IsLogging = true,
                LogLevel  = LogLevel.Debug
            };
            
            InvokeFunctionLogSetttings = new InvokeFunctionLogSetttings
            {
                LogInvokeResult = LogInvokeResult.IsFail,
                LogLevel  = LogLevel.Debug
            };
        }                
        
        public OperationLogSettings ValidationLogSettings  { get; set; }
        public OperationLogSettings LogRequestLogSettings  { get; set; }
        public InvokeFunctionLogSetttings InvokeFunctionLogSetttings { get; set; }
        
        public object Clone() => MemberwiseClone();
    }
}