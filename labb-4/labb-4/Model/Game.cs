using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace labb_4.Model
{
    public class Game : Product
    {
        public string Platform { get; set; }

        public Game(string name, int price, int quantity, int pid, string platform)
            : base(name, price, quantity, pid)
        {
            Platform = platform;
        }
    }
}
