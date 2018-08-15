using AutoMapper;
using PillarBox.Business.Models;
using PillarBox.Data.Filters;
using PillarBox.Data.Messages;
using PillarBox.Web.Models;
using PillarBox.Web.Models.Messages;
using PillarBox.Web.Models.Notifications;
using PillarBox.Web.Models.Settings;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PillarBox.Web.Init
{
    public class AutoMapperProfileConfiguration : Profile
    {
        public AutoMapperProfileConfiguration()
        {
            CreateMap(typeof(PaginatedList<>), typeof(PaginatedListModel<>))
                .ForMember("Items", t => t.MapFrom(o=> o ));

            CreateMap(typeof(ICollection<>), typeof(PaginatedListModel<>))
                .ForAllMembers(o => o.Ignore());

            CreateMap<Inbox, InboxModel>();
            CreateMap<Inbox, ViewInboxModel>()
                .ForMember(t => t.ParentPath, t => t.MapFrom(s => ParentPath(s)));

            CreateMap<UserInbox, UserInboxModel>();

            CreateMap<Message, NotificationModel>();
            CreateMap<Message, MessageSummaryModel>()
                .ForMember(m => m.Intro, t => t.MapFrom(o => o.Summary.Length>=20?o.Summary.Substring(0, 20): o.Summary));

            CreateMap<Message, MessageDetailsModel>()
                .ForMember(m => m.SizeBytes, t => t.MapFrom(o => o.Source.Length));

            CreateMap<MessageRule, RuleSummary>()
                .ForMember(m => m.Fields, t => t.MapFrom(o => o.Filters.Select(f => $"{f.FieldName} = {f.Pattern}")))
                .ForMember(m => m.Actions, t => t.MapFrom(o => o.Actions.Select(a => a.GetType().Name)));

        }

        private string ParentPath(Inbox inbox)
        {
            var i = inbox;
            string path = "/";
            while (i.ParentInbox != null)
            {
                i = i.ParentInbox;
                path = $"/{i.Name}{path}";
            }
            return path;
        }
    }
}
