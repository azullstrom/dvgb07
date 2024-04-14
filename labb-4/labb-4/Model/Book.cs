using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace labb_4.Model
{
    public class Book : Product
    {
        public string Author { get; set; }
        public string Genre { get; set; }
        public string Format { get; set; }
        public string Language { get; set; }

        public Book(string name, int price, int quantity, int pid, string author, string genre, string format, string language)
            : base(name, price, quantity, pid)
        {
            Author = author;
            Genre = genre;
            Format = format;
            Language = language;
        }
    }
}
