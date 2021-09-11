using CompanionApp2021.Models;
using CompanionApp2021.ViewModels;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace CompanionApp2021.Views
{
    public sealed partial class World_BossesPage : Page
    {
        public World_BossesViewModel ViewModel { get; } = new World_BossesViewModel();

        public World_BossesPage()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            await ViewModel.InitializeAsync();
        }

        private void ToggleSwitch_Toggled(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var worldEvent = (sender as ToggleSwitch).DataContext as WorldEventModel;

            if (worldEvent != null)
            {
                ViewModel.Switch(worldEvent);
            }
        }
    }
}
