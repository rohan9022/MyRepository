using System;
using System.Collections.Generic;
using System.Text;
using DatabaseModel.Models;

namespace DatabaseModel.Repository
{
    internal class UserRepository : IUserRepository
    {
        private readonly TestContext testContext;

        public UserRepository(IDbContextRepository dbContextRepository)
        {
            testContext = dbContextRepository.TestContext;
        }

        public void Add(UserMaster userMaster)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public UserMaster Get(int id)
        {
            throw new NotImplementedException();
        }

        public IList<UserMaster> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Update(UserMaster userMaster)
        {
            throw new NotImplementedException();
        }
    }
}