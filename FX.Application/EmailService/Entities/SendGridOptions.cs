﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FX.Application.EmailService.Entities
{
    public class SendGridKey
    {
        public string ApiKey { get; set; }
    }

    public class SendGridFrom
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string DisplayName { get; set; }
    }
}
