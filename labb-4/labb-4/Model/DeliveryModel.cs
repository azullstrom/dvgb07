using labb_4.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace labb_4.Model
{
    internal class DeliveryModel
    {
        private ListViewModel _listViewModel;

        public DeliveryModel(ListViewModel listViewModel)
        {
            _listViewModel = listViewModel;
            DeliveryProducts = new List<Product>();
        }

        public List<Product> DeliveryProducts { get; set; }

        public void AddToCart(Product product)
        {
            bool productExists = DeliveryProducts.Any(p => p.Name == product.Name);

            if (productExists)
            {
                Product existingProduct = DeliveryProducts.First(p => p.Name == product.Name);
                existingProduct.Quantity += product.Quantity;
            }
            else
            {
                DeliveryProducts.Add(product);
            }
            _listViewModel.DeliveryItems = null;
            _listViewModel.DeliveryItems = DeliveryProducts;
        }
    }
}
