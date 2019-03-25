using System;
using System.Collections.Generic;
using System.Text;

namespace DomainEntities
{
    public class UserEntity : BaseEntity
    {
        public string userName { get; set; }

        public string firstName { get; set; }

        public string lastName { get; set; }
    }
}
