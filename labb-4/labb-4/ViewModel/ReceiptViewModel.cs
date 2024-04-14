using labb_4.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage.Streams;
using Windows.Storage;

namespace labb_4.ViewModel
{
    internal class ReceiptViewModel : ViewModelBase
    {
        private VisibilityViewModel _visibilityViewModel;

        public ReceiptViewModel(VisibilityViewModel visibilityViewModel)
        {
            _visibilityViewModel = visibilityViewModel;
        }

        public async Task PrintReceipt(List<Product> products)
        {
            StringBuilder receiptBuilder = new StringBuilder();
            receiptBuilder.AppendLine("__________                       .__          __   ");
            receiptBuilder.AppendLine("\\______   \\  ____   ____   ____  |__|______ _/  |_");
            receiptBuilder.AppendLine(" |       _/_/ __ \\_/ ___\\_/ __ \\ |  |\\____ \\\\   __\\ ");
            receiptBuilder.AppendLine(" |    |   \\\\  ___/\\  \\___\\  ___/ |  ||  |_> >|  |  ");
            receiptBuilder.AppendLine(" |____|_  / \\___  >\\___  >\\___  >|__||   __/ |__|  ");
            receiptBuilder.AppendLine("        \\/      \\/     \\/     \\/     |__| ");
            receiptBuilder.AppendLine();
            foreach (var product in products)
            {
                receiptBuilder.AppendLine($"Product: {product.Name}, Price: {product.Price}, Quantity: {product.Quantity}, PID: { product.PID}");
            }
            string receiptText = receiptBuilder.ToString();

            StorageFile receiptFile = await SaveReceiptToFile(receiptText);

            await PrintFile(receiptFile);
        }

        private async Task<StorageFile> SaveReceiptToFile(string receiptText)
        {
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            StorageFile file = await folder.CreateFileAsync("Receipt.txt", CreationCollisionOption.ReplaceExisting);

            using (IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.ReadWrite))
            {
                using (DataWriter writer = new DataWriter(stream))
                {
                    writer.WriteString(receiptText);
                    await writer.StoreAsync();
                }
            }

            return file;
        }

        private async Task PrintFile(StorageFile file)
        {
            if (file != null)
            {
                try
                {
                    var options = new Windows.System.LauncherOptions();
                    options.DisplayApplicationPicker = true;
                    bool success = await Windows.System.Launcher.LaunchFileAsync(file, options);

                    if (success)
                    {
                        _visibilityViewModel.IsPrintDialogVisible = false;
                    } else
                    {
                        _visibilityViewModel.IsPrintDialogVisible = false;
                        Console.WriteLine("File couldn't be opened.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }
    }
}
