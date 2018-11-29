using System;

namespace Architecture.Model.Invoke
{
    public class InvokeResult<T>
    {
        public T          Result       { get; private set; }
        public bool       IsSuccess    { get; private set; }
        public ResultCode Code         { get; private set; }
        public string     ErrorMessage { get; private set; }
        public Exception  Exception    { get; private set; }

        public static InvokeResult<T> Ok(T result) => new InvokeResult<T>
        {
            IsSuccess    = true,
            Code         = ResultCode.Ok,
            Result       = result,
            ErrorMessage = null
        };

        public static InvokeResult<T> Fail
            (ResultCode resultCode) =>
            new InvokeResult<T>
            {
                IsSuccess    = false,
                Code         = resultCode,
                Result       = default(T),
                Exception    = null,
                ErrorMessage = null
            };

        public static InvokeResult<T> Fail
            (ResultCode resultCode, string errorMessage) =>
            new InvokeResult<T>
            {
                IsSuccess    = false,
                Code         = resultCode,
                Result       = default(T),
                ErrorMessage = errorMessage
            };

        public static InvokeResult<T> Fatal
            (Exception ex) =>
            new InvokeResult<T>
            {
                IsSuccess    = false,
                Code         = ResultCode.InternalServerError,
                Result       = default(T),
                Exception    = ex,
                ErrorMessage = null
            };

        public static InvokeResult<T> Ok() => Ok(default(T));
    }
}