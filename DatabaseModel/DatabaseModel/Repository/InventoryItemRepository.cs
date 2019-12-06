using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DatabaseModel.Models;

namespace DatabaseModel.Repository
{
    internal class InventoryItemRepository : IInventoryItemRepository
    {
        private readonly TestContext testContext;

        public InventoryItemRepository(IDbContextRepository dbContextRepository)
        {
            testContext = dbContextRepository.TestContext;
        }

        public void Add(InventoryItemMaster inventoryItemMaster)
        {
            testContext.InventoryItemMaster.Add(inventoryItemMaster);
            testContext.SaveChanges();
        }

        public void Delete(string id)
        {
            try
            {
                var inventory = testContext.InventoryItemMaster.Find(id);
                testContext.InventoryItemMaster.Remove(inventory);
                testContext.SaveChanges();
            }
            catch (Exception)
            {
                throw new NotImplementedException();
            }
        }

        public InventoryItemMaster Get(string id)
        {
            try
            {
                return testContext.InventoryItemMaster.Find(id);
            }
            catch (Exception)
            {
                throw new NotImplementedException();
            }
        }

        public IList<InventoryItemMaster> GetAll()
        {
            return testContext.InventoryItemMaster.OrderBy(x => x.ItemName).ToList();
        }

        public void Update(InventoryItemMaster inventoryItemMaster)
        {
            var inventory = testContext.InventoryItemMaster.Find(inventoryItemMaster.ItemId);
            inventory.ItemName = inventoryItemMaster.ItemName;
            inventory.Price = inventoryItemMaster.Price;
            inventory.TotalAvailable = inventoryItemMaster.TotalAvailable;
            inventory.TotalSales = inventoryItemMaster.TotalSales;
            inventory.TotalWasted = inventoryItemMaster.TotalWasted;

            testContext.SaveChanges();
        }

        //[sp_iim_InsertUpdateItem] needs to be verified sp again
        public void InsertUpdateItem(InventoryItemMaster inventoryItemMaster, int mode)
        {
            if (mode == 2)
            {
                Add(inventoryItemMaster);
            }
            if (mode == 3)
            {
                Update(inventoryItemMaster);
            }
            if (mode == 4)
            {
                Delete(inventoryItemMaster.ItemId);
            }
        }
    }
}