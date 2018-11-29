using System;
using System.Collections.Generic;
using Architecture.Model.Errors;
using Architecture.Model.Invoke;

namespace Architecture.View
{
    public class ServerResponse<T>
    {
        public T Result { get; set; }
        public bool Ok { get; set; }
        public string Message { get; set; }
        public IEnumerable<Error> Errors { get; set; }
        public ResultCode Code { get; set; }
    }
}
