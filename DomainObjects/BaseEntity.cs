using System;
using System.ComponentModel;

namespace DomainEntities
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public bool IsActive { get => isActive; set => isActive = value; }

        private bool isActive = true;
    }
}
