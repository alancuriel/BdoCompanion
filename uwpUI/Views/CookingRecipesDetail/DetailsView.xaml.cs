using System;

using uwpUI.ViewModels;

namespace uwpUI.Views.CookingRecipesDetail
{
    public sealed partial class DetailsView
    {
        public DetailsView()
        {
            InitializeComponent();
        }

        public CookingRecipesDetailViewModel ViewModel => DataContext as CookingRecipesDetailViewModel;
    }
}
