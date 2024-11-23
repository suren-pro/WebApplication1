using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Business.Exceptions
{
    public enum ErrorStatusCode
    {
        Success =000,
        FailedToAdd= 002,
        FailedToDelete=004,
        FailedToUpdate = 005,
        UserNotExist = 006,
        PostNotExist = 007,
        FailedToGetData =008,
        UsernameAlreadyExists = 009
    }
    public class BusinessExceptionHandler:Exception
    {

        public ErrorStatusCode StatusCode { get; set; }
        public BusinessExceptionHandler(ErrorStatusCode statusCode,string message):base(message)
        {
            this.StatusCode = statusCode;
        }
    }
}
