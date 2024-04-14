using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Diagnostics;
using Windows.Storage.Pickers;
using Windows.ApplicationModel.Background;
using static System.Net.Mime.MediaTypeNames;
using Windows.Storage;
using Windows.UI.Popups;
using System.Threading.Tasks;
using Windows.UI.Core.Preview;
using System.Reflection;
using Windows.ApplicationModel.DataTransfer;
using Windows.System;
using Windows.UI.Core;
using static System.Net.WebRequestMethods;

namespace Labb_3
{
    public sealed partial class MainPage : Page
    {
        private StorageFile file;
        private bool newFile, openFile, unsavedChanges, saveAs, dragAndDrop;
        private int characterCountNoSpace, characterCountSpace;
        private string fileName, mainText, originalMainText, previousOriginalMainText;

        public MainPage()
        {
            this.InitializeComponent();

            // https://stackoverflow.com/questions/62910280/is-it-possible-to-pop-up-my-dialog-box-when-click-the-close-icon-on-the-upper-ri
            Windows.UI.Core.Preview.SystemNavigationManagerPreview.GetForCurrentView().CloseRequested += async (sender, args) =>
            {
                args.Handled = true;
                await EndProgramAsync();
            };

            newFile = true;
            openFile = false;
            unsavedChanges = false;
            originalMainText = "";
            previousOriginalMainText = "init_state";
            saveAs = false;
            dragAndDrop = false;
            file = null;
            fileName = "namnlös.txt";
        }

        private async Task EndProgramAsync()
        {
            if (unsavedChanges)
            {
                MessageDialog msg = new MessageDialog("Vill du spara dokumentet?");
                msg.Commands.Add(new UICommand("Ja", async x =>
                {
                    await SaveFileAsync();
                    App.Current.Exit();
                }));
                msg.Commands.Add(new UICommand("Nej", x => App.Current.Exit()));
                msg.Commands.Add(new UICommand("Avbryt"));
                await msg.ShowAsync();
            } else
            {
                App.Current.Exit();
            }
        }

        private async void NewFile_Click(object sender, RoutedEventArgs e)
        {
            await NewFileAsync();
        }

        private async Task NewFileAsync()
        {
            bool continueExecution = true;

            if (unsavedChanges)
            {
                MessageDialog msg = new MessageDialog("Vill du spara dokumentet?");
                msg.Commands.Add(new UICommand("Ja", async x => await SaveFileAsync()));
                msg.Commands.Add(new UICommand("Nej"));
                msg.Commands.Add(new UICommand("Avbryt", x => { continueExecution = false; }));
                await msg.ShowAsync();
            }

            if (continueExecution)
            {
                fileName = "namnlös.txt";
                file = null;
                previousOriginalMainText = originalMainText;
                Main_TextBox.Text = originalMainText = "";
                newFile = true;
                openFile = true;
                unsavedChanges = false;
                dragAndDrop = false;
                DragAndDrop_TextBlock.Text = "Spara fil inaktiverat: Nej";
            }
        }

        private async void SaveFile_Click(object sender, RoutedEventArgs e)
        {
            await SaveFileAsync();
        }

        private async Task SaveFileAsync()
        {
            mainText = Main_TextBox.Text;

            if (newFile || dragAndDrop)
            {
                await SaveAsAsync();
            } else
            {
                if (file != null)
                {
                    await FileIO.WriteTextAsync(file, mainText);
                    FileName_TextBlock.Text = fileName;
                    unsavedChanges = false;
                    previousOriginalMainText = originalMainText;
                    originalMainText = mainText;
                }
            }
        }

        private async void SaveAs_Click(object sender, RoutedEventArgs e)
        {
            await SaveAsAsync();
        }

