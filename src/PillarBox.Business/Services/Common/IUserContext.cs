using System;

namespace PillarBox.Business.Services.Common
{
    public interface IUserContext
    {
        Guid CurrentUserId();
    }
}