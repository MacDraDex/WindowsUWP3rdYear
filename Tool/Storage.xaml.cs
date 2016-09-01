using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Tool
{
 
    public sealed partial class Storage : Page
    {
        public Storage()
        {
            this.InitializeComponent();
            this.ShowLocation();
        }

        private async void ShowLocation()
        {
            //if no text make it invisible
            if (locationShow.Text.ToString() == "")
            {
                btnClear.Visibility = Visibility.Visible;
            }
            //manage folder + content + show info about them
            //creating StorageFile
            StorageFolder locationFolder = ApplicationData.Current.LocalFolder;
            StorageFile locationFile;


            try
            {
                locationFile = await locationFolder.GetFileAsync("location.txt");

            }
            catch (Exception E)
            {
                string errorMSG = E.Message;
                locationShow.Text = "File not Found";
                return;
            }
            //reading from the file weatherFIle
            string fileText = await Windows.Storage.FileIO.ReadTextAsync(locationFile);


            //print content to txtBox locationShow
            locationShow.Text = locationShow.Text + fileText;


        }

        private async void btnClear_Tapped(object sender, TappedRoutedEventArgs e)
        {
            StorageFolder locationFolder = ApplicationData.Current.LocalFolder;

            StorageFile locationFile;


            try
            {
                locationFile = await locationFolder.GetFileAsync("location.txt");

            }
            catch (Exception E)
            {
                string errorMSG = E.Message;
                locationShow.Text = "File not Found";
                return;
            }

            string text = "";
            await Windows.Storage.FileIO.WriteTextAsync(locationFile, text);

            locationShow.Text = "";

            MessageDialog clearDialog = new MessageDialog("Cleared.");
            await clearDialog.ShowAsync();
        }

        private void Storage_BackRequested(object sender, BackRequestedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame == null)
                return;

          
            if (rootFrame.CanGoBack && e.Handled == false)
            {
                e.Handled = true;
                rootFrame.GoBack();
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame.CanGoBack)
            {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
                SystemNavigationManager.GetForCurrentView().BackRequested += Storage_BackRequested;
            }
            else
            {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            }

        }//nav end
    }
}