        private async Task SaveAsAsync()
        {
            mainText = Main_TextBox.Text;
            saveAs = true;

            var fileSavePicker = new FileSavePicker();
            fileSavePicker.FileTypeChoices.Add("Plain Text", new List<string>() { ".txt" });
            var result = await fileSavePicker.PickSaveFileAsync();

            if (result != null)
            {
                await FileIO.WriteTextAsync(result, mainText);
                unsavedChanges = false;
                FileName_TextBlock.Text = fileName = result.Name;
                newFile = false;
                file = result;
                previousOriginalMainText = originalMainText;
                originalMainText = mainText;
            }
            saveAs = false;
        }

        private async void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            await OpenFileAsync();
        }

        private async Task OpenFileAsync()
        {


            if (unsavedChanges)
            {
                MessageDialog msg = new MessageDialog("Vill du spara dokumentet?");
                msg.Commands.Add(new UICommand("Ja", async x =>
                {
                    await SaveFileAsync();

                    if (!saveAs)
                    {
                        await OpenFileAndUpdateUIAsync();
                    }
                }));
                msg.Commands.Add(new UICommand("Nej", async x =>
                {
                    await OpenFileAndUpdateUIAsync();
                }));
                msg.Commands.Add(new UICommand("Avbryt"));
                await msg.ShowAsync();
            } else
            {
                await OpenFileAndUpdateUIAsync();
            }
        }

        private async Task OpenFileAndUpdateUIAsync()
        {
            var fileOpenPicker = new FileOpenPicker();
            fileOpenPicker.FileTypeFilter.Add(".txt");
            string oldFileName = "";

            if (file != null)
            {
                oldFileName = file.Name;
            }
            file = await fileOpenPicker.PickSingleFileAsync();

            if (file != null && file.Name != oldFileName)
            {
                dragAndDrop = false;
                DragAndDrop_TextBlock.Text = "Spara fil inaktiverat: Nej";
                var text = await FileIO.ReadTextAsync(file);
                UpdateUIWithFileText(text, file);
            }
        }

        private void ClearText(object sender, RoutedEventArgs e)
        {
            Main_TextBox.Text = string.Empty;
        }

