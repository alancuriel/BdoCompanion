using System;
using System.Linq;

using Microsoft.Toolkit.Uwp.UI.Controls;
using uwpUI.Helpers;
using uwpUI.ViewModels;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace uwpUI.Views
{
    public sealed partial class CookingRecipesPage : Page
    {
        public CookingRecipesPage()
        {
            InitializeComponent();
            
        }

        private CookingRecipesViewModel ViewModel
        {
            get { return DataContext as CookingRecipesViewModel; }
        }

        private void ListView_Loaded(object sender, RoutedEventArgs e)
        {
            (sender as ListView).SelectedIndex = 0;
        }


        //private void MasterDetailsViewControl_Loaded(object sender, RoutedEventArgs e)
        //{
        //    if (MasterDetailsViewControl.ViewState == MasterDetailsViewState.Both)
        //    {
        //        ViewModel.ActiveItem = ViewModel.Items.FirstOrDefault();
        //    }
        //}


    }
}
