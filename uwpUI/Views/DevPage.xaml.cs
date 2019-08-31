using System;

using uwpUI.ViewModels;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace uwpUI.Views
{
    public sealed partial class DevPage : Page
    {
        // TODO WTS: Change the grid as appropriate to your app, adjust the column definitions on DevPage.xaml.
        // For help see http://docs.telerik.com/windows-universal/controls/raddatagrid/gettingstarted
        // You may also want to extend the grid to work with the RadDataForm http://docs.telerik.com/windows-universal/controls/raddataform/dataform-gettingstarted
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
