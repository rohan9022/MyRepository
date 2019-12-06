using System;
using System.Collections.Generic;
using System.Text;
using DatabaseModel.Models;

namespace DatabaseModel.Repository
{
    internal class MenuRepository : IMenuRepository
    {
        private readonly TestContext testContext;

        public MenuRepository(IDbContextRepository dbContextRepository)
        {
            testContext = dbContextRepository.TestContext;
        }

        public void Add(MenuMaster menuMaster)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public MenuMaster Get(int id)
        {
            throw new NotImplementedException();
        }

        public IList<MenuMaster> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Update(MenuMaster menuMaster)
        {
            throw new NotImplementedException();
        }
    }
}