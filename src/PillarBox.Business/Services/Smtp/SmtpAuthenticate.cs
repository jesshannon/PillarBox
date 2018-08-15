using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace PillarBox.Business.Services.Smtp
{
    public class SmtpAuthenticate : ISmtpAuthenticate
    {
        IMessageContext _context;
        public SmtpAuthenticate(IMessageContext context)
        {
            _context = context;
        }

        public bool Authenticate(string username, string password)
        {
            _context.User = username;
            _context.Password = password;
            return true;
        }
    }
}
