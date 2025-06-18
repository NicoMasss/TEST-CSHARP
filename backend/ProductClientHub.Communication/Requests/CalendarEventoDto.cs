using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductClientHub.Communication.Requests
{
    public class CalendarEventDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime End { get; set; }
    }
}
