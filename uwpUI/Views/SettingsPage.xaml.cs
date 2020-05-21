using System;

using uwpUI.ViewModels;

using Windows.UI.Xaml.Controls;

namespace uwpUI.Views
{
    public sealed partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        private SettingsViewModel ViewModel
        {
            get { return DataContext as SettingsViewModel; }
        }
    }
}
