using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using PillarBox.Business.Exceptions;
using PillarBox.Business.Models;
using PillarBox.Business.Services.Common;
using PillarBox.Data;
using PillarBox.Data.Messages;

namespace PillarBox.Business.Services.Messages
{
    public class InboxService : EntityService<Inbox>, IInboxService
    {
        IUserContext _userContext;
        public InboxService(PillarBoxContext dbContext, IUserContext userContext) : base(dbContext)
        {
            _userContext = userContext;
        }

        public Inbox GetInboxByPath(IEnumerable<string> path)
        {
            Inbox parent = null;
            Guid? parentId = null;
            
            foreach (string name in path)
            {
                var inbox = _dbContext.Inboxes.Where(i => i.Name == name && i.ParentInboxId == parentId).FirstOrDefault();
                if (inbox == null)
                {
                    inbox = new Inbox()
                    {
                        ParentInboxId = parentId,
                        ParentInbox = parent,
                        Name = name,
                        Children = new HashSet<Inbox>()
                    };
                    if (parent != null)
                    {
                        parent.Children.Add(inbox);
                    }
                    _dbContext.Inboxes.Add(inbox);
                }
                parent = inbox;
                parentId = inbox.Id;
            }

            _dbContext.SaveChanges();

            return parent;
        }

        /// <summary>
        /// Returned structured tree of all inboxes
        /// </summary>
        /// <returns></returns>
        public IList<UserInbox> GetRootInboxes()
        {
            var userId = _userContext.CurrentUserId();

            var inboxes = (from Inbox in _dbContext.Inboxes
                            orderby Inbox.Name
                            select new UserInbox {
                              Inbox = Inbox,
                              Starred = _dbContext.UserInboxes.Any(i=>i.Inbox == Inbox && i.Starred && i.UserId == userId),
                              MessageCount = Inbox.Messages.Count
                          }).ToList();
        
            var rootInboxes = new List<UserInbox>();
            var dict = inboxes.ToDictionary(i => i.Inbox.Id, i => i);
            foreach (var userInbox in inboxes)
            {
                if (userInbox.Inbox.ParentInboxId.HasValue)
                {
                    dict[userInbox.Inbox.ParentInboxId.Value].Children.Add(userInbox);
                }
                else
                {
                    rootInboxes.Add(userInbox);
                }
            }

            return rootInboxes;
        }

        public IEnumerable<string> TokenizeString(string path, Dictionary<string, string> contextVariables)
        {
            // temporarily swap out valid tokens before splitting
            var tempTokens = contextVariables.Keys.Select(k =>
            {
                var token = $"[{k}]";
                var temp = Guid.NewGuid().ToString();
                if (path.Contains(token))
                {
                    path = path.Replace(token, temp);
                    return new
                    {
                        temp,
                        value = contextVariables[k]
                    };
                }
                return null;
            }).Where(s => s != null).ToList();

            // clean leading and trailing characters
            path = path.Trim(new char[] { ' ', '/' });
            var pathElements = path.Split('/');

            return pathElements.Select(e => {
                tempTokens.ForEach(t => e = e.Replace(t.temp, t.value));
                return e;
                });
            
        }

        public void SetStar(Guid inboxId, bool isStarred)
        {
            var userId = _userContext.CurrentUserId();
            var userinbox = _dbContext.UserInboxes.Where(i => i.InboxId == inboxId && i.UserId == userId).FirstOrDefault();
            if (userinbox == null)
            {
                userinbox = new Data.Users.UserInbox()
                {
                    InboxId = inboxId,
                    UserId = userId
                };
                _dbContext.UserInboxes.Add(userinbox);
            }
            userinbox.Starred = isStarred;
            _dbContext.SaveChanges();
        }
    }
}