        private void MainTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (openFile && (previousOriginalMainText != originalMainText) || (originalMainText == Main_TextBox.Text))
            {
                FileName_TextBlock.Text = fileName;
                openFile = false;
                unsavedChanges = false;
            } else
            {
                FileName_TextBlock.Text = "*" + fileName;
                unsavedChanges = true;
            }
            string text = Main_TextBox.Text;
            characterCountNoSpace = text.Replace(" ", "").Replace("\n", "").Replace("\t", "").Replace("\r", "").Length;
            characterCountSpace = text.Replace("\n", "").Replace("\t", "").Replace("\r", "").Length;
            CharacterNoSpace_TextBlock.Text = "Utan mellanslag: " + characterCountNoSpace + " tecken";
            CharacterSpace_TextBlock.Text = "Med mellanslag: " + characterCountSpace + " tecken";
            string[] words = text.Split(new char[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            int wordCount = words.Length;
            WordCount_TextBlock.Text = "Antal ord: " + wordCount;
            int rowCount = text.Split('\r').Length;
            Row_TextBlock.Text = "Rader: " + rowCount;
        }

        private void ShowDragGraphics_DragEnter(object sender, DragEventArgs e)
        {
            Main_TextBox.Background = new SolidColorBrush(Windows.UI.Colors.Gray);
            // https://www.c-sharpcorner.com/article/how-to-implement-drag-drop-functionality-in-universal-windows-platform-uwp/  
            e.AcceptedOperation = DataPackageOperation.Copy;
            e.DragUIOverride.Caption = "Släpp fil här";
            e.DragUIOverride.IsGlyphVisible = true;
            e.DragUIOverride.IsContentVisible = true;
            e.DragUIOverride.IsCaptionVisible = true;
        }

        private void HideDragGraphics_DragLeave(object sender, DragEventArgs e)
        {
            Main_TextBox.Background = null;
        }

        private enum DropMode
        {
            Append, 
            Insert, 
            Open
        }

        private async void DropFileInTextBox_Drop(object sender, DragEventArgs e)
        {
            DropMode dropMode;
            Main_TextBox.Background = null;

            if ((Window.Current.CoreWindow.GetKeyState(VirtualKey.Control) & CoreVirtualKeyStates.Down) == CoreVirtualKeyStates.Down)
            {
                dropMode = DropMode.Append;
            } else if((Window.Current.CoreWindow.GetKeyState(VirtualKey.Shift) & CoreVirtualKeyStates.Down) == CoreVirtualKeyStates.Down)
            {
                dropMode = DropMode.Insert;    
            } else
            {
                dropMode = DropMode.Open;
            }

            await DropFileInTextBoxAsync(e, dropMode);
        }

        private async Task DropFileInTextBoxAsync(DragEventArgs e, DropMode dropMode)
        {
            // Fick hjälp av ChatGPT att klura ut hur man kan säkerställa att det är en .txt-fil och hur man kan läsa den. Allt annat är mitt egna originella arbete.
            if (e.DataView.Contains(StandardDataFormats.StorageItems))
            {
                var items = await e.DataView.GetStorageItemsAsync();
                if (items.Any(item => item.IsOfType(StorageItemTypes.File)))
                {
                    var readFile = items.FirstOrDefault(item => item.IsOfType(StorageItemTypes.File) && Path.GetExtension(item.Name).Equals(".txt", StringComparison.OrdinalIgnoreCase));
                    if (readFile != null)
                    {
                        StorageFile txtFile = (StorageFile)readFile;
                        string fileContent = await FileIO.ReadTextAsync(txtFile);

                        if (dropMode == DropMode.Append)
                        {
                            Main_TextBox.Text += fileContent;
                        } else if (dropMode == DropMode.Insert) 
                        {
                            int cursorPosition = Main_TextBox.SelectionStart;
                            Main_TextBox.Text = Main_TextBox.Text.Insert(cursorPosition, fileContent);
                            Main_TextBox.SelectionStart = cursorPosition + fileContent.Length;
                        } else
                        {
                            if (unsavedChanges)
                            {
                                MessageDialog msg = new MessageDialog("Vill du spara dokumentet?");
                                msg.Commands.Add(new UICommand("Ja", async x =>
                                {
                                    if (dragAndDrop)
                                    {
                                        await SaveAsAsync();
                                    } else
                                    {
                                        await SaveFileAsync();
                                    }
                                    
                                    if (!saveAs)
                                    {
                                        file = txtFile;
                                        UpdateUIWithFileText(fileContent, file);
                                    }
                                }));
                                msg.Commands.Add(new UICommand("Nej", x =>
                                {
                                    file = txtFile;
                                    UpdateUIWithFileText(fileContent, file);
                                }));
                                msg.Commands.Add(new UICommand("Avbryt"));
                                await msg.ShowAsync();
                            }
                            else
                            {
                                file = txtFile;
                                UpdateUIWithFileText(fileContent, file);
                            }
                            dragAndDrop = true;
                            DragAndDrop_TextBlock.Text = "Spara fil inaktiverat: Ja";
                        }
                    }
                    else
                    {
                        Debug.WriteLine("No .txt files were dropped.");
                    }
                }
            }
        }

        private void UpdateUIWithFileText(string text, StorageFile file)
        {
            openFile = true;
            newFile = false;
            unsavedChanges = false;
            previousOriginalMainText = Main_TextBox.Text;
            Main_TextBox.Text = text;
            originalMainText = text;
            FileName_TextBlock.Text = fileName = file.Name;
            characterCountNoSpace = text.Replace(" ", "").Length;
            characterCountSpace = text.Length;
            CharacterNoSpace_TextBlock.Text = "Utan mellanslag: " + characterCountNoSpace + " tecken";
            CharacterSpace_TextBlock.Text = "Med mellanslag: " + characterCountSpace + " tecken";
            string[] words = text.Split(new char[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            int wordCount = words.Length;
            WordCount_TextBlock.Text = "Antal ord: " + wordCount;
        }

    }
}
