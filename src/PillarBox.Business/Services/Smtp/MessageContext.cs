using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace PillarBox.Business.Services.Smtp
{
    /// <summary>
    /// Singleton within the scope of a single message, used to access information about the SMTP client
    /// </summary>
    public class MessageContext : IMessageContext
    {
        public IPEndPoint IPEndPoint { get; set; }

        public string User { get; set; } = "anonymous/[CLIENT_IP]";

        public string Password { get; set; }

        public string Hostname {
            get
            {
                try
                {
                    IPHostEntry entry = Dns.GetHostEntry(IPEndPoint.Address);
                    if (entry != null)
                    {
                        return entry.HostName;
                    }
                }
                catch (SocketException ex)
                {
                }
                return "unknown";
            }
        }
    }
}
