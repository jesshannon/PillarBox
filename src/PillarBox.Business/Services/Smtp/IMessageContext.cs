using System.Net;

namespace PillarBox.Business.Services.Smtp
{
    public interface IMessageContext
    {
        IPEndPoint IPEndPoint { get; set; }
        string User { get; set; }
        string Password { get; set; }
        string Hostname { get;  }
    }
}
