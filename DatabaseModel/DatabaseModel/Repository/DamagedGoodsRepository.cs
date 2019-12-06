using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DatabaseModel.Models;

namespace DatabaseModel.Repository
{
    internal class DamagedGoodsRepository : IDamagedGoodsRepository
    {
        private readonly TestContext testContext;
        private readonly IProductRepository productRepository;
        private readonly IInventoryItemRepository inventoryItemRepository;

        public DamagedGoodsRepository(IDbContextRepository dbContextRepository, IProductRepository productRepository, IInventoryItemRepository inventoryItemRepository)
        {
            testContext = dbContextRepository.TestContext;
            this.productRepository = productRepository;
            this.inventoryItemRepository = inventoryItemRepository;
        }

        public void Add(DamagedGoodsMaster damagedGoodsMaster, int type)
        {
            testContext.DamagedGoodsMaster.Add(damagedGoodsMaster);
            if (type == 1)
            {
                var product = productRepository.Get(damagedGoodsMaster.ProductId);
                product.TotalDamage += damagedGoodsMaster.Quantity;
                product.TotalAvailableQuota -= damagedGoodsMaster.Quantity;
                product.ModifiedDate = damagedGoodsMaster.ModifiedDate;
                product.ModifiedBy = damagedGoodsMaster.ModifiedBy;
            }
            else
            {
                var inventoryItem = inventoryItemRepository.Get(damagedGoodsMaster.ProductId);
                inventoryItem.TotalWasted += damagedGoodsMaster.Quantity;
                inventoryItem.TotalAvailable -= damagedGoodsMaster.Quantity;
                inventoryItem.ModifiedDate = damagedGoodsMaster.ModifiedDate;
                inventoryItem.ModifiedBy = damagedGoodsMaster.ModifiedBy;
            }

            testContext.SaveChanges();
        }

        public void Delete(DamagedGoodsMaster damagedGoodsMaster, int type)
        {
            var damagedGoodsItem = Get(damagedGoodsMaster.Date, damagedGoodsMaster.ProductId);
            testContext.DamagedGoodsMaster.Remove(damagedGoodsItem);

            if (type == 1)
            {
                var product = productRepository.Get(damagedGoodsItem.ProductId);
                product.TotalDamage -= damagedGoodsItem.Quantity;
                product.TotalAvailableQuota += damagedGoodsItem.Quantity;
                product.ModifiedDate = damagedGoodsItem.ModifiedDate;
                product.ModifiedBy = damagedGoodsItem.ModifiedBy;
            }
            else
            {
                var inventoryItem = inventoryItemRepository.Get(damagedGoodsItem.ProductId);
                inventoryItem.TotalWasted -= damagedGoodsItem.Quantity;
                inventoryItem.TotalAvailable += damagedGoodsItem.Quantity;
                inventoryItem.ModifiedDate = damagedGoodsItem.ModifiedDate;
                inventoryItem.ModifiedBy = damagedGoodsItem.ModifiedBy;
            }

            testContext.SaveChanges();
        }

        public DamagedGoodsMaster Get(DateTime damagedDate, string productId)
        {
            return testContext.DamagedGoodsMaster.Find(damagedDate, productId);
        }

        public IList<DamagedGoodsMaster> GetAll()
        {
            return testContext.DamagedGoodsMaster.ToList();
        }

        public void Update(DamagedGoodsMaster damagedGoodsMaster, int type)
        {
            var damagedGoodsItem = Get(damagedGoodsMaster.Date, damagedGoodsMaster.ProductId);
            int oldStock = damagedGoodsItem.Quantity;

            damagedGoodsItem.Quantity = damagedGoodsMaster.Quantity;
            damagedGoodsItem.Price = damagedGoodsMaster.Price;
            damagedGoodsItem.Comment = damagedGoodsMaster.Comment;
            damagedGoodsItem.ModifiedDate = damagedGoodsMaster.ModifiedDate;
            damagedGoodsItem.ModifiedBy = damagedGoodsMaster.ModifiedBy;

            if (type == 1)
            {
                var product = productRepository.Get(damagedGoodsItem.ProductId);
                product.TotalDamage += (damagedGoodsMaster.Quantity - oldStock);
                product.TotalAvailableQuota -= (damagedGoodsMaster.Quantity - oldStock);
                product.ModifiedDate = damagedGoodsItem.ModifiedDate;
                product.ModifiedBy = damagedGoodsItem.ModifiedBy;
            }
            else
            {
                var inventoryItem = inventoryItemRepository.Get(damagedGoodsItem.ProductId);
                inventoryItem.TotalWasted += (damagedGoodsMaster.Quantity - oldStock); ;
                inventoryItem.TotalAvailable -= (damagedGoodsMaster.Quantity - oldStock); ;
                inventoryItem.ModifiedDate = damagedGoodsItem.ModifiedDate;
                inventoryItem.ModifiedBy = damagedGoodsItem.ModifiedBy;
            }

            testContext.SaveChanges();
        }

        //[sp_dgm_GetDetails]
        public IList<DamagedGoodsMaster> GetDetails(DateTime damagedDate, string productId)
        {
            if (!string.IsNullOrEmpty(productId) && damagedDate != new DateTime())
            {
                return new List<DamagedGoodsMaster>() { testContext.DamagedGoodsMaster.Find(damagedDate, productId) };
            }
            if (!string.IsNullOrEmpty(productId))
            {
                return (List<DamagedGoodsMaster>)GetAll().Where(x => x.ProductId == productId);
            }
            if (damagedDate != new DateTime())
            {
                return (List<DamagedGoodsMaster>)GetAll().Where(x => x.Date == damagedDate);
            }

            return new List<DamagedGoodsMaster>();
        }

        //[sp_dgm_InsertUpdateDeleteDamage]
        public void InsertUpdateDeleteDamage(DamagedGoodsMaster damagedGoodsMaster, int type, int mode)
        {
            if (mode == 2)
            {
                Add(damagedGoodsMaster, type);
            }
            if (mode == 3)
            {
                Update(damagedGoodsMaster, type);
            }
            if (mode == 4)
            {
                Delete(damagedGoodsMaster, type);
            }
        }
    }
}