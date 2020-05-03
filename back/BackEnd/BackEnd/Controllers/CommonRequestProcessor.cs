using System;
using System.Net;
using System.Net.Http;

using Newtonsoft.Json;

using DataTypes;
using DataTypes.Exceptions;

namespace BackEnd.Controllers
{
    public class CommonRequestProcessor
    {
        private class ResultWrapper<TResult>
        {
            public string error { get; private set; }
            public string data { get; private set; }

            public ResultWrapper(TResult result)
            {
                error = null;
                data = JsonConvert.SerializeObject(result);
            }

            public ResultWrapper(Exception exception)
            {
                error = JsonConvert.SerializeObject(exception);
                data = null;
            }
        }

        public static HttpResponseMessage ExecuteCommonRequestProccessing<TResult, TDto>(
            Func<TResult> executor, HttpRequestMessage request) where TDto : IDto
        {
            try
            {
                TResult result = executor();
                ResultWrapper<TResult> wrappedResult = new ResultWrapper<TResult>(result);
                return request.CreateResponse(HttpStatusCode.OK, wrappedResult);
            }
            catch(ConftictException ex)
            {
                return request.CreateResponse(HttpStatusCode.Conflict, new ResultWrapper<TResult>(ex));
            }
            catch(NotFoundException ex)
            {
                return request.CreateResponse(HttpStatusCode.NotFound, new ResultWrapper<TResult>(ex));
            }
            catch(InvalidDataException<TDto> ex)
            {
                return request.CreateResponse(HttpStatusCode.BadRequest, new ResultWrapper<TResult>(ex));
            }
        }
    }
}