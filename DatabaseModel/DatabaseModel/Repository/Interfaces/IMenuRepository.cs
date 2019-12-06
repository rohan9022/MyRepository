using DatabaseModel.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseModel.Repository
{
    internal interface IMenuRepository
    {
        void Add(MenuMaster menuMaster);

        void Update(MenuMaster menuMaster);

        void Delete(int id);

        MenuMaster Get(int id);

        IList<MenuMaster> GetAll();
    }
}