using System;
using uwpUI.ViewModels;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using uwpUI.Helpers;

namespace uwpUI.Views
{
    public sealed partial class SecondPage : Page
    {
        public SecondPage()
        {
            InitializeComponent();
        }

        private SecondViewModel ViewModel
        {
            get { return DataContext as SecondViewModel; }
        }

        private void Bosses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GridView item = (sender as GridView);
            GridViewItem gvitem = item.ContainerFromItem(item.SelectedItem) as GridViewItem;
            CheckBox CheckBox = FindControlHelper.FindChildControl<CheckBox>(gvitem, typeof(CheckBox), "BossSwitch");
            CheckBox?.Focus(FocusState.Programmatic);
        }

        private void Bosses_Loaded(object sender, RoutedEventArgs e)
        {
            GridView gridView = sender as GridView;
            gridView.SelectedIndex = 0;
        }

        private void NotificationTime_PointerWheelChanged(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {

        }
    }
}
