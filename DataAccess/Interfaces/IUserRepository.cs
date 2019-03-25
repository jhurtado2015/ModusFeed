﻿using DomainEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Interfaces
{
    public interface IUserRepository : IGenericFeedRepository<UserEntity>
    {
      
    }
}
