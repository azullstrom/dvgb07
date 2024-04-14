using labb_4.Database;
using labb_4.Model;
using labb_4.View;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

// Fått mycket hjälp att implementera MVVM pattern från dessa källor: https://www.youtube.com/watch?v=DhSMx_MVgiQ

namespace labb_4.ViewModel
{
    internal class MainViewModel : ViewModelBase
    {
        private ProductModel _productModel;
        private CartModel _cartModel;
        private DeliveryModel _deliveryModel;
        private int pid;
        

        public MainViewModel(ProductModel productModel)
        {
            _productModel = productModel;
            _cartModel = new CartModel(_productModel.Products);
            ColorViewModel = new ColorViewModel();
            VisibilityViewModel = new VisibilityViewModel();
            ProductViewModel = new ProductViewModel();
            SearchProductViewModel = new SearchProductViewModel();
            SelectionViewModel = new SelectionViewModel(VisibilityViewModel, ColorViewModel);
            ReceiptViewModel = new ReceiptViewModel(VisibilityViewModel);
            ListViewModel = new ListViewModel();
            _deliveryModel = new DeliveryModel(ListViewModel);
            pid = GetHighestProductId() + 1;
            LoadProducts(_productModel.Products);
        }

// ViewModels

        public ColorViewModel ColorViewModel { get; }

        public VisibilityViewModel VisibilityViewModel { get; }

        public ProductViewModel ProductViewModel { get; }

        public SelectionViewModel SelectionViewModel { get; }

        public ListViewModel ListViewModel { get; }

        public ReceiptViewModel ReceiptViewModel { get; }

        public SearchProductViewModel SearchProductViewModel { get; }
        

// Commands and Functions

        public ICommand ToggleViewCommand => new DelegateCommand(ToggleView);
        private void ToggleView()
        {
            SelectionViewModel.SelectedBook = null;
            SelectionViewModel.SelectedMovie = null;
            SelectionViewModel.SelectedGame = null;
            VisibilityViewModel.IsEditProductEnabled = false;
            VisibilityViewModel.IsStorageViewVisible = VisibilityViewModel.IsStorageViewVisible == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
            VisibilityViewModel.IsCashViewVisible = VisibilityViewModel.IsCashViewVisible == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
        }

        public ICommand ToggleAddDeliveryViewCommand => new DelegateCommand(ToggleAddDeliveryView);
        private void ToggleAddDeliveryView()
        {
            SelectionViewModel.SelectedBook = null;
            SelectionViewModel.SelectedMovie = null;
            SelectionViewModel.SelectedGame = null;
            VisibilityViewModel.IsEditProductEnabled = false;
            VisibilityViewModel.IsAddDeliveryVisible = VisibilityViewModel.IsAddDeliveryVisible == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
            VisibilityViewModel.IsStorageViewVisible = VisibilityViewModel.IsStorageViewVisible == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
            _deliveryModel.DeliveryProducts = new List<Product>();
            ListViewModel.DeliveryItems = new List<Product>();
            ResetProducts();
            ColorViewModel.ResetColors();
        }

        public ICommand ToggleAddProductViewCommand => new DelegateCommand(ToggleAddProductView);
        private void ToggleAddProductView()
        {
            SelectionViewModel.SelectedBook = null;
            SelectionViewModel.SelectedMovie = null;
            SelectionViewModel.SelectedGame = null;
            VisibilityViewModel.IsEditProductEnabled = false;
            VisibilityViewModel.IsAddProductVisible = VisibilityViewModel.IsAddProductVisible == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
            VisibilityViewModel.IsStorageViewVisible = VisibilityViewModel.IsStorageViewVisible == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
            ResetProducts();
            ColorViewModel.ResetColors();
        }

        public ICommand ToggleIncreaseQtyDialogCommand => new DelegateCommand(() => ToggleIncreaseQtyDialog());
        private void ToggleIncreaseQtyDialog()
        {
            VisibilityViewModel.IsIncreaseQtyDialogVisible = VisibilityViewModel.IsIncreaseQtyDialogVisible == false ? true : false;
        }

