using DataAccess.DatabaseContext;
using DataAccess.Interfaces;
using DomainEntities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class UserRepository : GenericFeedRepository<UserEntity>, IGenericFeedRepository<UserEntity>, IUserRepository
    {
        public UserRepository(FeedContext context) : base(context)
        {
            
        }

    }
}
