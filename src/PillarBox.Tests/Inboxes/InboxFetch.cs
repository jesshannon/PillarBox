using PillarBox.Business.Services.Messages;
using PillarBox.Data;
using PillarBox.Data.Messages;
using PillarBox.Tests.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace PillarBox.Tests.Inboxes
{
    public class InboxFetch
    {
        MockedDbContext<PillarBoxContext> db = new MockedDbContext<PillarBoxContext>();

        [Fact]
        void NewMultiDepthInbox()
        {
            InboxService inboxService = new InboxService(db.Object, null);

            var variables = new Dictionary<string, string>() {
                {"SPECIAL/_VARIABLE","special/value" }
            };

            var newInbox = inboxService.GetInboxByPath(inboxService.TokenizeString("first/[SPECIAL/_VARIABLE]/last", variables));

            Assert.Equal("last", newInbox.Name);
            Assert.NotNull(newInbox.ParentInbox);

            Assert.Equal("special/value", newInbox.ParentInbox.Name);
            Assert.NotNull(newInbox.ParentInbox.ParentInbox);
            Assert.Contains(newInbox, newInbox.ParentInbox.Children);

            Assert.Equal("first", newInbox.ParentInbox.ParentInbox.Name);
            Assert.Null(newInbox.ParentInbox.ParentInbox.ParentInbox);
            Assert.Contains(newInbox.ParentInbox, newInbox.ParentInbox.ParentInbox.Children);
        }

        [Fact]
        void NewInboxUnderExisting()
        {
            InboxService inboxService = new InboxService(db.Object, null);

            Inbox existing = new Inbox() {
                Name = "existing",
                Id = Guid.NewGuid()
            };
            db.Object.Inboxes.Add(existing);

            var newInbox = inboxService.GetInboxByPath(inboxService.TokenizeString("existing/new", new Dictionary<string, string>()));

            Assert.Equal("new", newInbox.Name);
            Assert.NotNull(newInbox.ParentInbox);

            Assert.Equal(existing.Id, newInbox.ParentInbox.Id);

        }

        [Fact]
        void NewInboxWithLeadingTrailing()
        {
            InboxService inboxService = new InboxService(db.Object, null);
            
            var newInbox = inboxService.GetInboxByPath(inboxService.TokenizeString("/one/two/", new Dictionary<string, string>()));

            Assert.Equal("two", newInbox.Name);
            Assert.NotNull(newInbox.ParentInbox);

            Assert.Equal("one", newInbox.ParentInbox.Name);
            Assert.Null(newInbox.ParentInbox.ParentInbox);
            Assert.Contains(newInbox, newInbox.ParentInbox.Children);
            
        }
    }
}
