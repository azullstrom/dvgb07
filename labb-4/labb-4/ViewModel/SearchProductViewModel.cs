using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace labb_4.ViewModel
{
    internal class SearchProductViewModel : ViewModelBase
    {
        private string _searchNameBox, _searchQuantityBox, _searchArtistBox;

        public string SearchNameBox
        {
            get { return _searchNameBox; }
            set
            {
                _searchNameBox = value;
                OnPropertyChanged(nameof(SearchNameBox));
            }
        }
        public string SearchQuantityBox
        {
            get { return _searchQuantityBox; }
            set
            {
                _searchQuantityBox = value;
                OnPropertyChanged(nameof(SearchQuantityBox));
            }
        }
        public string SearchArtistBox
        {
            get { return _searchArtistBox; }
            set
            {
                _searchArtistBox = value;
                OnPropertyChanged(nameof(SearchArtistBox));
            }
        }
    }
}
