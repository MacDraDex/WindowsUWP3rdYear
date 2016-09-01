using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Provider;
using Windows.UI.Popups;
using Windows.UI.Text;
using Windows.UI.Xaml.Controls;


// i have used this tutorial to create Rich text editor
//https://msdn.microsoft.com/en-us/windows/uwp/controls-and-patterns/rich-edit-box

namespace Tool
{
    public class Library
    {
        // Task is a asynch operation that return value
        public async Task<bool> Confirm(string content, string title, string ok, string cancel)
        {
            bool result = false;
            MessageDialog dialog = new MessageDialog(content, title);
            dialog.Commands.Add(new UICommand(ok, new UICommandInvokedHandler((cmd) => result = true)));
            dialog.Commands.Add(new UICommand(cancel, new UICommandInvokedHandler((cmd) => result = false)));
            await dialog.ShowAsync();
            return result;
        }
        //get information from app
        public string get(ref RichEditBox document)
        {
            string value = string.Empty;
            document.Document.GetText(Windows.UI.Text.TextGetOptions.FormatRtf, out value);
            return value;
        }
        // set settings
        private void set(ref RichEditBox document, string value)
        {
            document.Document.SetText(Windows.UI.Text.TextSetOptions.FormatRtf, value);
            selected(ref document);
        }
        //tracks selection
        private void selected(ref RichEditBox document)
        {
            document.Focus(Windows.UI.Xaml.FocusState.Keyboard);
        }

     
       
        //Size of the text
        public void Size(ref RichEditBox document, ref ComboBox value)
        {
            if (document != null && value != null)
            {
                string selected = ((ComboBoxItem)value.SelectedItem).Tag.ToString();
                document.Document.Selection.CharacterFormat.Size = float.Parse(selected);
                this.selected(ref document);
            }
        }

        //sets text to bold
        public bool Bold(ref RichEditBox document)
        {
            document.Document.Selection.CharacterFormat.Bold = Windows.UI.Text.FormatEffect.Toggle;
            selected(ref document);
            return document.Document.Selection.CharacterFormat.Bold.Equals(FormatEffect.On);
        }
        //sets text to italic
        public bool Italic(ref RichEditBox document)
        {
            document.Document.Selection.CharacterFormat.Italic = Windows.UI.Text.FormatEffect.Toggle;
            selected(ref document);
            return document.Document.Selection.CharacterFormat.Italic.Equals(FormatEffect.On);
        }

        // this is file search
        ///https://msdn.microsoft.com/en-us/windows/uwp/files/quickstart-using-file-and-folder-pickers
        public async void Open(RichEditBox document)
        {
            try
            {
                FileOpenPicker picker = new FileOpenPicker();
                picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
                picker.FileTypeFilter.Add(".rtf");
                StorageFile file = await picker.PickSingleFileAsync();
                set(ref document, await FileIO.ReadTextAsync(file));
            }
            catch
            {

            }
        }

        // saves file to desktop with .rtf extension ¬ default name of file is Document
        //https://msdn.microsoft.com/en-us/windows/uwp/files/quickstart-save-a-file-with-a-picker
        public async void Save(RichEditBox document)
        {
            try
            {

                FileSavePicker picker = new FileSavePicker();
                picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
                picker.FileTypeChoices.Add("Rich Text", new List<string>() { ".rtf" });
                picker.DefaultFileExtension = ".rtf";
                picker.SuggestedFileName = "Document";

                StorageFile file = await picker.PickSaveFileAsync();
                if (file != null)
                {
                    CachedFileManager.DeferUpdates(file);
                    await FileIO.WriteTextAsync(file, get(ref document));
                    FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync(file);
                }
            }
            // if there is some problem with a file it will display the error message to the screen
            catch
            {
                MessageDialog myMsg = new MessageDialog(" Problem Saving File.");
                await myMsg.ShowAsync();
            }
        }
    }
}
