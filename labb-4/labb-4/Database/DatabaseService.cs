using labb_4.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml.Shapes;

namespace labb_4.Database
{
    public class DatabaseService
    {
        private StorageFile file;

        public DatabaseService()
        {
            Products = new List<Product>();
        }

        public List<Product> Products { get; set; }

        public async Task<List<Product>> ReadCSVFileToStringAsync()
        {
            try
            {
                var picker = new FileOpenPicker();
                picker.FileTypeFilter.Add(".csv");
                file = await picker.PickSingleFileAsync();

                if (file != null)
                {
                    string csvString = await FileIO.ReadTextAsync(file);
                    return ParseCSVStringToList(csvString);
                }
                else
                {
                    return new List<Product>();
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error reading CSV file: {e.Message}");
                return new List<Product>();
            }
        }

        // Tog hjälp av ChatGPT för strängparsingen. Fick såklart göra lite modifieringar för att få det att fungera.
        // Jag lyckades få till abstraktionen bra tyckte jag, då jag kan göra en lista av produkter för att generalisera koden.
        private List<Product> ParseCSVStringToList(string csvString)
        {

            if (csvString == null)
            {
                return new List<Product>();
            } else
            {
                Products.Clear();
            }

            // Dela upp strängen i CSV-filens olika sektioner. Böcker, Datorspel, Filmer.
            var sections = csvString.Split(new[] { "\r\n\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var section in sections)
            {
                // Plocka ut varje enskild rad i den nuvarande sektionen.
                var lines = section.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                // Skippa rubriken och attribut-rubriker och lägg in i en array.
                var headerLine = lines.First();
                lines = lines.Skip(1).ToArray();

                foreach (var line in lines)
                {
                    var fields = line.Split(',');

                    Product product = null;
                    if (headerLine.Contains("Böcker"))
                    {
                        product = new Book(
                            fields[0],
                            int.Parse(fields[1]),
                            int.Parse(fields[2]),
                            int.Parse(fields[3]),
                            fields[4],
                            fields[5],
                            fields[6],
                            fields[7]
                        );
                    }
                    else if (headerLine.Contains("Datorspel"))
                    {
                        product = new Game(
                            fields[0],
                            int.Parse(fields[1]),
                            int.Parse(fields[2]),
                            int.Parse(fields[3]),
                            fields[4]
                        );
                    }
                    else if (headerLine.Contains("Filmer"))
                    {
                        product = new Movie(
                            fields[0],
                            int.Parse(fields[1]),
                            int.Parse(fields[2]),
                            int.Parse(fields[3]),
                            fields[4],
                            fields[5]
                        );
                    }

                    if (product != null)
                        Products.Add(product);
                }
            }

            return Products;
        }

        public async Task<List<Product>> WriteProductToCSVAsync(Product product)
        {
            try
            {
                string newProductInfo = "";
                string existingContent = await FileIO.ReadTextAsync(file);
                int indexToAppend = -1;

                if (!ProductExists(product))
                {
                    if (product is Book book)
                    {
                        int newQuantity = book.Quantity + GetQuantityOfProductAsync(product);
                        newProductInfo = $"\r\n{book.Name},{book.Price},{newQuantity},{book.PID},{book.Author},{book.Genre},{book.Format},{book.Language}";
                        indexToAppend = existingContent.IndexOf("Böcker") + "Böcker".Length;
                    }
                    else if (product is Game game)
                    {
                        int newQuantity = game.Quantity + GetQuantityOfProductAsync(product);
                        newProductInfo = $"\r\n{game.Name},{game.Price},{newQuantity},{game.PID},{game.Platform}";
                        indexToAppend = existingContent.IndexOf("Datorspel") + "Datorspel".Length;
                    }
                    else if (product is Movie movie)
                    {
                        int newQuantity = movie.Quantity + GetQuantityOfProductAsync(product);
                        newProductInfo = $"\r\n{movie.Name},{movie.Price},{newQuantity},{movie.PID},{movie.Format},{movie.Time}";
                        indexToAppend = existingContent.IndexOf("Filmer") + "Filmer".Length;
                    }
                    string updatedContent = existingContent.Insert(indexToAppend, newProductInfo);
                    await FileIO.WriteTextAsync(file, updatedContent);
                    var updatedContentToList = ParseCSVStringToList(updatedContent);

                    return updatedContentToList;
                }
                else
                {
                    Product existingProduct = FindProduct(product);
                    existingContent = UpdateExistingProductQuantity(existingContent, product, existingProduct);
                    await FileIO.WriteTextAsync(file, existingContent);
                    var updatedContentToList = ParseCSVStringToList(existingContent);

                    return updatedContentToList;
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error writing product to CSV file: {e.Message}");
                return null;
            }
        }

        public async Task<List<Product>> RemoveProductFromCSVAsync(Product product)
        {
            try
            {
                string existingContent = await FileIO.ReadTextAsync(file);

                string productInfoToRemove = "";
                if (product is Book book)
                {
                    productInfoToRemove = $"\r\n{book.Name},{book.Price},{book.Quantity},{book.PID},{book.Author},{book.Genre},{book.Format},{book.Language}";
                }
                else if (product is Game game)
                {
                    productInfoToRemove = $"\r\n{game.Name},{game.Price},{game.Quantity},{game.PID},{game.Platform}";
                }
                else if (product is Movie movie)
                {
                    productInfoToRemove = $"\r\n{movie.Name},{movie.Price},{movie.Quantity},{movie.PID},{movie.Format},{movie.Time}";
                }

                string updatedContent = existingContent.Replace(productInfoToRemove, "");
                await FileIO.WriteTextAsync(file, updatedContent);
                var updatedContentToList = ParseCSVStringToList(updatedContent);

                return updatedContentToList;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error removing product from CSV file: {e.Message}");
                return null;
            }
        }

        public async Task<List<Product>> UpdateQtyProductInCSVAsync(Product product, int quantity)
        {
            try
            {
                string existingContent = await FileIO.ReadTextAsync(file);
                string productInfoToUpdate = "";
                string newProductInfo = "";
                int newQuantity = 0;

                if (product is Book book)
                {
                    productInfoToUpdate = $"\r\n{book.Name},{book.Price},{book.Quantity},{book.PID},{book.Author},{book.Genre},{book.Format},{book.Language}";
                    newQuantity = book.Quantity + quantity;
                    newProductInfo = $"\r\n{book.Name},{book.Price},{newQuantity},{book.PID},{book.Author},{book.Genre},{book.Format},{book.Language}";
                }
                else if (product is Game game)
                {
                    productInfoToUpdate = $"\r\n{game.Name},{game.Price},{game.Quantity},{game.PID},{game.Platform}";
                    newQuantity = game.Quantity + quantity;
                    newProductInfo = $"\r\n{game.Name},{game.Price},{newQuantity},{game.PID},{game.Platform}";
                }
                else if (product is Movie movie)
                {
                    productInfoToUpdate = $"\r\n{movie.Name},{movie.Price},{movie.Quantity},{movie.PID},{movie.Format},{movie.Time}";
                    newQuantity = movie.Quantity + quantity;
                    newProductInfo = $"\r\n{movie.Name},{movie.Price},{newQuantity},{movie.PID},{movie.Format},{movie.Time}";
                }

                string updatedContent = existingContent.Replace(productInfoToUpdate, newProductInfo);
                await FileIO.WriteTextAsync(file, updatedContent);
                var updatedContentToList = ParseCSVStringToList(updatedContent);

                return updatedContentToList;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error updating product quantity in CSV file: {e.Message}");
                return null;
            }
        }

        private bool ProductExists(Product product)
        {
            string productName = product.Name.Trim();

            if (product is Book book)
            {
                return Products.OfType<Book>().Any(b => string.Equals(b.Name.Trim(), productName, StringComparison.OrdinalIgnoreCase));
            }
            else if (product is Game game)
            {
                return Products.OfType<Game>().Any(g => string.Equals(g.Name.Trim(), productName, StringComparison.OrdinalIgnoreCase));
            }
            else if (product is Movie movie)
            {
                return Products.OfType<Movie>().Any(m => string.Equals(m.Name.Trim(), productName, StringComparison.OrdinalIgnoreCase));
            }

            return false;
        }



        public int GetQuantityOfProductAsync(Product product)
        {
            try
            {
                if (product is Book book)
                {
                    var existingBook = Products.OfType<Book>().FirstOrDefault(b => b.Name == book.Name);
                    if (existingBook != null)
                        return existingBook.Quantity; 
                }
                else if (product is Movie movie)
                {
                    var existingMovie = Products.OfType<Movie>().FirstOrDefault(m => m.Name == movie.Name);
                    if (existingMovie != null)
                        return existingMovie.Quantity; 
                }
                else if (product is Game game)
                {
                    var existingGame = Products.OfType<Game>().FirstOrDefault(g => g.Name == game.Name);
                    if (existingGame != null)
                        return existingGame.Quantity; 
                }

                return 0;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error getting quantity of product: {e.Message}");
                return 0;
            }
        }

        internal async Task<List<Product>> AddDeliveryToCSVAsync(List<Product> products)
        {
            try
            {
                string existingContent = await FileIO.ReadTextAsync(file);

                foreach (var product in products)
                {
                    string newProductInfo = "";
                    int indexToAppend = -1;

                    if (!ProductExists(product))
                    {
                        if (product is Book book)
                        {
                            newProductInfo = $"\r\n{book.Name},{book.Price},{book.Quantity},{book.PID},{book.Author},{book.Genre},{book.Format},{book.Language}";
                            indexToAppend = existingContent.IndexOf("Böcker") + "Böcker".Length;
                        }
                        else if (product is Game game)
                        {
                            newProductInfo = $"\r\n{game.Name},{game.Price},{game.Quantity},{game.PID},{game.Platform}";
                            indexToAppend = existingContent.IndexOf("Datorspel") + "Datorspel".Length;
                        }
                        else if (product is Movie movie)
                        {
                            newProductInfo = $"\r\n{movie.Name},{movie.Price},{movie.Quantity},{movie.PID},{movie.Format},{movie.Time}";
                            indexToAppend = existingContent.IndexOf("Filmer") + "Filmer".Length;
                        }

                        if (indexToAppend != -1)
                        {
                            existingContent = existingContent.Insert(indexToAppend, newProductInfo);
                        }
                    }
                    else
                    {
                        Product existingProduct = FindProduct(product);
                        existingContent = UpdateExistingProductQuantity(existingContent, product, existingProduct);
                    }
                }

                await FileIO.WriteTextAsync(file, existingContent);
                var updatedContentToList = ParseCSVStringToList(existingContent);

                return updatedContentToList;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error adding delivery to CSV file: {e.Message}");
                return null;
            }
        }

        private Product FindProduct(Product product)
        {
            try
            {
                string productName = product.Name.Trim();

                if (product is Book)
                {
                    var existingBook = Products.OfType<Book>().FirstOrDefault(b => string.Equals(b.Name.Trim(), productName, StringComparison.OrdinalIgnoreCase));
                    if (existingBook != null)
                        return existingBook;
                }
                else if (product is Movie)
                {
                    var existingMovie = Products.OfType<Movie>().FirstOrDefault(m => string.Equals(m.Name.Trim(), productName, StringComparison.OrdinalIgnoreCase));
                    if (existingMovie != null)
                        return existingMovie;
                }
                else if (product is Game)
                {
                    var existingGame = Products.OfType<Game>().FirstOrDefault(g => string.Equals(g.Name.Trim(), productName, StringComparison.OrdinalIgnoreCase));
                    if (existingGame != null)
                        return existingGame;
                }

                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error getting product: {e.Message}");
                return null;
            }
        }


        private string UpdateExistingProductQuantity(string existingContent, Product product, Product existingProduct)
        {
            string productInfoToUpdate = "";
            string newInfo = "";
            if (product is Book book)
            {
                Book existingBook = (Book)existingProduct;
                productInfoToUpdate = $"\r\n{existingBook.Name},{existingBook.Price},{existingBook.Quantity},{existingBook.PID},{existingBook.Author},{existingBook.Genre},{existingBook.Format},{existingBook.Language}";
                newInfo = $"\r\n{book.Name},{book.Price},{existingBook.Quantity + book.Quantity},{existingBook.PID},{book.Author},{book.Genre},{book.Format},{book.Language}";
            }
            else if (product is Game game)
            {
                Game existingGame = (Game)existingProduct;
                productInfoToUpdate = $"\r\n{existingGame.Name},{existingGame.Price},{existingGame.Quantity},{existingGame.PID},{existingGame.Platform}";
                newInfo = $"\r\n{game.Name},{game.Price},{existingGame.Quantity + game.Quantity},{existingGame.PID},{game.Platform}";
            }
            else if (product is Movie movie)
            {
                Movie existingMovie = (Movie)existingProduct;
                productInfoToUpdate = $"\r\n{existingMovie.Name},{existingMovie.Price},{existingMovie.Quantity},{existingMovie.PID},{existingMovie.Format},{existingMovie.Time}";
                newInfo = $"\r\n{movie.Name},{movie.Price},{existingMovie.Quantity + movie.Quantity},{existingMovie.PID},{movie.Format},{movie.Time}";
            }

            string updatedContent = existingContent.Replace(productInfoToUpdate, newInfo);
            return updatedContent;
        }

        internal async Task<List<Product>> ReturnProductToCSVAsync(Product product)
        {
            try
            {
                string existingContent = await FileIO.ReadTextAsync(file);

                if (ProductExists(product))
                {
                    Product existingProduct = FindProduct(product);
                    existingContent = UpdateExistingProductQuantity(existingContent, product, existingProduct);
                    await FileIO.WriteTextAsync(file, existingContent);
                    var updatedContentToList = ParseCSVStringToList(existingContent);

                    return updatedContentToList;
                }
                else
                {
                    var sameContentToList = ParseCSVStringToList(existingContent);
                    return sameContentToList;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine($"Error writing product to CSV file: {e.Message}");
                return null;
            }
        }

        internal async Task<List<Product>> UpdateCSVQtyAndPriceAsync(List<Product> products)
        {
            string updatedContent = "";

            foreach (Product product in products)
            {
                string existingContent = await FileIO.ReadTextAsync(file);
                var existingProduct = FindXmlProduct(product);
                var productInfoToUpdate = "";
                var newInfo = "";

                if (existingProduct != null)
                {
                    if (existingProduct is Book book)
                    {
                        Book existingBook = (Book)existingProduct;
                        productInfoToUpdate = $"\r\n{existingBook.Name},{existingBook.Price},{existingBook.Quantity},{existingBook.PID},{existingBook.Author},{existingBook.Genre},{existingBook.Format},{existingBook.Language}";
                        newInfo = $"\r\n{book.Name},{product.Price},{product.Quantity},{existingBook.PID},{book.Author},{book.Genre},{book.Format},{book.Language}";
                    }
                    else if (existingProduct is Game game)
                    {
                        Game existingGame = (Game)existingProduct;
                        productInfoToUpdate = $"\r\n{existingGame.Name},{existingGame.Price},{existingGame.Quantity},{existingGame.PID},{existingGame.Platform}";
                        newInfo = $"\r\n{game.Name},{product.Price},{product.Quantity},{existingGame.PID},{game.Platform}";
                    }
                    else if (existingProduct is Movie movie)
                    {
                        Movie existingMovie = (Movie)existingProduct;
                        productInfoToUpdate = $"\r\n{existingMovie.Name},{existingMovie.Price},{existingMovie.Quantity},{existingMovie.PID},{existingMovie.Format},{existingMovie.Time}";
                        newInfo = $"\r\n{movie.Name},{product.Price},{product.Quantity},{existingMovie.PID},{movie.Format},{movie.Time}";
                    }

                    updatedContent = existingContent.Replace(productInfoToUpdate, newInfo);
                    await FileIO.WriteTextAsync(file, updatedContent);
                }
            }
            var updatedContentToList = ParseCSVStringToList(updatedContent);

            return updatedContentToList;
        }

        private Product FindXmlProduct(Product product)
        {
            try
            {
                int pid = product.PID;

                var existingProduct = Products.FirstOrDefault(p => p.PID == pid);

                return existingProduct;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error getting product: {e.Message}");
                return null;
            }
        }
    }
}