        public ICommand ToggleDecreaseQtyDialogCommand => new DelegateCommand(() => ToggleDecreaseQtyDialog());
        private void ToggleDecreaseQtyDialog()
        {
            VisibilityViewModel.IsDecreaseQtyDialogVisible = VisibilityViewModel.IsDecreaseQtyDialogVisible == false ? true : false;
        }

        public ICommand ToggleRemoveProductDialogCommand => new DelegateCommand(async () => await ToggleRemoveProductDialog());
        private async Task ToggleRemoveProductDialog()
        {
            if (SelectionViewModel.SelectedProduct.Quantity > 0)
            {
                VisibilityViewModel.IsRemoveProductDialogVisible = VisibilityViewModel.IsRemoveProductDialogVisible == false ? true : false;
            }
            else
            {
                await RemoveProductAsync();
            }
        }

        public ICommand ToggleFilterDialogCommand => new DelegateCommand(() => ToggleFilterDialog());
        private void ToggleFilterDialog()
        {
            VisibilityViewModel.IsFilterDialogVisible = VisibilityViewModel.IsFilterDialogVisible == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
            LoadProducts(_productModel.Products);
        }

        public ICommand ToggleReturnProductViewCommand => new DelegateCommand(() => ToggleReturnProductView());
        private void ToggleReturnProductView()
        {
            VisibilityViewModel.IsReturnProductVisible = VisibilityViewModel.IsReturnProductVisible == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
        }

        public ICommand SearchAndViewProductsCommand => new DelegateCommand(() => SearchAndViewProducts());
        private void SearchAndViewProducts()
        {
            LoadProducts(_productModel.Products);
        }

        public ICommand IncreaseCartQtyCommand => new DelegateCommand(() => IncreaseCartQty());
        private void IncreaseCartQty()
        {
            var temp = SelectionViewModel.SelectedCartItem;
            ListViewModel.CartItems = null;
            ListViewModel.CartItems = _cartModel.IncreaseQuantityAndReturnList(temp);
            SelectionViewModel.SelectedCartItem = null;
            VisibilityViewModel.IsEditCartEnabled = false;
        }

        public ICommand DecreaseCartQtyCommand => new DelegateCommand(() => DecreaseCartQty());
        private void DecreaseCartQty()
        {
            var temp = SelectionViewModel.SelectedCartItem;
            ListViewModel.CartItems = null;
            ListViewModel.CartItems = _cartModel.DecreaseQuantityAndReturnList(temp);
            SelectionViewModel.SelectedCartItem = null;
            VisibilityViewModel.IsEditCartEnabled = false;
        }

        public ICommand AddProductToCartCommand => new DelegateCommand(() => AddProductToCart());
        private void AddProductToCart()
        {
            ListViewModel.CartItems = null;
            Product product = new Product(SelectionViewModel.SelectedProduct);
            if (product.Quantity > 0)
            {
                ListViewModel.CartItems = _cartModel.AddToCartAndReturnList(product);
            }
        }

        public ICommand RemoveProductFromCartCommand => new DelegateCommand(() => RemoveProductFromCart());
        private void RemoveProductFromCart()
        {
            var temp = _cartModel.RemoveFromCartAndReturnList(SelectionViewModel.SelectedCartItem);
            ListViewModel.CartItems = null;
            ListViewModel.CartItems = temp;
            SelectionViewModel.SelectedCartItem = null;
            VisibilityViewModel.IsEditCartEnabled = false;
        }

