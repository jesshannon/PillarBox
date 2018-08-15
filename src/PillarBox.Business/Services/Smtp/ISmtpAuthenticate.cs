using System.Net;

namespace PillarBox.Business.Services.Smtp
{
    public interface ISmtpAuthenticate
    {
        bool Authenticate(string username, string password);
    }
}