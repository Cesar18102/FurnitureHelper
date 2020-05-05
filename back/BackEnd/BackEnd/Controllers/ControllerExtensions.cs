using System;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Collections.Generic;
using System.Web.Http.ModelBinding;

using Newtonsoft.Json;

using Models;
using ServicesContract.Dto;
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
        private static Type GetErrorType(Type prevType, string errorFieldSequence)
        {
            if (string.IsNullOrEmpty(errorFieldSequence))
                return prevType;

            int firstIndexOfDot = errorFieldSequence.IndexOf('.');

            if (firstIndexOfDot == -1)
            {
                PropertyInfo info = prevType.GetProperty(errorFieldSequence);
                return info.PropertyType;
            }

            string nextFieldName = errorFieldSequence.Substring(0, firstIndexOfDot);
            string subSequence = errorFieldSequence.Substring(firstIndexOfDot + 1);
            Type currentType = prevType.GetProperty(nextFieldName).PropertyType;

            return GetErrorType(currentType, subSequence);
        }

        public static HttpResponseMessage ExecuteProtectedAndWrapResult<TSource, TResult>(
            this HttpRequestMessage request, Func<TSource, TResult> executor,
            ModelStateDictionary modelState, TSource arg) where TSource : IDto
                                                          where TResult : IModel
        {
            try
            {
                if (arg == null)
                    throw new ValidationException();

                ValidationException ex = new ValidationException();
                bool shouldBeThrown = false;

                foreach (KeyValuePair<string, ModelState> fieldState in modelState)
                    foreach (ModelError error in fieldState.Value.Errors)
                    {
                        int firstIndexOfDot = fieldState.Key.IndexOf('.');

                        if (firstIndexOfDot == -1)
                        {
                            shouldBeThrown = true;
                            continue;
                        }

                        string errorFieldSequence = fieldState.Key.Substring(firstIndexOfDot + 1);
                        int lastIndexOfDot = errorFieldSequence.LastIndexOf('.');

                        Type errorType = typeof(TSource);
                        string errorField = errorFieldSequence;

                        if (lastIndexOfDot != -1)
                        {
                            errorField = errorFieldSequence.Substring(lastIndexOfDot + 1);
                            errorType = GetErrorType(typeof(TSource), errorFieldSequence.Substring(0, lastIndexOfDot));                            
                        }

                        ValidationFailInfo failInfo = ValidationFailInfo.CreateValidationFailInfo(
                            errorType, errorField, error.ErrorMessage //"Invalid data for " + errorField + " provided" 
                        );

                        ex.ValidationFailInfos.Add(failInfo);
                    }

                if (ex.ValidationFailInfos.Count > 0 || shouldBeThrown)
                    throw ex;

                arg.Validate();

                TResult result = executor(arg);
                ResultWrapper<TResult> wrappedResult = new ResultWrapper<TResult>(result);
                return request.CreateResponse(HttpStatusCode.OK, wrappedResult);
            }
            catch(UnauthorizedException ex) { return request.CreateResponse(HttpStatusCode.Unauthorized, new ResultWrapper<TResult>(ex)); }
            catch (ForbiddenException ex) { return request.CreateResponse(HttpStatusCode.Forbidden, new ResultWrapper<TResult>(ex)); }
            catch (ConflictException ex) { return request.CreateResponse(HttpStatusCode.Conflict, new ResultWrapper<TResult>(ex)); }
            catch(NotFoundException ex) { return request.CreateResponse(HttpStatusCode.NotFound, new ResultWrapper<TResult>(ex)); }
            catch(ValidationException ex) { return request.CreateResponse(HttpStatusCode.BadRequest, new ResultWrapper<TResult>(ex)); }
        }
    }
}