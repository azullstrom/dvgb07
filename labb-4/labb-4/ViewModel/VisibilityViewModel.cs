using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Haptics;
using Windows.UI.Xaml;

namespace labb_4.ViewModel
{
    internal class VisibilityViewModel : ViewModelBase
    {
        private Visibility _isReturnProductVisible, _isFilterDialogVisible, _isAddProductVisible, _isAddDeliveryVisible, _isTransparentBackgroundVisible, _isStorageViewVisible, _isCashViewVisible, _isBookTemplateVisible, _isGameTemplateVisible, _isMovieTemplateVisible, _isUpdateQtyTemplateVisible;
        private bool _isPrintDialogVisible, _isEditCartEnabled, _isDecreaseQtyDialogVisible, _isIncreaseQtyDialogVisible, _isRemoveProductDialogVisible, _isAddButtonEnabled, _isEditProductEnabled;

        public VisibilityViewModel() 
        {
            _isStorageViewVisible = Visibility.Collapsed;
            _isCashViewVisible = Visibility.Visible;
            _isBookTemplateVisible = Visibility.Collapsed;
            _isGameTemplateVisible = Visibility.Collapsed;
            _isMovieTemplateVisible = Visibility.Collapsed;
            _isTransparentBackgroundVisible = Visibility.Collapsed;
            _isAddDeliveryVisible = Visibility.Collapsed;
            _isAddProductVisible = Visibility.Collapsed;
            _isFilterDialogVisible = Visibility.Collapsed;
            _isReturnProductVisible = Visibility.Collapsed;
            _isPrintDialogVisible = false;
            _isEditCartEnabled = false;
            _isRemoveProductDialogVisible = false;
            _isIncreaseQtyDialogVisible = false;
            _isAddButtonEnabled = false;
        }

        public bool IsAddButtonEnabled
        {
            get { return _isAddButtonEnabled; }
            set
            {
                if (_isAddButtonEnabled != value)
                {
                    _isAddButtonEnabled = value;
                    OnPropertyChanged(nameof(IsAddButtonEnabled));
                }
            }
        }
        public bool IsEditCartEnabled
        {
            get { return _isEditCartEnabled; }
            set
            {
                if (_isEditCartEnabled != value)
                {
                    _isEditCartEnabled = value;
                    OnPropertyChanged(nameof(IsEditCartEnabled));
                }
            }
        }
        public bool IsEditProductEnabled
        {
            get { return _isEditProductEnabled; }
            set
            {
                if (_isEditProductEnabled != value)
                {
                    _isEditProductEnabled = value;
                    OnPropertyChanged(nameof(IsEditProductEnabled));
                }
            }
        }

        
        public bool IsIncreaseQtyDialogVisible
        {
            get { return _isIncreaseQtyDialogVisible; }
            set
            {
                if (_isIncreaseQtyDialogVisible != value)
                {
                    _isIncreaseQtyDialogVisible = value;
                    OnPropertyChanged(nameof(IsIncreaseQtyDialogVisible));

                    if (value == true)
                    {
                        IsTransparentBackgroundVisible = Visibility.Visible;
                    }
                    else
                    {
                        IsTransparentBackgroundVisible = Visibility.Collapsed;
                    }
                }
            }
        }

        

        public bool IsDecreaseQtyDialogVisible
        {
            get { return _isDecreaseQtyDialogVisible; }
            set
            {
                if (_isDecreaseQtyDialogVisible != value)
                {
                    _isDecreaseQtyDialogVisible = value;
                    OnPropertyChanged(nameof(IsDecreaseQtyDialogVisible));

                    if (value == true)
                    {
                        IsTransparentBackgroundVisible = Visibility.Visible;
                    }
                    else
                    {
                        IsTransparentBackgroundVisible = Visibility.Collapsed;
                    }
                }
            }
        }
        public bool IsRemoveProductDialogVisible
        {
            get { return _isRemoveProductDialogVisible; }
            set
            {
                if (_isRemoveProductDialogVisible != value)
                {
                    _isRemoveProductDialogVisible = value;
                    OnPropertyChanged(nameof(IsRemoveProductDialogVisible));

                    if (value == true)
                    {
                        IsTransparentBackgroundVisible = Visibility.Visible;
                    }
                    else
                    {
                        IsTransparentBackgroundVisible = Visibility.Collapsed;
                    }
                }
            }
        }

