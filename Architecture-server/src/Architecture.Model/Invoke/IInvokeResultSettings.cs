using System;

namespace Architecture.Common.Invoke
{
    public interface IInvokeResultSettings : ICloneable
    {
        OperationLogSettings ValidationLogSettings  { get; }
        OperationLogSettings LogRequestLogSettings  { get; }
        InvokeFunctionLogSetttings InvokeFunctionLogSetttings { get; }
    }
}