using System;
using System.Collections.Generic;
using System.Text;

namespace PillarBox.Data.Config
{
    public class ServerDefaults
    {
        public string SmtpPassword { get; set; }

        public string SmtpPorts { get; set; }

        public bool EnableSecureSmtp { get; set; }

        public int HttpPort { get; set; }
    }
}