        public ICommand SubmitCartOrderCommand => new DelegateCommand(async () => await SubmitCartOrder());
        private async Task SubmitCartOrder()
        {
            if (ListViewModel.CartItems != null)
            {
                foreach (var cartItem in ListViewModel.CartItems)
                {
                    Product product = _productModel.Products.FirstOrDefault(p => p.Name == cartItem.Name);
                    await _productModel.UpdateQtyProductInDatabaseAsync(product, -cartItem.Quantity);
                }
                SelectionViewModel.SelectedCartItem = null;
                VisibilityViewModel.IsEditCartEnabled = false;
                SelectionViewModel.SelectedBook = null;
                SelectionViewModel.SelectedMovie = null;
                SelectionViewModel.SelectedGame = null;
                VisibilityViewModel.IsEditProductEnabled = false;
                LoadProducts(_productModel.Products);
                _cartModel.MainProducts = _productModel.Products;
                VisibilityViewModel.IsPrintDialogVisible = true;
                ShowToastNotification("Succé!", "Ordern lyckades.");
            } else
            {
                ShowToastNotification("Felmeddelande", "Ordern misslyckades.");
            }
        }

        public ICommand PrintReceiptCommand => new DelegateCommand(async () => await PrintReceipt());
        private async Task PrintReceipt()
        {
            await ReceiptViewModel.PrintReceipt(ListViewModel.CartItems);
            _cartModel.CartProducts = new List<Product>();
            ListViewModel.CartItems = new List<Product>();
        }

        public ICommand SubmitDeliveryOrderCommand => new DelegateCommand(async () => await SubmitDeliveryOrder());
        private async Task SubmitDeliveryOrder()
        {
            var oldProducts = _productModel.Products.ToList();
            if (ListViewModel.DeliveryItems != null)
            {
                await _productModel.AddDeliveryToDatabaseAsync(ListViewModel.DeliveryItems);
                LoadProducts(_productModel.Products);
                _cartModel.MainProducts = _productModel.Products;
                ListViewModel.DeliveryItems = new List<Product>();
                _deliveryModel.DeliveryProducts = new List<Product>();
                ResetProducts();
                ColorViewModel.ResetColors();
                if (!oldProducts.SequenceEqual(_productModel.Products))
                {
                    ShowToastNotification("Succé!", "Produkterna lades till i systemet.");
                }
                else
                {
                    ShowToastNotification("Felmeddelande", "Produkterna lades inte till i systemet.");
                }
            }
            else
            {
                ShowToastNotification("Felmeddelande", "Produkterna lades inte till i systemet.");
            }
        }

        public ICommand AddDeliveryProductCommand => new DelegateCommand(() => AddDeliveryProduct());
        private void AddDeliveryProduct()
        {
            Product product = null;
            List<Book> existingBooks = _productModel.Products.OfType<Book>().ToList();
            List<Game> existingGames = _productModel.Products.OfType<Game>().ToList();
            List<Movie> existingMovies = _productModel.Products.OfType<Movie>().ToList();
            var existingBook = existingBooks.FirstOrDefault(book => book.Name == ProductViewModel.NewName);
            var existingGame = existingGames.FirstOrDefault(game => game.Name == ProductViewModel.NewName);
            var existingMovie = existingMovies.FirstOrDefault(movie => movie.Name == ProductViewModel.NewName);


            if (IsTextInputCorrect())
            {
                if (SelectionViewModel.SelectedProductType == "Bok" && (existingBook == null || existingBook is Book book))
                {
                    product = new Book(ProductViewModel.NewName, int.Parse(ProductViewModel.NewPrice), int.Parse(ProductViewModel.NewQuantity), pid++, ProductViewModel.NewBookAuthor, ProductViewModel.NewBookGenre, ProductViewModel.NewBookFormat, ProductViewModel.NewBookLanguage);
                }
                else if (SelectionViewModel.SelectedProductType == "Spel" && (existingGame == null || existingGame is Game game) )
                {
                    product = new Game(ProductViewModel.NewName, int.Parse(ProductViewModel.NewPrice), int.Parse(ProductViewModel.NewQuantity), pid++, ProductViewModel.NewGamePlatform);
                }
                else if (SelectionViewModel.SelectedProductType == "Film" && (existingMovie == null || existingMovie is Movie movie))
                {
                    string durationString = ProductViewModel.NewMovieDuration != null ? ProductViewModel.NewMovieDuration + " min" : ProductViewModel.NewMovieDuration;
                    product = new Movie(ProductViewModel.NewName, int.Parse(ProductViewModel.NewPrice), int.Parse(ProductViewModel.NewQuantity), pid++, ProductViewModel.NewMovieFormat, durationString);
                }

                if (product != null)
                {
                    _deliveryModel.AddToCart(product);
                    ResetProducts();
                    ColorViewModel.ResetColors();
                }
                else
                {
                    ShowToastNotification("Felmeddelande", "Produkterna lades inte till i leveranslistan.");
                }
            }
        }


