using System;

using uwpUI.ViewModels;

namespace uwpUI.Views.CookingRecipesDetail
{
    public sealed partial class MasterView
    {
        public MasterView()
        {
            InitializeComponent();
        }

        public CookingRecipesDetailViewModel ViewModel => DataContext as CookingRecipesDetailViewModel;
    }
}