        public bool IsPrintDialogVisible
        {
            get { return _isPrintDialogVisible; }
            set
            {
                if (_isPrintDialogVisible != value)
                {
                    _isPrintDialogVisible = value;
                    OnPropertyChanged(nameof(IsPrintDialogVisible));

                    if (value == true)
                    {
                        IsTransparentBackgroundVisible = Visibility.Visible;
                    }
                    else
                    {
                        IsTransparentBackgroundVisible = Visibility.Collapsed;
                    }
                }
            }
        }

        public Visibility IsFilterDialogVisible
        {
            get { return _isFilterDialogVisible; }
            set
            {
                if (_isFilterDialogVisible != value)
                {
                    _isFilterDialogVisible = value;
                    OnPropertyChanged(nameof(IsFilterDialogVisible));
                }
            }
        }

        public Visibility IsAddProductVisible
        {
            get { return _isAddProductVisible; }
            set
            {
                if (_isAddProductVisible != value)
                {
                    _isAddProductVisible = value;
                    OnPropertyChanged(nameof(IsAddProductVisible));
                }
            }
        }

        public Visibility IsReturnProductVisible
        {
            get { return _isReturnProductVisible; }
            set
            {
                if (_isReturnProductVisible != value)
                {
                    _isReturnProductVisible = value;
                    OnPropertyChanged(nameof(IsReturnProductVisible));
                }
            }
        }

        public Visibility IsAddDeliveryVisible
        {
            get { return _isAddDeliveryVisible; }
            set
            {
                if (_isAddDeliveryVisible != value)
                {
                    _isAddDeliveryVisible = value;
                    OnPropertyChanged(nameof(IsAddDeliveryVisible));
                }
            }
        }
        public Visibility IsStorageViewVisible
        {
            get { return _isStorageViewVisible; }
            set
            {
                if (_isStorageViewVisible != value)
                {
                    _isStorageViewVisible = value;
                    OnPropertyChanged(nameof(IsStorageViewVisible));
                }
            }
        }
        public Visibility IsCashViewVisible
        {
            get { return _isCashViewVisible; }
            set
            {
                if (_isCashViewVisible != value)
                {
                    _isCashViewVisible = value;
                    OnPropertyChanged(nameof(IsCashViewVisible));
                }
            }
        }
        public Visibility IsBookTemplateVisible
        {
            get { return _isBookTemplateVisible; }
            set
            {
                if (_isBookTemplateVisible != value)
                {
                    _isBookTemplateVisible = value;
                    OnPropertyChanged(nameof(IsBookTemplateVisible));
                }
            }
        }
        public Visibility IsGameTemplateVisible
        {
            get { return _isGameTemplateVisible; }
            set
            {
                if (_isGameTemplateVisible != value)
                {
                    _isGameTemplateVisible = value;
                    OnPropertyChanged(nameof(IsGameTemplateVisible));
                }
            }
        }
        public Visibility IsMovieTemplateVisible
        {
            get { return _isMovieTemplateVisible; }
            set
            {
                if (_isMovieTemplateVisible != value)
                {
                    _isMovieTemplateVisible = value;
                    OnPropertyChanged(nameof(IsMovieTemplateVisible));
                }
            }
        }
        public Visibility IsTransparentBackgroundVisible
        {
            get { return _isTransparentBackgroundVisible; }
            set
            {
                if (_isTransparentBackgroundVisible != value)
                {
                    _isTransparentBackgroundVisible = value;
                    OnPropertyChanged(nameof(IsTransparentBackgroundVisible));
                }
            }
        }

        
    }
}
