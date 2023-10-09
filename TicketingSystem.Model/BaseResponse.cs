using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketingSystem.Model
{
    public class BaseResponse
    {
        public bool Success { get; set; }
        public object Data { get; set; }
        public string Message { get; set; }
    }
}
