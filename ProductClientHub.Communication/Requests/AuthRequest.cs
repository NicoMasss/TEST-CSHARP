<<<<<<< Updated upstream
﻿namespace ProductClientHub.Communication.Requests
{
    public class AuthRequest
    {
        public string User =  string.Empty;

        public string Password = string.Empty;
    }
}
=======
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductClientHub.Communication.Requests
{
    public class AuthRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
>>>>>>> Stashed changes