        public ICommand ReturnProductCommand => new DelegateCommand(async () => await ReturnProductAsync());
        private async Task ReturnProductAsync()
        {
            Product product = null;
            List<Book> existingBooks = _productModel.Products.OfType<Book>().ToList();
            List<Game> existingGames = _productModel.Products.OfType<Game>().ToList();
            List<Movie> existingMovies = _productModel.Products.OfType<Movie>().ToList();
            var existingBook = existingBooks.FirstOrDefault(book => book.Name == ProductViewModel.NewName);
            var existingGame = existingGames.FirstOrDefault(game => game.Name == ProductViewModel.NewName);
            var existingMovie = existingMovies.FirstOrDefault(movie => movie.Name == ProductViewModel.NewName);

            int currentPid, price;
            string author = null, genre = null, format = null, language = null, platform = null, duration = null;

            if (existingBook != null)
            {
                price = existingBook.Price;
                currentPid = existingBook.PID;

                if (SelectionViewModel.SelectedProductType == "Bok" && existingBook is Book book)
                {
                    author = book.Author;
                    genre = book.Genre;
                    format = book.Format;
                    language = book.Language;
                    product = new Book(ProductViewModel.NewName, price, int.Parse(ProductViewModel.NewQuantity), currentPid, author, genre, format, language);
                }
            }
            else if (existingGame != null)
            {
                price = existingGame.Price;
                currentPid = existingGame.PID;

                if (SelectionViewModel.SelectedProductType == "Spel" && existingGame is Game game)
                {
                    platform = game.Platform;
                    product = new Game(ProductViewModel.NewName, price, int.Parse(ProductViewModel.NewQuantity), currentPid, platform);
                }
            }
            else if (existingMovie != null)
            {
                price = existingMovie.Price;
                currentPid = existingMovie.PID;

                if (SelectionViewModel.SelectedProductType == "Film" && existingMovie is Movie movie)
                {
                    format = movie.Format;
                    duration = movie.Time;
                    product = new Movie(ProductViewModel.NewName, price, int.Parse(ProductViewModel.NewQuantity), currentPid, format, duration);
                }
            }

            var oldProducts = _productModel.Products.ToList();
            if (product != null)
            {
                await _productModel.ReturnProductToDatabaseAsync(product);
                LoadProducts(_productModel.Products);
                _cartModel.MainProducts = _productModel.Products;
                ResetProducts();
                ColorViewModel.ResetColors();
                if (!oldProducts.SequenceEqual(_productModel.Products))
                {
                    ShowToastNotification("Succé!", "Produkten returnerades och lades till i systemet.");
                }
                else
                {
                    ShowToastNotification("Felmeddelande", "Produkten returnerades inte och lades inte till i systemet.");
                }
            }
            else
            {
                ShowToastNotification("Felmeddelande", "Produkten returnerades inte och lades inte till i systemet.");
            }
        }


