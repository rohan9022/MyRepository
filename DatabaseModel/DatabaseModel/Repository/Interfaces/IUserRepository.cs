using DatabaseModel.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseModel.Repository
{
    internal interface IUserRepository
    {
        void Add(UserMaster userMaster);

        void Update(UserMaster userMaster);

        void Delete(int id);

        UserMaster Get(int id);

        IList<UserMaster> GetAll();
    }
}