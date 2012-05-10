﻿using NewSocialNetwork.Domain;
using NSN.Framework;
using System.Collections.Generic;

namespace NewSocialNetwork.Repositories
{
    public interface IFriendRepository : IRepository<Friend>
    {
        IList<Friend> GetNotMutualFriends(int userId, int friendUserId);
    }
}