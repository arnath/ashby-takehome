using System;

namespace Ashby.Models
{
    public class HttpResponse
    {
        public HttpResponse(int statusCode, string message)
        {
            this.StatusCode = statusCode;
            this.Message = message;
        }

        public int StatusCode { get; }

        public string Message { get; }
    }
}

