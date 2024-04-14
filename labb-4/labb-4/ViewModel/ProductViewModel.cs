using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace labb_4.ViewModel
{
    internal class ProductViewModel : ViewModelBase
    {
        private string _productQuantityToAdd, _newName, _newPrice, _newQuantity, _newBookAuthor, _newBookGenre, _newBookFormat, _newBookLanguage, _newGamePlatform, _newMovieFormat, _newMovieDuration;

        public ProductViewModel()
        {
            
        }
        
        public string NewName
        {
            get { return _newName; }
            set
            {
                _newName = value;
                OnPropertyChanged(nameof(NewName));
            }
        }
        public string NewPrice
        {
            get { return _newPrice; }
            set
            {
                _newPrice = value;
                OnPropertyChanged(nameof(NewPrice));
            }
        }
        public string NewQuantity
        {
            get { return _newQuantity; }
            set
            {
                _newQuantity = value;
                OnPropertyChanged(nameof(NewQuantity));
            }
        }
        public string NewBookAuthor
        {
            get { return _newBookAuthor; }
            set
            {
                _newBookAuthor = value;
                OnPropertyChanged(nameof(NewBookAuthor));
            }
        }
        public string NewBookGenre
        {
            get { return _newBookGenre; }
            set
            {
                _newBookGenre = value;
                OnPropertyChanged(nameof(NewBookGenre));
            }
        }
        public string NewBookFormat
        {
            get { return _newBookFormat; }
            set
            {
                _newBookFormat = value;
                OnPropertyChanged(nameof(NewBookFormat));
            }
        }
        public string NewBookLanguage
        {
            get { return _newBookLanguage; }
            set
            {
                _newBookLanguage = value;
                OnPropertyChanged(nameof(NewBookLanguage));
            }
        }
        public string NewGamePlatform
        {
            get { return _newGamePlatform; }
            set
            {
                _newGamePlatform = value;
                OnPropertyChanged(nameof(NewGamePlatform));
            }
        }
        public string NewMovieFormat
        {
            get { return _newMovieFormat; }
            set
            {
                _newMovieFormat = value;
                OnPropertyChanged(nameof(NewMovieFormat));
            }
        }
        public string NewMovieDuration
        {
            get { return _newMovieDuration; }
            set
            {
                _newMovieDuration = value;
                OnPropertyChanged(nameof(NewMovieDuration));
            }
        }
        public string ProductQuantityTextBox
        {
            get { return _productQuantityToAdd; }
            set
            {
                _productQuantityToAdd = value;
                OnPropertyChanged(nameof(ProductQuantityTextBox));
            }
        }
    }
}
