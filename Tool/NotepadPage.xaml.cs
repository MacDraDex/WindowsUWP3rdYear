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
    public sealed partial class NotepadPage : Page
    {
        public Library Library = new Library();
    
        public NotepadPage()
        {
            this.InitializeComponent();
        }


        //method to open document in notepad
        private void Open_Click(object sender, RoutedEventArgs e)
        {
            Library.Open(Document);
        }

        // save file in notepad
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Library.Save(Document);
        }

        //https://msdn.microsoft.com/en-us/library/system.windows.controls.primitives.selector.selectionchanged(v=vs.110).aspx
        private void Size_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Library.Size(ref Document, ref Size);
        }

        //https://msdn.microsoft.com/en-us/windows/uwp/controls-and-patterns/rich-edit-box
        private void Bold_Click(object sender, RoutedEventArgs e)
        {
            Bold.IsChecked = Library.Bold(ref Document);
        }


        private void Italic_Click(object sender, RoutedEventArgs e)
        {
            Italic.IsChecked = Library.Italic(ref Document);
        }


      


        // this method allows to navigate to previous page
        private void NotepadPage_BackRequested(object sender, BackRequestedEventArgs e)
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
                SystemNavigationManager.GetForCurrentView().BackRequested += NotepadPage_BackRequested;
            }
            else
            {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            }

        }
    }
}
