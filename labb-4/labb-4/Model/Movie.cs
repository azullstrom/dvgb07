using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace labb_4.Model
{
    public class Movie : Product
    {
        public string Format { get; set; }
        public string Time { get; set; }

        public Movie(string name, int price, int quantity, int pid, string format, string time)
            : base(name, price, quantity, pid)
        {
            Format = format;
            Time = time;
        }
    }
}
