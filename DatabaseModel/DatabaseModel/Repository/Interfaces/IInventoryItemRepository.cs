using DatabaseModel.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseModel.Repository
{
    internal interface IInventoryItemRepository
    {
        void Add(InventoryItemMaster inventoryItemMaster);

        void Update(InventoryItemMaster inventoryItemMaster);

        void Delete(string id);

        InventoryItemMaster Get(string id);

        IList<InventoryItemMaster> GetAll();
    }
}