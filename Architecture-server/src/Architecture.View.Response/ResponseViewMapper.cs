using System;
using AutoMapper;
using Architecture.Model.Invoke;

namespace Architecture.View.Response
{
    public class ResponseViewMapper : IViewMapper
    {
        private readonly IMapper _mapper;

        public ResponseViewMapper(IMapper mapper)
        {
            _mapper = mapper;
        }

        public ServerResponse<TOut> InvokeResultToServerResponse<TIn, TOut>(InvokeResult<TIn> invokeResult)
        {
            var serverResponse = new ServerResponse<TOut>
            {
                Ok = invokeResult.IsSuccess,
                Message = invokeResult.ErrorMessage,
                //Errors = invokeResult.Errors,
                Code = invokeResult.Code,
                Result = _mapper.Map<TOut>(invokeResult.Result),
            };

            return serverResponse;
        }
    }
}
