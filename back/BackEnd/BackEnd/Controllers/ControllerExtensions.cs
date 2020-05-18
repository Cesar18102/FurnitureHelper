using System;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Collections.Generic;
using System.Web.Http.ModelBinding;

using Models;
using ServicesContract.Dto;
using ServicesContract.Exceptions;

namespace BackEnd.Controllers
{
    public class ResultWrapper<TResult> where TResult : class
    {
        public Exception error { get; private set; }
        public TResult data { get; private set; }

        public ResultWrapper(TResult result)
        {
            error = null;
            data = result;
        }

        public ResultWrapper(Exception exception)
        {
            error = exception;
            data = null;
        }
    }

    public static class ControllerExtensions
    {
        private static string GetStringWithoutIndexing(string str)
        {
            int firstIndexIdSquareBracket = str.IndexOf('[');

            if (firstIndexIdSquareBracket == -1)
                return str;

            return str.Substring(0, firstIndexIdSquareBracket) + str.Substring(str.IndexOf(']') + 1);
        }

        private static Type GetNextType(Type prevType, string fieldName)
        {
            string fieldNameWithoutIndex = GetStringWithoutIndexing(fieldName);
            PropertyInfo info = prevType.GetProperty(fieldNameWithoutIndex);

            if (info == null)
                return null;

            Type type = info.PropertyType;

            if (fieldNameWithoutIndex != fieldName)
                return type.GetGenericArguments()[0];

            return type;
        }

        private static Type GetErrorType(Type prevType, string errorFieldSequence)
        {
            if (string.IsNullOrEmpty(errorFieldSequence) || prevType == null)
                return prevType;

            int firstIndexOfDot = errorFieldSequence.IndexOf('.');

            if (firstIndexOfDot == -1)
                return GetNextType(prevType, errorFieldSequence);

            string nextFieldName = errorFieldSequence.Substring(0, firstIndexOfDot);
            string subSequence = errorFieldSequence.Substring(firstIndexOfDot + 1);

            Type currentType = GetNextType(prevType, nextFieldName);
            return GetErrorType(currentType, subSequence);
        }

        private static void ValidateModelState<TSource>(ModelStateDictionary modelState) where TSource : IDto
        {
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

                    if (errorType == null || errorType.GetProperty(errorField) == null)
                    {
                        shouldBeThrown = true;
                        continue;
                    }

                    ValidationFailInfo failInfo = ValidationFailInfo.CreateValidationFailInfo(
                        errorType, errorField, error.ErrorMessage //"Invalid data for " + errorField + " provided" 
                    );

                    ex.ValidationFailInfos.Add(failInfo);
                }

            if (ex.ValidationFailInfos.Count > 0 || shouldBeThrown)
                throw ex;
        }

        private static HttpResponseMessage ExecuteProtectedAndWrapResultCommon<TResult>(this HttpRequestMessage request, Func<TResult> executor) where TResult : class
        {
            try
            {
                TResult result = executor();
                ResultWrapper<TResult> wrappedResult = new ResultWrapper<TResult>(result);
                return request.CreateResponse(HttpStatusCode.OK, wrappedResult);
            }
            catch(UnauthorizedException ex) { return request.CreateResponse(HttpStatusCode.Unauthorized, new ResultWrapper<TResult>(ex)); }
            catch (ForbiddenException ex) { return request.CreateResponse(HttpStatusCode.Forbidden, new ResultWrapper<TResult>(ex)); }
            catch (ConflictException ex) { return request.CreateResponse(HttpStatusCode.Conflict, new ResultWrapper<TResult>(ex)); }
            catch(NotFoundException ex) { return request.CreateResponse(HttpStatusCode.NotFound, new ResultWrapper<TResult>(ex)); }
            catch(ValidationException ex) { return request.CreateResponse(HttpStatusCode.BadRequest, new ResultWrapper<TResult>(ex)); }
            catch(ArgumentException ex) { return request.CreateResponse(HttpStatusCode.BadRequest, new ResultWrapper<TResult>(ex)); }
        }

        private static HttpResponseMessage ExecuteProtectedAndWrapResultWithArgument<TSource, TResult>
            (this HttpRequestMessage request, Func<TSource, TResult> executor, ModelStateDictionary modelState, TSource arg) where TSource : IDto
                                                                                                                             where TResult : class
        {
            return ExecuteProtectedAndWrapResultCommon<TResult>(
                request, () => {
                    if (arg == null)
                        throw new ValidationException();

                    ValidateModelState<TSource>(modelState);

                    return executor(arg);
                }
            );
        }

        public static HttpResponseMessage ExecuteProtectedAndWrapResult<TSource, TResult>
            (this HttpRequestMessage request, Func<TSource, TResult> executor, 
             ModelStateDictionary modelState, TSource arg) where TSource : IDto
                                                           where TResult : class, IModel
        {
            return ExecuteProtectedAndWrapResultWithArgument<TSource, TResult>(request, executor, modelState, arg);
        }

        public static HttpResponseMessage ExecuteProtectedAndWrapResult<TSource, TResult>
            (this HttpRequestMessage request, Func<TSource, IEnumerable<TResult>> executor,
             ModelStateDictionary modelState, TSource arg) where TSource : IDto
                                                           where TResult : IModel
        {
            return ExecuteProtectedAndWrapResultWithArgument<TSource, IEnumerable<TResult>>(request, executor, modelState, arg);
        }

        public static HttpResponseMessage ExecuteProtectedAndWrapResult<TResult>
            (this HttpRequestMessage request, Func<TResult> executor) where TResult : class, IModel
        {
            return ExecuteProtectedAndWrapResultCommon<TResult>(request, executor);
        }

        public static HttpResponseMessage ExecuteProtectedAndWrapResult<TResult>
            (this HttpRequestMessage request, Func<IEnumerable<TResult>> executor) where TResult : IModel
        {
            return ExecuteProtectedAndWrapResultCommon<IEnumerable<TResult>>(request, executor);
        }
    }
}