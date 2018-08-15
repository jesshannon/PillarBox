using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace PillarBox.Business.Services.Smtp
{
    public interface IMessageHandler
    {
        void HandleMessage(Stream content);
    }
}
