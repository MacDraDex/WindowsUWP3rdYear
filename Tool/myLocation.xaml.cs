using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Services.Maps;
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
 
    public sealed partial class myLocation : Page
    {
        Geolocator locator;
        public myLocation()
        {
            this.InitializeComponent();
            this.SetupGeolocator();
        }


        //this method sets up geolocator
        //https://msdn.microsoft.com/library/windows/apps/br225535
        private void SetupGeolocator()
        {
            locator = new Geolocator();
            locator.DesiredAccuracy = PositionAccuracy.High;

        }

        //geoloc setup
        private async void setupGeoLocation()
        {
            //this piece checks if geolocator is working correcly. 
            //code snippet taken from https://github.com/arkiq/SampleGPS/blob/master/GPSDataSample/MainPage.xaml.cs
            var accessStatus = await Geolocator.RequestAccessAsync();
            switch (accessStatus)
            {
                case GeolocationAccessStatus.Allowed:
                    {
                        MessageDialog myMsg = new MessageDialog(" GeoLocation in use.");
                        await myMsg.ShowAsync();


                        break;
                    }
                case GeolocationAccessStatus.Denied:
                    {
                        MessageDialog myMsg = new MessageDialog("Please turn on GeoLocation Data.");
                        await myMsg.ShowAsync();
                        break;
                    }
                default:
                    {
                        MessageDialog myMsg = new MessageDialog("Please check your Connection.");
                        await myMsg.ShowAsync();
                        break;
                    }
            }
        }


        

        private async void getLocation(Geoposition pos)
        {
            //locator setup
            BasicGeoposition location = new BasicGeoposition();
            location.Latitude = pos.Coordinate.Latitude;
            location.Longitude = pos.Coordinate.Longitude;
            Geopoint pointToReverseGeocode = new Geopoint(location);

           
            // myPositions is a query to find exact location. in this case it will show the location of the device settings. 
            MapLocationFinderResult myPosition =await MapLocationFinder.FindLocationsAtAsync(pointToReverseGeocode);

     
            //if position found then print information to the screen
            if (myPosition.Status == MapLocationFinderStatus.Success)
            {
                
                tblLocation2.Text += "Country:    " + myPosition.Locations[0].Address.Country.ToString()
                    + System.Environment.NewLine +
                    "City:    " + myPosition.Locations[0].Address.Town.ToString()
                    + System.Environment.NewLine +
                    "Address:  " + myPosition.Locations[0].Address.FormattedAddress.ToString() 
                    + System.Environment.NewLine +
                    "Time:    " + pos.Coordinate.Timestamp.ToString() 
                    + System.Environment.NewLine +
                    "Longitude: " + pos.Coordinate.Latitude.ToString() 
                    + System.Environment.NewLine +
                    "Latitude:  " + pos.Coordinate.Longitude.ToString() 
                    + System.Environment.NewLine;
             }

        }

        private async void btnLocation_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Geoposition pos = await locator.GetGeopositionAsync();
            getLocation(pos);

            
            //shows Locations
            tblExactLocation.Visibility = Visibility.Visible;
        }

        //ISOLATED STORAGE
        //LOCATION IS BEING SAVED IN CURRENT LOCAL FOLDER
        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            StorageFolder locationStorage = ApplicationData.Current.LocalFolder;

            StorageFile locationFile;
            string fileText = "";

            try
            {
                locationFile = await locationStorage.GetFileAsync("location.txt");
                fileText = await Windows.Storage.FileIO.ReadTextAsync(locationFile);
            }
            catch (Exception E)
            {
                string message = E.Message;
                locationFile = await locationStorage.CreateFileAsync("location.txt");
            }


            string resultOutput = "";

            resultOutput = tblLocation2.Text.ToString();


            await Windows.Storage.FileIO.WriteTextAsync(locationFile, fileText + resultOutput + System.Environment.NewLine + System.Environment.NewLine);

            MessageDialog clearDialog = new MessageDialog("Location Saved");
            await clearDialog.ShowAsync();

            // save button disapears after clicking on it
            btnSave.Visibility = Visibility.Collapsed;
        }

        // back navigation
        private void myLocation_BackRequested(object sender, BackRequestedEventArgs e)
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
                SystemNavigationManager.GetForCurrentView().BackRequested += myLocation_BackRequested;
            }
            else
            {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            }

        }
    }
}