        public ICommand AddProductCommand => new DelegateCommand(async () => await AddProductAsync());
        private async Task AddProductAsync()
        {
            Product product = null;
            List<Book> existingBooks = _productModel.Products.OfType<Book>().ToList();
            List<Game> existingGames = _productModel.Products.OfType<Game>().ToList();
            List<Movie> existingMovies = _productModel.Products.OfType<Movie>().ToList();
            var existingBook = existingBooks.FirstOrDefault(book => book.Name == ProductViewModel.NewName);
            var existingGame = existingGames.FirstOrDefault(game => game.Name == ProductViewModel.NewName);
            var existingMovie = existingMovies.FirstOrDefault(movie => movie.Name == ProductViewModel.NewName);

            if (IsTextInputCorrect())
            {
                if (SelectionViewModel.SelectedProductType == "Bok" && (existingBook == null || existingBook is Book book))
                {
                    product = new Book(ProductViewModel.NewName, int.Parse(ProductViewModel.NewPrice), int.Parse(ProductViewModel.NewQuantity), pid++, ProductViewModel.NewBookAuthor, ProductViewModel.NewBookGenre, ProductViewModel.NewBookFormat, ProductViewModel.NewBookLanguage);
                }
                else if (SelectionViewModel.SelectedProductType == "Spel" && (existingGame == null || existingGame is Game game))
                {
                    product = new Game(ProductViewModel.NewName, int.Parse(ProductViewModel.NewPrice), int.Parse(ProductViewModel.NewQuantity), pid++, ProductViewModel.NewGamePlatform);
                }
                else if (SelectionViewModel.SelectedProductType == "Film" && (existingMovie == null || existingMovie is Movie movie))
                {
                    string durationString = ProductViewModel.NewMovieDuration != null ? ProductViewModel.NewMovieDuration + " min" : ProductViewModel.NewMovieDuration;
                    product = new Movie(ProductViewModel.NewName, int.Parse(ProductViewModel.NewPrice), int.Parse(ProductViewModel.NewQuantity), pid++, ProductViewModel.NewMovieFormat, durationString);
                }

                var oldProducts = _productModel.Products.ToList();
                if (product != null)
                {
                    await _productModel.AddProductToDatabaseAsync(product);
                    LoadProducts(_productModel.Products);
                    _cartModel.MainProducts = _productModel.Products;
                    ResetProducts();
                    ColorViewModel.ResetColors();
                    if (!oldProducts.SequenceEqual(_productModel.Products))
                    {
                        ShowToastNotification("Succé!", "Produkten lades till i systemet.");
                    }
                    else
                    {
                        ShowToastNotification("Felmeddelande", "Produkten lades inte till i systemet.");
                    }
                }
                else
                {
                    ShowToastNotification("Felmeddelande", "Produkten lades inte till i systemet.");
                }
            } 
        }

        public ICommand DecreaseQuantityCommand => new DelegateCommand(async () => await DecreaseQuantity());
        private async Task DecreaseQuantity()
        {
            if (SelectionViewModel.SelectedProduct != null)
            {
                if (int.TryParse(ProductViewModel.ProductQuantityTextBox, out int qtyToRemove) && SelectionViewModel.SelectedProduct.Quantity - qtyToRemove >= 0 && qtyToRemove > 0)
                {
                    var prevProduct = SelectionViewModel.SelectedProduct;
                    await _productModel.UpdateQtyProductInDatabaseAsync(SelectionViewModel.SelectedProduct, -qtyToRemove);
                    LoadProducts(_productModel.Products);
                    var updatedProduct = _productModel.Products.FirstOrDefault(p => p.Name == prevProduct.Name);
                    int updatedQty = updatedProduct.Quantity;

                    Product existingCartProduct = _cartModel.FindProduct(prevProduct);
                    if (existingCartProduct != null)
                    {
                        if (existingCartProduct.Quantity > updatedQty)
                        {
                            ListViewModel.CartItems = null;
                            ListViewModel.CartItems = _cartModel.SetCartItemQuantityAndReturnList(existingCartProduct, updatedQty);
                        }
                    }

                    ResetProducts();
                    VisibilityViewModel.IsEditProductEnabled = false;
                    ShowToastNotification("Succé!", $"Produktkvantiteten minskades med {qtyToRemove}.");
                    _cartModel.MainProducts = _productModel.Products;
                    CancelUpdateQtyProduct();
                }
            }
        }

