using System;
using System.Collections.Generic;
using System.Text;
using Common.Shared.Enums;

namespace Common.Shared.Response
{
    public class Response<T>
    {
        public T? Data { get; set; }
        public string Message { get; set; } = string.Empty;
        public StatusCodeType StatusCode { get; set; }
    }
}
