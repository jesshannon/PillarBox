using PillarBox.Data;
using PillarBox.Data.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PillarBox.Business.Services.Common
{
    public class UserContext : IUserContext
    {
        PillarBoxContext _dbContext;
        public UserContext(PillarBoxContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Guid CurrentUserId() {
            // just create a single default user for now, multi user will come later
            var user = _dbContext.User.FirstOrDefault();
            if (user == null)
            {
                user = new User() {
                    Email = "temp@example.com",
                    Password = ""
                };
                _dbContext.User.Add(user);
                _dbContext.SaveChanges();
            }
            return user.Id;
        }
    }
}
