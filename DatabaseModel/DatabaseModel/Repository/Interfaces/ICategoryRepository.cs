using DatabaseModel.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseModel.Repository
{
    internal interface ICategoryRepository
    {
        void Add(CategoryMaster categoryMaster);

        void Update(CategoryMaster categoryMaster);

        void Delete(int id);

        CategoryMaster Get(int id);

        IList<CategoryMaster> GetAll();

        bool CheckCategoryNameAvailability(CategoryMaster categoryMaster, int pageMode);

        void InsertUpdateCategory(CategoryMaster categoryMaster);
    }
}