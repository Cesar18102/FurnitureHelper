using System;
using System.Net;
using System.Net.Http;

using Newtonsoft.Json;

using Models;
using ServicesContract.Exceptions;

namespace BackEnd.Controllers
{
    public class ResultWrapper<TResult>
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

    public static class ControllerExtensions
    {
        public static HttpResponseMessage ExecuteProtectedAndWrapResult<TResult>(this HttpRequestMessage request, Func<TResult> executor) 
            where TResult : IModel
        {
            try
            {
                TResult result = executor();
                ResultWrapper<TResult> wrappedResult = new ResultWrapper<TResult>(result);
                return request.CreateResponse(HttpStatusCode.OK, wrappedResult);
            }
            catch(UnauthorizedException ex)
            {
                return request.CreateResponse(HttpStatusCode.Unauthorized, new ResultWrapper<TResult>(ex));
            }
            catch(ConftictException ex)
            {
                return request.CreateResponse(HttpStatusCode.Conflict, new ResultWrapper<TResult>(ex));
            }
            catch(NotFoundException ex)
            {
                return request.CreateResponse(HttpStatusCode.NotFound, new ResultWrapper<TResult>(ex));
            }
            catch(ValidationException<TResult> ex)
            {
                return request.CreateResponse(HttpStatusCode.BadRequest, new ResultWrapper<TResult>(ex));
            }
        }
    }
}