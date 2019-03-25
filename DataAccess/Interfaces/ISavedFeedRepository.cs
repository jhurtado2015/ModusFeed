using DomainEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Interfaces
{
    public interface ISavedFeedRepository: IGenericFeedRepository<SavedFeedEntity>
    {
    }
}