        public ICommand IncreaseQuantityCommand => new DelegateCommand(async () => await IncreaseQuantity());
        private async Task IncreaseQuantity()
        {
            if (SelectionViewModel.SelectedProduct != null)
            {
                if (int.TryParse(ProductViewModel.ProductQuantityTextBox, out int qtyToAdd) && qtyToAdd > 0)
                {
                    await _productModel.UpdateQtyProductInDatabaseAsync(SelectionViewModel.SelectedProduct, qtyToAdd);
                    ShowToastNotification("Succé!", $"Produktkvantiteten ökades med {qtyToAdd}.");
                    LoadProducts(_productModel.Products);
                    _cartModel.MainProducts = _productModel.Products;
                    ResetProducts();
                    VisibilityViewModel.IsEditProductEnabled = false;
                    CancelUpdateQtyProduct();
                }
            }
        }

        public ICommand UpdateQtyProductCommand => new DelegateCommand(async () => await UpdateQtyProductAsync());
        private async Task UpdateQtyProductAsync()
        {
            try
            {
                if (int.TryParse(ProductViewModel.ProductQuantityTextBox, out int quantity) && quantity > 0)
                {
                    if (SelectionViewModel.SelectedBook != null)
                    {
                        await _productModel.UpdateQtyProductInDatabaseAsync(SelectionViewModel.SelectedBook, quantity);
                        LoadProducts(_productModel.Products);
                    }
                    else if (SelectionViewModel.SelectedGame != null)
                    {
                        await _productModel.UpdateQtyProductInDatabaseAsync(SelectionViewModel.SelectedGame, quantity);
                        LoadProducts(_productModel.Products);
                    }
                    else if (SelectionViewModel.SelectedMovie != null)
                    {
                        await _productModel.UpdateQtyProductInDatabaseAsync(SelectionViewModel.SelectedMovie, quantity);
                        LoadProducts(_productModel.Products);
                    }
                    else
                    {
                        Debug.WriteLine("No product to update");
                    }
                    CancelUpdateQtyProduct();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error updating product: {e.Message}");
            }
        }


        public ICommand RemoveProductCommand => new DelegateCommand(async () => await RemoveProductAsync());
        private async Task RemoveProductAsync()
        {
            try
            {
                Product product = null;
                if (SelectionViewModel.SelectedBook != null)
                {
                    product = SelectionViewModel.SelectedBook;
                    await _productModel.RemoveProductFromDatabaseAsync(SelectionViewModel.SelectedBook);
                }
                else if (SelectionViewModel.SelectedGame != null)
                {
                    product = SelectionViewModel.SelectedGame;
                    await _productModel.RemoveProductFromDatabaseAsync(SelectionViewModel.SelectedGame);
                }
                else if (SelectionViewModel.SelectedMovie != null)
                {
                    product = SelectionViewModel.SelectedMovie;
                    await _productModel.RemoveProductFromDatabaseAsync(SelectionViewModel.SelectedMovie);
                }
                else
                {
                    Debug.WriteLine("No product to remove");
                }

                if (product != null)
                {
                    ListViewModel.CartItems = null;
                    ListViewModel.CartItems = _cartModel.RemoveFromCartAndReturnList(product);
                }
                ShowToastNotification("Succé!", "Produkten togs bort.");
                LoadProducts(_productModel.Products);
                _cartModel.MainProducts = _productModel.Products;
                ResetProducts();
                CancelRemoveProduct();
                VisibilityViewModel.IsEditProductEnabled = false;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error removing product: {e.Message}");
            }
        }

        public ICommand CancelRemoveProductCommand => new DelegateCommand(() => CancelRemoveProduct());
        private void CancelRemoveProduct()
        {
            VisibilityViewModel.IsRemoveProductDialogVisible = false;
        }

        public ICommand CancelPrintReceiptCommand => new DelegateCommand(() => CancelPrintReceipt());
        private void CancelPrintReceipt()
        {
            VisibilityViewModel.IsPrintDialogVisible = false;
            _cartModel.CartProducts = new List<Product>();
            ListViewModel.CartItems = new List<Product>();
        }

        public ICommand CancelUpdateQtyProductCommand => new DelegateCommand(() => CancelUpdateQtyProduct());
        private void CancelUpdateQtyProduct()
        {
            VisibilityViewModel.IsIncreaseQtyDialogVisible = false;
            VisibilityViewModel.IsDecreaseQtyDialogVisible = false;
            ProductViewModel.ProductQuantityTextBox = null;
        }

        private bool IsTextInputCorrect()
        {
            SolidColorBrush rightInputColor = new SolidColorBrush(ColorViewModel.StandardColor);
            SolidColorBrush wrongInputColor = new SolidColorBrush(ColorViewModel.ErrorColor);

            bool isNameValid = !string.IsNullOrWhiteSpace(ProductViewModel.NewName);
            bool isPriceValid = int.TryParse(ProductViewModel.NewPrice, out int price) && price >= 1;
            bool isQuantityValid = int.TryParse(ProductViewModel.NewQuantity, out int quantity) && quantity >= 1;
            ColorViewModel.NameInputColor = isNameValid == true ? rightInputColor : wrongInputColor;
            ColorViewModel.PriceInputColor = isPriceValid == true ? rightInputColor : wrongInputColor;
            ColorViewModel.QuantityInputColor = isQuantityValid == true ? rightInputColor : wrongInputColor;

            if (SelectionViewModel.SelectedProductType == "Bok")
            {
                bool isAuthorValid = IsNullOrEmptyOrNoWhitespace(ProductViewModel.NewBookAuthor) || (Regex.IsMatch(ProductViewModel.NewBookAuthor, @"^[a-zA-Z]+(\s[a-zA-Z.]+)*$"));
                bool isGenreValid = IsNullOrEmptyOrNoWhitespace(ProductViewModel.NewBookGenre) || (Regex.IsMatch(ProductViewModel.NewBookGenre, @"^[a-zA-Z]+$"));
                bool isFormatValid = IsNullOrEmptyOrNoWhitespace(ProductViewModel.NewBookFormat) || (Regex.IsMatch(ProductViewModel.NewBookFormat, @"^[a-zA-Z]+$"));
                bool isLanguageValid = IsNullOrEmptyOrNoWhitespace(ProductViewModel.NewBookLanguage) || (Regex.IsMatch(ProductViewModel.NewBookLanguage, @"^[a-zA-Z]+$"));
                ColorViewModel.BookAuthorInputColor = isAuthorValid == true ? rightInputColor : wrongInputColor;
                ColorViewModel.BookGenreInputColor = isGenreValid == true ? rightInputColor : wrongInputColor;
                ColorViewModel.BookFormatInputColor = isFormatValid == true ? rightInputColor : wrongInputColor;
                ColorViewModel.BookLanguageInputColor = isLanguageValid == true ? rightInputColor : wrongInputColor;
                bool isTextInputCorrect = isNameValid && isPriceValid && isQuantityValid && isAuthorValid && isGenreValid && isFormatValid && isLanguageValid;
                return isTextInputCorrect;
            }
            else if (SelectionViewModel.SelectedProductType == "Spel")
            {
                bool isPlatformValid = IsNullOrEmptyOrNoWhitespace(ProductViewModel.NewGamePlatform) || Regex.IsMatch(ProductViewModel.NewGamePlatform, @"^[a-zA-Z]+(\s[a-zA-Z]+)*$");
                ColorViewModel.GamePlatformInputColor = isPlatformValid == true ? rightInputColor : wrongInputColor;
                bool isTextInputCorrect = isNameValid && isPriceValid && isQuantityValid && isPlatformValid;
                return isTextInputCorrect;
            }
            else if (SelectionViewModel.SelectedProductType == "Film")
            {
                bool isFormatValid = IsNullOrEmptyOrNoWhitespace(ProductViewModel.NewMovieFormat) || Regex.IsMatch(ProductViewModel.NewMovieFormat, @"^[a-zA-Z]+$");
                bool isDurationValid = IsNullOrEmptyOrNoWhitespace(ProductViewModel.NewMovieDuration) || int.TryParse(ProductViewModel.NewMovieDuration, out int duration) && duration >= 1;
                ColorViewModel.MovieFormatInputColor = isFormatValid == true ? rightInputColor : wrongInputColor;
                ColorViewModel.MovieDurationInputColor = isDurationValid == true ? rightInputColor : wrongInputColor;
                bool isTextInputCorrect = isNameValid && isPriceValid && isQuantityValid && isFormatValid && isDurationValid;
                return isTextInputCorrect;
            }

            return false;
        }

        private bool IsNullOrEmptyOrNoWhitespace(string str)
        {
            return str == null || str == "" || !string.IsNullOrWhiteSpace(str);
        }

        private int GetHighestProductId()
        {
            int maxPID = 0;

            foreach (var product in _productModel.Products)
            {
                if (product.PID > maxPID)
                {
                    maxPID = product.PID;
                }
            }

            return maxPID;
        }

        private void LoadProducts(List<Product> products)
        {
            if (VisibilityViewModel.IsFilterDialogVisible == Visibility.Collapsed)
            {
                ListViewModel.Books = products.OfType<Book>().ToList();
                ListViewModel.Movies = products.OfType<Movie>().ToList();
                ListViewModel.Games = products.OfType<Game>().ToList();
            }
            else
            {
                List<Product> filteredProducts = FilterProducts(products, SearchProductViewModel.SearchNameBox, SearchProductViewModel.SearchQuantityBox);
                ListViewModel.Books = filteredProducts.OfType<Book>().ToList();
                ListViewModel.Movies = filteredProducts.OfType<Movie>().ToList();
                ListViewModel.Games = filteredProducts.OfType<Game>().ToList();
            }
        }

        private List<Product> FilterProducts(List<Product> products, string nameFilter, string quantityFilter)
        {
            var filteredProducts = new List<Product>(products);

            if (!string.IsNullOrEmpty(nameFilter))
            {
                filteredProducts = filteredProducts.Where(p => p.Name.Contains(nameFilter)).ToList();
            }

            if (!string.IsNullOrEmpty(quantityFilter))
            {
                int quantity;
                if (int.TryParse(quantityFilter, out quantity))
                {
                    filteredProducts = filteredProducts.Where(p => p.Quantity == quantity).ToList();
                }
            }

            return filteredProducts;
        }



        private void ResetProducts()
        {
            SelectionViewModel.SelectedProductType = null;
            ProductViewModel.NewName = null;
            ProductViewModel.NewPrice = null;
            ProductViewModel.NewQuantity = null;
            ProductViewModel.NewBookAuthor = null;
            ProductViewModel.NewBookFormat = null;
            ProductViewModel.NewBookGenre = null;
            ProductViewModel.NewBookLanguage = null;
            ProductViewModel.NewGamePlatform = null;
            ProductViewModel.NewMovieDuration = null;
            ProductViewModel.NewMovieFormat = null;
        }

        // https://stackoverflow.com/questions/37541923/how-to-create-informative-toast-notification-in-uwp-app
        public void ShowToastNotification(string title, string message)
        {
            ToastNotifier ToastNotifier = ToastNotificationManager.CreateToastNotifier();
            Windows.Data.Xml.Dom.XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);
            Windows.Data.Xml.Dom.XmlNodeList toastNodeList = toastXml.GetElementsByTagName("text");
            toastNodeList.Item(0).AppendChild(toastXml.CreateTextNode(title));
            toastNodeList.Item(1).AppendChild(toastXml.CreateTextNode(message));
            Windows.Data.Xml.Dom.IXmlNode toastNode = toastXml.SelectSingleNode("/toast");
            Windows.Data.Xml.Dom.XmlElement audio = toastXml.CreateElement("audio");
            audio.SetAttribute("src", "ms-winsoundevent:Notification.SMS");

            ToastNotification toast = new ToastNotification(toastXml);
            toast.ExpirationTime = DateTime.Now.AddSeconds(4);
            ToastNotifier.Show(toast);
        }

    }
}
