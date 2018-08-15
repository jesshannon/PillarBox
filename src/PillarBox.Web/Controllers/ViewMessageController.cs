using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using PillarBox.Business.Services.Messages;
using PillarBox.Data.Messages;
using PillarBox.Web.Models.Messages;
using PillarBox.Web.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PillarBox.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/ViewMessage")]
    public class ViewMessageController : Controller
    {
        IMapper _mapper;
        IInboxService _inboxService;
        IMessagesService _messagesService;

        public ViewMessageController(IMapper mapper, IInboxService inboxService, IMessagesService messagesService)
        {
            _mapper = mapper;
            _inboxService = inboxService;
            _messagesService = messagesService;
        }

        

        [Route("[action]")]
        public ActionResult ContentPart(string id, string contentId)
        {
            var message = _messagesService.GetById(Guid.Parse(id));
            var mime = MimeMessageFromMessage(message);
            var part = mime.BodyParts.Where(b => b.ContentId == contentId).FirstOrDefault();
            if (part == null)
            {
                return new ContentResult()
                {
                    StatusCode = 404
                };
            }

            var ms = new MemoryStream();
            ((MimePart)part).Content.DecodeTo(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, part.ContentType.MimeType);
        }

        [Route("[action]")]
        public ContentResult View(string id, ViewType viewType, bool showImages = true)
        {
            var message = _messagesService.GetById(Guid.Parse(id));
            var mime = MimeMessageFromMessage(message);

            switch (viewType)
            {
                case ViewType.Original:
                    return Content(message.Source, "text/plain");

                case ViewType.Standard:
                    var htmlBodyPart = mime.BodyParts.Where(b => b.ContentType.MimeType == "text/html" && !(b.ContentDisposition?.IsAttachment ?? false)).FirstOrDefault();
                    if (htmlBodyPart == null) {
                        goto case ViewType.Plain;
                    }
                    return Content(StandardView(MimePartBody(htmlBodyPart), id, showImages), "text/html");
                    break;

                case ViewType.HTML:
                    var htmlSourceBodyPart = mime.BodyParts.Where(b => b.ContentType.MimeType == "text/html" && !(b.ContentDisposition?.IsAttachment ?? false)).FirstOrDefault();
                    if (htmlSourceBodyPart == null)
                    {
                        goto case ViewType.Plain;
                    }
                    return Content(HTMLView(MimePartBody(htmlSourceBodyPart)), "text/html");
                    break;

                case ViewType.Plain:
                    var plainBodyPart = mime.BodyParts.Where(b => b.ContentType.MimeType == "text/plain" && !(b.ContentDisposition?.IsAttachment ?? false)).FirstOrDefault();
                    if (plainBodyPart == null)
                    {
                        return Content("No plain text content found.", "text/plain");
                    }
                    return Content(PlainTextView(MimePartBody(plainBodyPart)), "text/html");
                    break;

                 default:
                    goto case ViewType.Original;
            }
            
        }

        private string MimePartBody(MimeEntity part)
        {
            var ms = new MemoryStream();
            ((MimePart)part).Content.DecodeTo(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return new StreamReader(ms).ReadToEnd();
        }

        private string StandardView(string body, string messageId, bool showImages)
        {
            body = Regex.Replace(body, "(\"|')cid:([^\"']*)", $@"$1/api/ViewMessage/ContentPart?id={messageId}&contentId=$2", RegexOptions.IgnoreCase);

            body = "<style>html,body{font-family:sans-serif;}</style>" + body;

            return body;
        }

        private string HTMLView(string body)
        {
            return $@"<html>
                    <head>
                        <link rel=""stylesheet"" href=""/assets/highlight/github.css"">
                        <script src=""/assets/highlight/highlight.pack.js""></script>
                        <script>hljs.initHighlightingOnLoad();</script>
                    </head>
                    <body style=""margin:0;"">
                        <pre><code class=""html"">{body.Replace("<", "&lt;")}</code></pre>
                    </body>
                    </html>";
        }

        private static string PlainTextView(string body)
        {
            body = $"<pre>{body.Replace("<", "&lt;")}</pre>";

            body = Regex.Replace(body, @"((http|ftp|https)://([\w_-]+(?:(?:\.[\w_-]+)+))([\w.,@?^=%&:/~+#-]*[\w@?^=%&/~+#-])?)", @"<a href=""$1"">$1</a>");

            return body;
        }

        public static MimeMessage MimeMessageFromMessage(Message m)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(m.Source);
            writer.Flush();
            stream.Position = 0;
            return MimeMessage.Load(stream);
        }
    }
}