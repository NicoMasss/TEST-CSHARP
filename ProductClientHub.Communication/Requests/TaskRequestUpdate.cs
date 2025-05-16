using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductClientHub.Communication.Requests
{
    public class TaskRequestUpdate
    {
        public Guid Id { get; set; } 
        public string NewTitle { get; set; } = string.Empty;
        public string NewDescription { get; set; } = string.Empty;
        public string NewStatus { get; set; } = string.Empty;
    }
}
