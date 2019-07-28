using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UdemyIdentityTokenBasedAuth.Domain.Responses
{
    public class BaseResponse<T> where T:class

    {
        public T Extra { get; set; }


        public bool Success { get; set; }
        public string Message { get; set; }


        public BaseResponse(T extra=null)
        {
            this.Extra = extra;
            this.Success = true;
        }
        public BaseResponse(string message)
        {
            this.Success = false;
            this.Message = message;
        }


    }
}
