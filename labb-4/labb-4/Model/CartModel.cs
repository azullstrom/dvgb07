using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace labb_4.Model
{
    internal class CartModel
    {

        public CartModel(List<Product> mainProducts)
        {
            CartProducts = new List<Product>();
            MainProducts = mainProducts;
        }

        public List<Product> CartProducts { get; set; }
        public List<Product> MainProducts { get; set; }

        public List<Product> AddToCartAndReturnList(Product product)
        {
            bool productExists = CartProducts.Any(p => p.Name == product.Name);

            if (productExists)
            {
                Product existingProduct = CartProducts.First(p => p.Name == product.Name);
                if (existingProduct.Quantity < product.Quantity)
                {
                    existingProduct.Quantity++;
                }
            }
            else
            {
                MainProducts.Add(product);
                product.Quantity = 1;
                CartProducts.Add(product);
            }

            return CartProducts;
        }

        public List<Product> RemoveFromCartAndReturnList(Product product)
        {
            Product productToRemove = CartProducts.FirstOrDefault(p => p.Name == product.Name);

            if (productToRemove != null)
            {
                MainProducts.Remove(productToRemove);
                CartProducts.Remove(productToRemove);
            }

            return CartProducts;
        }


        public List<Product> IncreaseQuantityAndReturnList(Product product)
        {
            Product existingProduct = CartProducts.FirstOrDefault(p => p.Name == product.Name);

            if (existingProduct != null)
            {
                Product mainProduct = MainProducts.FirstOrDefault(p => p.Name == product.Name);
                if (existingProduct.Quantity < mainProduct.Quantity)
                {
                    existingProduct.Quantity++;
                }
            }

            return CartProducts;
        }

        public List<Product> DecreaseQuantityAndReturnList(Product product)
        {
            Product existingProduct = CartProducts.FirstOrDefault(p => p.Name == product.Name);

            if (existingProduct != null)
            {
                if (existingProduct.Quantity == 1)
                {
                    MainProducts.Remove(existingProduct);
                    CartProducts.Remove(existingProduct);
                }
                else
                {
                    existingProduct.Quantity--;
                }
            }

            return CartProducts;
        }

        public Product FindProduct(Product product)
        {
            Product existingProduct = CartProducts.FirstOrDefault(p => p.Name == product.Name);

            if (existingProduct != null)
            {
                return existingProduct;
            }
            return null;
        }

        public List<Product> SetCartItemQuantityAndReturnList(Product cartProduct, int quantity)
        {
            Product existingProduct = CartProducts.FirstOrDefault(p => p.Name == cartProduct.Name);

            if (existingProduct != null)
            {
                if (quantity > 0)
                {
                    existingProduct.Quantity = quantity;
                }
                else
                {
                    MainProducts.Remove(existingProduct);
                    CartProducts.Remove(existingProduct);
                }
            }

            return CartProducts;
        }

    }
}
