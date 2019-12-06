using System;
using System.Collections.Generic;
using System.Text;
using DatabaseModel.Models;

namespace DatabaseModel.Repository
{
    internal class DbContextRepository : IDbContextRepository
    {
        public TestContext TestContext
        {
            get
            {
                return new TestContext();
            }
        }
    }
}