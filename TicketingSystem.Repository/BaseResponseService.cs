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
        public BaseResponse GetSuccessResponse()
        {
            return new BaseResponse() { Success = true, Message = "Success", };
        }
        public BaseResponse GetSuccessResponse(object data)
        {
            return new BaseResponse() { Success = true, Message = "Success", Data = data };
        }
        public BaseResponse GetValidatationResponse(string message)
        {
            return new BaseResponse() { Success = false, Message = message };
        }
        public BaseResponse GetErrorResponse(Exception ex)
        {
            return new BaseResponse() { Success = false, Message = ex.Message };
        }
    }
}
