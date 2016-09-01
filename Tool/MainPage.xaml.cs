using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Tool
{
 
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        // this method navigates to Notepad Page
        private void btnNotepad_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(NotepadPage));
           
        }

        // this method navigates to Location Page
        private void btnLocation_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(myLocation));
        }

        // this method navigates to Storage Page
        private void btnStorage_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Storage));
        }
        
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;

            base.OnNavigatedTo(e);
        }
    }
}
