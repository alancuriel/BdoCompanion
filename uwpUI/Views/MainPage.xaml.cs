using System;

using uwpUI.ViewModels;

using Windows.UI.Xaml.Controls;

namespace uwpUI.Views
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private MainViewModel ViewModel
        {
            get { return DataContext as MainViewModel; }
        }
    }
}
