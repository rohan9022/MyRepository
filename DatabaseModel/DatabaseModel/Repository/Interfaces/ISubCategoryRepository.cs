using DatabaseModel.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseModel.Repository
{
    internal interface ISubCategoryRepository
    {
        void Add(SubCategoryMaster subCategoryMaster);

        void Update(SubCategoryMaster subCategoryMaster);

        void Delete(int id);

        SubCategoryMaster Get(int id);

        IList<SubCategoryMaster> GetAll();
    }
}