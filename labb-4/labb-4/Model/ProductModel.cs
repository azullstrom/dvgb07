using labb_4.Database;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace labb_4.Model
{
    internal class ProductModel
    {
        private DatabaseService _databaseService;

        public ProductModel() { 
            _databaseService = new DatabaseService();
        }

        public List<Product> Products { get; set; }

        public async Task GetProductsFromDatabaseAsync()
        {
            Products = await _databaseService.ReadCSVFileToStringAsync();
        }

        public async Task AddProductToDatabaseAsync(Product product)
        {
            Products = await _databaseService.WriteProductToCSVAsync(product);
        }

        public async Task RemoveProductFromDatabaseAsync(Product product)
        {
            Products = await _databaseService.RemoveProductFromCSVAsync(product);
        }

        public async Task UpdateQtyProductInDatabaseAsync(Product product, int quantity)
        {
            Products = await _databaseService.UpdateQtyProductInCSVAsync(product, quantity);
        }

        internal async Task AddDeliveryToDatabaseAsync(List<Product> products)
        {
            Products = await _databaseService.AddDeliveryToCSVAsync(products);
        }

        internal async Task ReturnProductToDatabaseAsync(Product product)
        {
            Products = await _databaseService.ReturnProductToCSVAsync(product);
        }
    }
}
