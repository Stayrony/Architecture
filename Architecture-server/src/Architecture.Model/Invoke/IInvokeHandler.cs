using System;
using System.Threading.Tasks;

namespace Architecture.Model.Invoke
{
    public interface IInvokeHandler<K>
    {
        IInvokeResultSettings InvokeResultSettings { get; }
        void SetInvokeResultSettings(Action<IInvokeResultSettings> settingsAction);
        
        InvokeResult<T> Invoke<T>(Func<InvokeResult<T>> func);
        InvokeResult<T> Invoke<T>(Func<InvokeResult<T>> func, Func<IInvokeResultSettings, IInvokeResultSettings> settingsFunc);
        
        InvokeResult<T> Invoke<T>(Func<InvokeResult<T>> func, object request);
        InvokeResult<T> Invoke<T>(Func<InvokeResult<T>> func, object request, Func<IInvokeResultSettings, IInvokeResultSettings> settingsFunc);
        
        Task<InvokeResult<T>> InvokeAsync<T>(Func<Task<InvokeResult<T>>> func);
        Task<InvokeResult<T>> InvokeAsync<T>(Func<Task<InvokeResult<T>>> func, Func<IInvokeResultSettings, IInvokeResultSettings> settingsFunc);
        
        Task<InvokeResult<T>> InvokeAsync<T>(Func<Task<InvokeResult<T>>> func, object request);
        Task<InvokeResult<T>> InvokeAsync<T>(Func<Task<InvokeResult<T>>> func, object request, Func<IInvokeResultSettings, IInvokeResultSettings> settingsFunc);
    }
}