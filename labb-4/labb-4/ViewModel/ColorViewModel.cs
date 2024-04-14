using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Windows.UI;

namespace labb_4.ViewModel
{
    internal class ColorViewModel : ViewModelBase
    {
        private SolidColorBrush _nameInputColor;
        private SolidColorBrush _priceInputColor;
        private SolidColorBrush _quantityInputColor;
        private SolidColorBrush _bookAuthorInputColor;
        private SolidColorBrush _bookGenreInputColor;
        private SolidColorBrush _bookFormatInputColor;
        private SolidColorBrush _bookLanguageInputColor;
        private SolidColorBrush _gamePlatformInputColor;
        private SolidColorBrush _movieFormatInputColor;
        private SolidColorBrush _movieDurationInputColor;

        public ColorViewModel()
        {
            StandardColor = Colors.LightGray;
            ErrorColor = Colors.IndianRed;
            _nameInputColor = new SolidColorBrush(StandardColor);
            _priceInputColor = new SolidColorBrush(StandardColor);
            _quantityInputColor = new SolidColorBrush(StandardColor);
            _bookAuthorInputColor = new SolidColorBrush(StandardColor);
            _bookGenreInputColor = new SolidColorBrush(StandardColor);
            _bookFormatInputColor = new SolidColorBrush(StandardColor);
            _bookLanguageInputColor = new SolidColorBrush(StandardColor);
            _gamePlatformInputColor = new SolidColorBrush(StandardColor);
            _movieFormatInputColor = new SolidColorBrush(StandardColor);
            _movieDurationInputColor = new SolidColorBrush(StandardColor);
        }

        public Color StandardColor { get; set; }

        public Color ErrorColor { get; set; }

        public SolidColorBrush NameInputColor
        {
            get { return _nameInputColor; }
            set
            {
                if (_nameInputColor != value)
                {
                    _nameInputColor = value;
                    OnPropertyChanged(nameof(NameInputColor));
                }
            }
        }

        public SolidColorBrush PriceInputColor
        {
            get { return _priceInputColor; }
            set
            {
                if (_priceInputColor != value)
                {
                    _priceInputColor = value;
                    OnPropertyChanged(nameof(PriceInputColor));
                }
            }
        }

        public SolidColorBrush QuantityInputColor
        {
            get { return _quantityInputColor; }
            set
            {
                if (_quantityInputColor != value)
                {
                    _quantityInputColor = value;
                    OnPropertyChanged(nameof(QuantityInputColor));
                }
            }
        }

        public SolidColorBrush BookAuthorInputColor
        {
            get { return _bookAuthorInputColor; }
            set
            {
                if (_bookAuthorInputColor != value)
                {
                    _bookAuthorInputColor = value;
                    OnPropertyChanged(nameof(BookAuthorInputColor));
                }
            }
        }

        public SolidColorBrush BookGenreInputColor
        {
            get { return _bookGenreInputColor; }
            set
            {
                if (_bookGenreInputColor != value)
                {
                    _bookGenreInputColor = value;
                    OnPropertyChanged(nameof(BookGenreInputColor));
                }
            }
        }

        public SolidColorBrush BookFormatInputColor
        {
            get { return _bookFormatInputColor; }
            set
            {
                if (_bookFormatInputColor != value)
                {
                    _bookFormatInputColor = value;
                    OnPropertyChanged(nameof(BookFormatInputColor));
                }
            }
        }

        public SolidColorBrush BookLanguageInputColor
        {
            get { return _bookLanguageInputColor; }
            set
            {
                if (_bookLanguageInputColor != value)
                {
                    _bookLanguageInputColor = value;
                    OnPropertyChanged(nameof(BookLanguageInputColor));
                }
            }
        }

        public SolidColorBrush GamePlatformInputColor
        {
            get { return _gamePlatformInputColor; }
            set
            {
                if (_gamePlatformInputColor != value)
                {
                    _gamePlatformInputColor = value;
                    OnPropertyChanged(nameof(GamePlatformInputColor));
                }
            }
        }

        public SolidColorBrush MovieFormatInputColor
        {
            get { return _movieFormatInputColor; }
            set
            {
                if (_movieFormatInputColor != value)
                {
                    _movieFormatInputColor = value;
                    OnPropertyChanged(nameof(MovieFormatInputColor));
                }
            }
        }

        public SolidColorBrush MovieDurationInputColor
        {
            get { return _movieDurationInputColor; }
            set
            {
                if (_movieDurationInputColor != value)
                {
                    _movieDurationInputColor = value;
                    OnPropertyChanged(nameof(MovieDurationInputColor));
                }
            }
        }

        internal void ResetColors()
        {
            NameInputColor = new SolidColorBrush(StandardColor);
            PriceInputColor = new SolidColorBrush(StandardColor);
            QuantityInputColor = new SolidColorBrush(StandardColor);
            BookAuthorInputColor = new SolidColorBrush(StandardColor);
            BookGenreInputColor = new SolidColorBrush(StandardColor);
            BookFormatInputColor = new SolidColorBrush(StandardColor);
            BookLanguageInputColor = new SolidColorBrush(StandardColor);
            GamePlatformInputColor = new SolidColorBrush(StandardColor);
            MovieFormatInputColor = new SolidColorBrush(StandardColor);
            MovieDurationInputColor = new SolidColorBrush(StandardColor);
        }
    }
}
