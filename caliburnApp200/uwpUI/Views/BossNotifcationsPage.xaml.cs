using System;
using uwpUI.Helpers;
using uwpUI.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace uwpUI.Views
{
    public sealed partial class BossNotifcationsPage : Page
    {
        public BossNotifcationsPage()
        {
            InitializeComponent();
            SettingsShadow.Receivers.Add(SettingsBackgroundGrid);
        }


        private BossNotifcationsViewModel ViewModel
        {
            get { return DataContext as BossNotifcationsViewModel; }
        }

        private void GridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GridView item = (sender as GridView);
            GridViewItem gvitem = item.ContainerFromItem(item.SelectedItem) as GridViewItem;
            CheckBox CheckBox = FindControlHelper.FindChildControl<CheckBox>(gvitem, typeof(CheckBox), "BossCheckBox");
            CheckBox?.Focus(FocusState.Programmatic);
        }
    }
}
