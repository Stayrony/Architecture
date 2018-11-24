using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Architecture.Common.Invoke
{
    public class BaseInvokeHandler<K> : IInvokeHandler<K>
    {
        private readonly IInvokeResultSettings _invokeResultSettings;
        private readonly ILogger<K>            _logger;

        public BaseInvokeHandler(IInvokeResultSettings invokeResultSettings, ILogger<K> logger)
        {
            _invokeResultSettings = invokeResultSettings;
            _logger               = logger;
        }

        #region Invoke

        public InvokeResult<T> Invoke<T>(Func<InvokeResult<T>> func, object request)
        {
            return Invoke(func, request, _ => _invokeResultSettings);
        }

        public InvokeResult<T> Invoke<T>(Func<InvokeResult<T>> func, object request,
                                         Func<IInvokeResultSettings, IInvokeResultSettings> settingsFunc)
        {
            var settings = settingsFunc((IInvokeResultSettings) _invokeResultSettings.Clone());

            LogRequest(request, settings.LogRequestLogSettings);

            var validationRules = new List<ValidationResult>();
            if (!Validator.TryValidateObject(request, new ValidationContext(request), validationRules, true))
            {
                LogValidation(validationRules, settings.ValidationLogSettings);
                return InvokeResult<T>.Fail(ResultCode.ValidationError,
                    string.Join(" ",
                        validationRules.Select(x => $"\"{x.MemberNames.FirstOrDefault()}\":\"{x.ErrorMessage}\"")));
            }

            InvokeResult<T> invokeResult = null;
            try
            {
                invokeResult = func();
            }
            catch (Exception ex)
            {
                LogException(ex);
            }

            LogInvoke(invokeResult, settings.InvokeFunctionLogSetttings);

            return invokeResult;
        }

        public InvokeResult<T> Invoke<T>(Func<InvokeResult<T>> func)
        {
            return Invoke(func, _ => _invokeResultSettings);
        }

        public InvokeResult<T> Invoke<T>(Func<InvokeResult<T>> func,
                                         Func<IInvokeResultSettings, IInvokeResultSettings> settingsFunc)
        {
            var             settings     = settingsFunc((IInvokeResultSettings) _invokeResultSettings.Clone());
            InvokeResult<T> invokeResult = null;
            try
            {
                invokeResult = func();
            }
            catch (Exception ex)
            {
                LogException(ex);
            }

            LogInvoke(invokeResult, settings.InvokeFunctionLogSetttings);

            return invokeResult;
        }

        #endregion

        #region AsyncInvoke

        public Task<InvokeResult<T>> InvokeAsync<T>(Func<Task<InvokeResult<T>>> func)
        {
            return InvokeAsync(func, _ => _invokeResultSettings);
        }

        public async Task<InvokeResult<T>> InvokeAsync<T>(Func<Task<InvokeResult<T>>> func,
                                                          Func<IInvokeResultSettings, IInvokeResultSettings>
                                                              settingsFunc)
        {
            var             settings     = settingsFunc((IInvokeResultSettings) _invokeResultSettings.Clone());
            InvokeResult<T> invokeResult = null;
            try
            {
                invokeResult = await func();
            }
            catch (Exception ex)
            {
                LogException(ex);
            }

            LogInvoke(invokeResult, settings.InvokeFunctionLogSetttings);

            return invokeResult;
        }

        public Task<InvokeResult<T>> InvokeAsync<T>(Func<Task<InvokeResult<T>>> func, object request)
        {
            return InvokeAsync(func, request, _ => _invokeResultSettings);
        }

        public async Task<InvokeResult<T>> InvokeAsync<T>(Func<Task<InvokeResult<T>>> func, object request,
                                                          Func<IInvokeResultSettings, IInvokeResultSettings>
                                                              settingsFunc)
        {
            var settings = settingsFunc((IInvokeResultSettings) _invokeResultSettings.Clone());

            LogRequest(request, settings.LogRequestLogSettings);

            var validationRules = new List<ValidationResult>();
            if (!Validator.TryValidateObject(request, new ValidationContext(request), validationRules, true))
            {
                LogValidation(validationRules, settings.ValidationLogSettings);
                return InvokeResult<T>.Fail(ResultCode.ValidationError,
                    string.Join(" ",
                        validationRules.Select(x => $"\"{x.MemberNames.FirstOrDefault()}\":\"{x.ErrorMessage}\"")));
            }

            InvokeResult<T> invokeResult = null;
            try
            {
                invokeResult = await func();
            }
            catch (Exception ex)
            {
                LogException(ex);
            }

            LogInvoke(invokeResult, settings.InvokeFunctionLogSetttings);

            return invokeResult;
        }

        #endregion

        public void SetInvokeResultSettings(Action<IInvokeResultSettings> settingsAction)
        {
            settingsAction(_invokeResultSettings);
        }

        public IInvokeResultSettings InvokeResultSettings => _invokeResultSettings;

        protected virtual void LogRequest(object request, OperationLogSettings operationLogSettings)
        {
            if (operationLogSettings.IsLogging)
            {
                _logger.Log(operationLogSettings.LogLevel, "{request}", request);
            }
        }

        protected virtual void LogValidation(IEnumerable<ValidationResult> validationResults,
                                             OperationLogSettings operationLogSettings)
        {
            if (operationLogSettings.IsLogging)
            {
                _logger.Log(operationLogSettings.LogLevel, "Validation failed");
            }
        }

        protected virtual void LogException(Exception ex)
        {
            _logger.LogCritical("Exception occured type: {Type} message: {Message} stackTrace: {StackTrace}",
                ex.GetType(), ex.Message, ex.StackTrace);
        }

        protected virtual void LogInvoke<T>(InvokeResult<T> result, InvokeFunctionLogSetttings operationLogSettings)
        {
            if(result == null) return;
            
            switch (operationLogSettings.LogInvokeResult)
            {
                case LogInvokeResult.Always:
                    _logger.Log(operationLogSettings.LogLevel, "{@result}", result);
                    break;
                case LogInvokeResult.IsFail:
                    if (!result.IsSuccess)
                        _logger.Log(operationLogSettings.LogLevel, "{@result}", result);

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}