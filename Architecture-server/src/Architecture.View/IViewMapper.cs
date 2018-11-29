using System;
using Architecture.Model.Invoke;

namespace Architecture.View
{
    public interface IViewMapper
    {
        ServerResponse<TOut> InvokeResultToServerResponse<TIn, TOut>(InvokeResult<TIn> invokeResult);
    }
}
