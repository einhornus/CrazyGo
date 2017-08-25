
using System.Collections.Generic; using System;

using System.Text;

using BrainDuelsLib.web.exceptions;

namespace BrainDuelsLib.web.requests
{
    public class ErrorResponse : Response
    {
        private string errorCode;
        private string errorDetails;

        public ErrorResponse(string responseString)
        {
            string code = responseString.Substring(1, responseString.IndexOf('$') - 1);
            string details = responseString.Substring(responseString.IndexOf('$') + 1);
            this.errorCode = code;
            this.errorDetails = details;
        }

        public ErrorResponse(string errorCode, string details)
        {
            this.errorCode = errorCode;
            this.errorDetails = details;
        }

        public string GetErrorCode()
        {
            return errorCode;
        }

        public string GetDetails()
        {
            return errorDetails;
        }

        public override string ToString()
        {
            return errorCode + " : " + errorDetails;
        }

        public Exception ThrowException()
        {
            if(errorCode.Equals("SECURITY")){
                return new SecurityError();
            }

            if (errorCode.Equals("PARSE"))
            {
                return new WebException();
            }

            if (errorCode.Equals("UNEXPECTED_PARAMETER"))
            {
                return new WebException();
            }

            if (errorCode.Equals("MISSED_PARAMETER"))
            {
                return new WebException();
            }

            if (errorCode.Equals("UNEXPECTED_REQUEST_TYPE"))
            {
                return new WebException();
            }

            if (errorCode.Equals("LOGIN_EXISTS"))
            {
                return new LoginExists();
            }

            if (errorCode.Equals("WRONG_LOGIN_PASSWORD"))
            {
                return new WrongLoginPassword();
            }

            return new WebException();
        }
    }
}
