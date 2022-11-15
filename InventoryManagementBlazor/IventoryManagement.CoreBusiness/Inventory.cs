using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IventoryManagement.CoreBusiness
{
    public class Inventory
    {
        public int InventoryId { get; set; }

        public string InventoryName { get; set; } = string.Empty;

        public int Quantity { get; set; }

        public decimal Price { get; set; }
    }
}
