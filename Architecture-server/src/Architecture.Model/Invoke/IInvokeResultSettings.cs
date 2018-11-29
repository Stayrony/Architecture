using System;

namespace Architecture.Model.Invoke
{
    public interface IInvokeResultSettings : ICloneable
    {
        OperationLogSettings ValidationLogSettings  { get; }
        OperationLogSettings LogRequestLogSettings  { get; }
        InvokeFunctionLogSetttings InvokeFunctionLogSetttings { get; }
    }
}