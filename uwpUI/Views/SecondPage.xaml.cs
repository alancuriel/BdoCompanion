using System;
using uwpUI.ViewModels;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

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

        public static T FindControl<T>(UIElement parent, Type targetType, string ControlName) where T : FrameworkElement
        {

            if (parent == null) return null;
            if (parent.GetType() == targetType && ((T)parent).Name == ControlName) return (T)parent;

            int count = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < count; i++)
            {
                UIElement child = (UIElement)VisualTreeHelper.GetChild(parent, i);
                T result = FindControl<T>(child, targetType, ControlName);
                if (result != null) return result;
            }
            return null;
        }

        private void Bosses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GridView item = (sender as GridView);
            GridViewItem gvitem = item.ContainerFromItem(item.SelectedItem) as GridViewItem;
            ToggleSwitch toggleSwitch = FindControl<ToggleSwitch>(gvitem, typeof(ToggleSwitch), "BossSwitch");
            toggleSwitch?.Focus(FocusState.Programmatic);
        }

        private void Bosses_Loaded(object sender, RoutedEventArgs e)
        {
            GridView gridView = sender as GridView;
            gridView.SelectedIndex = 0;
        }

    }
}
