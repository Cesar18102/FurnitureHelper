using System;
using System.IO;
using System.Net;
using System.Text;
using System.Linq;
using System.Threading;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;

using Newtonsoft.Json;

using Models.Exceptions;

namespace ServerAccess
{
    internal class Server : IServer
    {
        private const string JSON_CONTENT_TYPE = "application/json";

        public async Task<TOut> SendGet<TOut>(string url) where TOut : class =>
            await SendGet<TOut>(url, null);

        public async Task<TOut> SendGet<TOut>(string url, IDictionary<string, string> parameters) where TOut : class
        {
            string paramsString = parameters == null ? "" : string.Join("&", parameters.Select(param => param.Key + "=" + param.Value));
            string uri = url + (paramsString == "" ? "" : ("?" + paramsString));
            return await Send<TOut>(uri, WebRequestMethods.Http.Get, null);
        }

        public async Task<TOut> SendPost<TIn, TOut>(string url, TIn body) where TIn : class
                                                                          where TOut : class
        {
            string bodyString = JsonConvert.SerializeObject(body);
            return await Send<TOut>(url, WebRequestMethods.Http.Post, bodyString);
        }

        private async Task<TOut> Send<TOut>(string url, string method, string body) where TOut : class
        {
            HttpWebRequest request = HttpWebRequest.CreateHttp(url);
            request.Method = method;
            request.KeepAlive = false;
            request.Timeout = Timeout.Infinite;
            request.ContentType = JSON_CONTENT_TYPE;
            request.ProtocolVersion = HttpVersion.Version10;

            if (body != "" && body != null)
            {
                byte[] bytes = Encoding.UTF8.GetBytes(body);
                using (Stream strw = await request.GetRequestStreamAsync())
                    strw.Write(bytes, 0, bytes.Length);
            }

            try
            {
                using (WebResponse response = await request.GetResponseAsync())
                using (StreamReader str = new StreamReader(response.GetResponseStream()))
                {
                    string responseJson = await str.ReadToEndAsync();
                    return JsonConvert.DeserializeObject<ResponseWrapper<TOut>>(responseJson).Data;
                }
            }
            catch (WebException ex)
            {
                try
                {
                    using (StreamReader str = new StreamReader(ex.Response.GetResponseStream()))
                    {
                        CustomException cex = FilterExceptions<TOut>(str.ReadToEnd());

                        if (cex != null)
                            throw cex;

                        throw ex;
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(
                        "\n\n*******SERVER ERROR*******\n\n" + 
                        JsonConvert.SerializeObject(e) + 
                        "\n\n*****************\n\n"
                    );

                    throw;
                }
            }
        }

        private class ResponseWrapper<TOut> where TOut : class
        {
            [JsonProperty("data")]
            public TOut Data { get; set; }

            [JsonProperty("error")]
            public object Error { get; set; }
        }

        private CustomException FilterExceptions<TOut>(string responseJson) where TOut : class
        {
            ResponseWrapper<TOut> response = JsonConvert.DeserializeObject<ResponseWrapper<TOut>>(responseJson);
            string errorJson = JsonConvert.SerializeObject(response.Error);

            return FilterException<ConflictException>(errorJson) ?? FilterException<ForbiddenException>(errorJson) ?? 
                   FilterException<NotFoundException>(errorJson) ?? FilterException<ValidationException>(errorJson) ??
                   FilterException<CustomException>(errorJson);
        }

        private CustomException FilterException<TErr>(string errorJson) where TErr : CustomException
        {
            try { return JsonConvert.DeserializeObject<TErr>(errorJson); }
            catch(Exception e) { return null; }
        }
    }
}
