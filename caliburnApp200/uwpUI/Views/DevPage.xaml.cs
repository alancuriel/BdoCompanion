using System;

using uwpUI.ViewModels;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace uwpUI.Views
{
    public sealed partial class DevPage : Page
    {
        public DevPage()
        {
            InitializeComponent();
        }

        private DevViewModel ViewModel
        {
            get { return DataContext as DevViewModel; }
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            await ViewModel.LoadDataAsync();
        }
    }
}
