using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace labb_4.Model
{
    public class Product
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public int PID { get; set; }
        public int Quantity { get; set; }

        // Copy constructor
        public Product(Product original)
        {
            Name = original.Name;
            Price = original.Price;
            PID = original.PID;
            Quantity = original.Quantity;
        }

        public Product(string name, int price, int quantity, int pid) { 
            Name = name;
            Price = price;
            PID = pid;
            Quantity = quantity;
        }
    }
}
