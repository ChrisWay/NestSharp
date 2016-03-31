using NestSharp.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NestSharp
{
    public class NestException : Exception
    {
        public NestException() {
        }

        public NestException(string error, string errorDescription)
            : base(errorDescription) {
            Error = error;
        }

        public NestException(ErrorResponse errorResponse) 
            : this(errorResponse.Error, errorResponse.ErrorDescription){
        }
     
        public string Error { get; protected set; }
    }
}
