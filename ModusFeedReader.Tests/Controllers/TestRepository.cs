using DataAccess.DatabaseContext;
using DataAccess.Interfaces;
using DataAccess.Repositories;
using DomainEntities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModusFeedReader.Tests.Controllers
{
    [TestClass]
    public class TestRepository
    {
        [TestMethod]
        public void All()
        {
            var context = new FeedContext();

            IUserRepository userRep = new UserRepository(context);

           
        }
    }
}
