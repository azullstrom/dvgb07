using labb_4.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace labb_4.ViewModel
{
    internal class SelectionViewModel : ViewModelBase
    {
        private VisibilityViewModel _visibilityViewModel;
        private ColorViewModel _colorViewModel;
        private string _selectedProductType;
        private Product _selectedCartItem;
        private Product _selectedProduct;
        private Book _selectedBook;
        private Movie _selectedMovie;
        private Game _selectedGame;

        public SelectionViewModel(VisibilityViewModel visibilityViewModel, ColorViewModel colorViewModel)
        {
            _visibilityViewModel = visibilityViewModel;
            _colorViewModel = colorViewModel;
        }

        public string SelectedProductType
        {
            get { return _selectedProductType; }
            set
            {

                if (_selectedProductType != value)
                {
                    _selectedProductType = value;

                    if (value == "Bok")
                    {
                        _visibilityViewModel.IsBookTemplateVisible = Visibility.Visible;
                        _visibilityViewModel.IsGameTemplateVisible = Visibility.Collapsed;
                        _visibilityViewModel.IsMovieTemplateVisible = Visibility.Collapsed;
                        _visibilityViewModel.IsAddButtonEnabled = true;
                    }
                    else if (value == "Film")
                    {
                        _visibilityViewModel.IsBookTemplateVisible = Visibility.Collapsed;
                        _visibilityViewModel.IsGameTemplateVisible = Visibility.Collapsed;
                        _visibilityViewModel.IsMovieTemplateVisible = Visibility.Visible;
                        _visibilityViewModel.IsAddButtonEnabled = true;
                    }
                    else if (value == "Spel")
                    {
                        _visibilityViewModel.IsBookTemplateVisible = Visibility.Collapsed;
                        _visibilityViewModel.IsGameTemplateVisible = Visibility.Visible;
                        _visibilityViewModel.IsMovieTemplateVisible = Visibility.Collapsed;
                        _visibilityViewModel.IsAddButtonEnabled = true;
                    }
                    else
                    {
                        _visibilityViewModel.IsBookTemplateVisible = Visibility.Collapsed;
                        _visibilityViewModel.IsGameTemplateVisible = Visibility.Collapsed;
                        _visibilityViewModel.IsMovieTemplateVisible = Visibility.Collapsed;
                        _visibilityViewModel.IsAddButtonEnabled = false;
                    }

                    _colorViewModel.ResetColors();
                    OnPropertyChanged(nameof(SelectedProductType));
                }
            }
        }

        public Product SelectedCartItem
        {
            get { return _selectedCartItem; }
            set
            {
                if (_selectedCartItem != value)
                {
                    _visibilityViewModel.IsEditCartEnabled = true;
                    _selectedCartItem = value;
                    OnPropertyChanged(nameof(SelectedCartItem));
                }
            }
        }
        public Product SelectedProduct
        {
            get { return _selectedProduct; }
            set
            {
                if (_selectedProduct != value)
                {
                    _selectedProduct = value;
                    OnPropertyChanged(nameof(SelectedProduct));
                }
            }
        }
        public Book SelectedBook
        {
            get { return _selectedBook; }
            set
            {
                if (_selectedBook != value)
                {
                    _visibilityViewModel.IsEditProductEnabled = true;
                    SelectedGame = null;
                    SelectedMovie = null;
                    _selectedBook = value;
                    OnPropertyChanged(nameof(SelectedBook));
                    SelectedProduct = _selectedBook;
                }
            }
        }
        public Movie SelectedMovie
        {
            get { return _selectedMovie; }
            set
            {
                if (_selectedMovie != value)
                {
                    _visibilityViewModel.IsEditProductEnabled = true;
                    SelectedGame = null;
                    SelectedBook = null;
                    _selectedMovie = value;
                    OnPropertyChanged(nameof(SelectedMovie));
                    SelectedProduct = _selectedMovie;
                }
            }
        }
        public Game SelectedGame
        {
            get { return _selectedGame; }
            set
            {
                if (_selectedGame != value)
                {
                    _visibilityViewModel.IsEditProductEnabled = true;
                    SelectedBook = null;
                    SelectedMovie = null;
                    _selectedGame = value;
                    OnPropertyChanged(nameof(SelectedGame));
                    SelectedProduct = _selectedGame;
                }
            }
        }
    }
}
