/*
   File: BaseResponseService.cs
   Description: This file contains the implementation of the BaseResponseService class, 
   which provides methods for generating various types of responses, including  success responses, validation responses,
   and error responses. This use for other services class responses.
   Author: Prabhash D.V.S.
   Creation Date: 2023/10/03
   Last Modified Date: 2023/10/05
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketingSystem.Model;

namespace TicketingSystem.Repository
{
    public class BaseResponseService
    {
        /// <summary>
        /// Create a success response with a default "Success" message.
        /// </summary>
        public BaseResponse GetSuccessResponse()
        {
            return new BaseResponse() { Success = true, Message = "Success" };
        }

        /// <summary>
        /// Create a success response with a custom data object and a default "Success" message.
        /// </summary>
        /// <param name="data">The data to include in the response.</param>
        public BaseResponse GetSuccessResponse(object data)
        {
            return new BaseResponse() { Success = true, Message = "Success", Data = data };
        }

        /// <summary>
        /// Create a validation response with a custom error message.
        /// </summary>
        /// <param name="message">The validation error message.</param>
        public BaseResponse GetValidatationResponse(string message)
        {
            return new BaseResponse() { Success = false, Message = message };
        }

        /// <summary>
        /// Create an error response based on an exception with its message as the error message.
        /// </summary>
        /// <param name="ex">The exception that triggered the error.</param>
        public BaseResponse GetErrorResponse(Exception ex)
        {
            return new BaseResponse() { Success = false, Message = ex.Message };
        }
    }
}
