using DatabaseModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DatabaseModel.Repository
{
    internal class CategoryRepository : ICategoryRepository
    {
        private readonly TestContext testContext;
        private readonly IProductDescriptionRepository productDescriptionRepository;

        public CategoryRepository(IDbContextRepository dbContextRepository, IProductDescriptionRepository productDescriptionRepository)
        {
            testContext = dbContextRepository.TestContext;
            this.productDescriptionRepository = productDescriptionRepository;
        }

        public void Add(CategoryMaster categoryMaster)
        {
            categoryMaster.CategoryId = testContext.CategoryMaster.Max(x => x.CategoryId) + 1;
            testContext.CategoryMaster.Add(categoryMaster);
            testContext.SaveChanges();
        }

        public void Update(CategoryMaster categoryMaster)
        {
            var category = testContext.CategoryMaster.Find(categoryMaster.CategoryId);
            category.Cgst = categoryMaster.Cgst;
            category.Igst = categoryMaster.Igst;
            category.Name = categoryMaster.Name;
            category.Sgst = categoryMaster.Sgst;

            testContext.SaveChanges();
        }

        //[sp_cm_DeleteCategory]
        public void Delete(int id)
        {
            try
            {
                var category = testContext.CategoryMaster.Find(id);
                testContext.CategoryMaster.Remove(category);
                testContext.SaveChanges();
            }
            catch (Exception)
            {
                throw new NotImplementedException();
            }
        }

        public CategoryMaster Get(int id)
        {
            try
            {
                return testContext.CategoryMaster.Find(id);
            }
            catch (Exception)
            {
                throw new NotImplementedException();
            }
        }

        //[sp_cm_GetCategoryList]
        public IList<CategoryMaster> GetAll()
        {
            return testContext.CategoryMaster.OrderBy(x => x.Name).ToList();
        }

        //[sp_cm_CheckCategoryNameAvailability]
        public bool CheckCategoryNameAvailability(CategoryMaster categoryMaster, int pageMode)
        {
            try
            {
                if (pageMode == 2)
                {
                    return testContext.CategoryMaster.Any(x => x.Name == categoryMaster.Name);
                }

                return testContext.CategoryMaster.Any(x => x.Name == categoryMaster.Name && x.CategoryId == categoryMaster.CategoryId);
            }
            catch (Exception)
            {
                throw new NotImplementedException();
            }
        }

        //[sp_cm_InsertUpdateCategory]
        public void InsertUpdateCategory(CategoryMaster categoryMaster)
        {
            try
            {
                if (categoryMaster.CategoryId > 0)
                {
                    Update(categoryMaster);
                }
                else
                {
                    Add(categoryMaster);
                }
            }
            catch (Exception)
            {
                throw new NotImplementedException();
            }
        }

        //[sp_FindGstRate]
        public decimal FindGstRate(string productId, int GstType)
        {
            var categoryId = productDescriptionRepository.Get(productId).Category;

            if (GstType == 1)
            {
                return Get(categoryId).Cgst;
            }
            if (GstType == 2)
            {
                return Get(categoryId).Sgst;
            }
            if (GstType == 3)
            {
                return Get(categoryId).Igst;
            }

            throw new Exception("GST rate not found!");
        }
    }
}