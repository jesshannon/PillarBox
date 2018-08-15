using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using PillarBox.Data.Common;
using PillarBox.Data.Filters;
using PillarBox.Data.Messages;
using PillarBox.Data.Users;

namespace PillarBox.Data
{
    public partial class PillarBoxContext : DbContext
    {

        public virtual DbSet<Inbox> Inboxes { get; set; }
        public virtual DbSet<Message> Messages { get; set; }

        public virtual DbSet<MessageAction> MessageActions { get; set; }
        public virtual DbSet<MessageFilter> MessageFilters { get; set; }
        public virtual DbSet<MessageRule> MessageRules { get; set; }

        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserInbox> UserInboxes { get; set; }
        public virtual DbSet<UserMessage> UserMessages { get; set; }
               

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite(@"DataSource=PillarBox.sqlite3");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Inbox>()
                .HasMany(d => d.Children)
                .WithOne(d => d.ParentInbox);

            modelBuilder.Entity<Inbox>()
                .HasMany(i => i.Messages)
                .WithOne(m => m.Inbox)
                .IsRequired();

            modelBuilder.Entity<MessageAction>()
               .ToTable("MessageActions")
               .HasDiscriminator<string>("ActionType")
               .HasValue<MessageActionForward>("Foward")
               .HasValue<MessageActionNotification>("Notification")
               .HasValue<MessageActionWebHook>("WebHook");

        }

        public override int SaveChanges()
        {
            AddTimestamps();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            AddTimestamps();
            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
        
        private void AddTimestamps()
        {
            var entities = ChangeTracker.Entries().Where(x => x.Entity is EntityBase && (x.State == EntityState.Added || x.State == EntityState.Modified));

            foreach (var entity in entities)
            {
                if (entity.State == EntityState.Added)
                {
                    ((EntityBase)entity.Entity).DateCreated = DateTime.UtcNow;
                }

                ((EntityBase)entity.Entity).DateModified = DateTime.UtcNow;
            }
        }
    }
}
