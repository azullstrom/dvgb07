using labb_4.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace labb_4.ViewModel
{
    internal class ListViewModel : ViewModelBase
    {
        private List<Product> _cartItems;
        private List<Product> _deliveryItems;
        private List<Book> _books;
        private List<Movie> _movies;
        private List<Game> _games;

        public ListViewModel()
        {
            ProductTypes = new List<string> { "Bok", "Film", "Spel" };
        }

        public List<string> ProductTypes { get; }

        public List<Product> CartItems
        {
            get { return _cartItems; }
            set
            {
                if (_cartItems != value)
                {
                    _cartItems = value;
                    OnPropertyChanged(nameof(CartItems));
                }
            }
        }
        public List<Product> DeliveryItems
        {
            get { return _deliveryItems; }
            set
            {
                if (_deliveryItems != value)
                {
                    _deliveryItems = value;
                    OnPropertyChanged(nameof(DeliveryItems));
                }
            }
        }
        public List<Book> Books
        {
            get { return _books; }
            set
            {
                if (_books != value)
                {
                    _books = value;
                    OnPropertyChanged(nameof(Books));
                }
            }
        }
        public List<Movie> Movies
        {
            get { return _movies; }
            set
            {
                if (_movies != value)
                {
                    _movies = value;
                    OnPropertyChanged(nameof(Movies));
                }
            }
        }
        public List<Game> Games
        {
            get { return _games; }
            set
            {
                if (_games != value)
                {
                    _games = value;
                    OnPropertyChanged(nameof(Games));
                }
            }
        }

        public List<Book> FilteredBooks { get; internal set; }
        public List<Movie> FilteredMovies { get; internal set; }
        public List<Game> FilteredGames { get; internal set; }
    }
}
