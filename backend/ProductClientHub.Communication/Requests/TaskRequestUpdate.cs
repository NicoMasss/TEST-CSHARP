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
        public string? NewTitle { get; set; }
        public string? NewDescription { get; set; }
        public string? NewStatus { get; set; }
        public DateTime? NewDueDate { get; set; }
        public Guid? NewAssignedTo { get; set; }
        public Guid? NewAssignedToClient { get; set; }
    }

}
