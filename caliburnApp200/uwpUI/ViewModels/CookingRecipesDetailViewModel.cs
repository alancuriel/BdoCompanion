using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Caliburn.Micro;

using uwpUI.Core.Models;
using uwpUI.Core.Services;
using uwpUI.Models;

namespace uwpUI.ViewModels
{
    public class CookingRecipesDetailViewModel : Screen
    {
        private BindableCollection<RecipeMaterialModel> _materials;
        public BindableCollection<RecipeMaterialModel> Materials
        {
            get
            {
                return _materials;
            }
            set
            {
                _materials = value;
                NotifyOfPropertyChange(() => Materials);
            }
        }

        private BindableCollection<BdoItem> _results;

        public BindableCollection<BdoItem> Results
        {
            get { return _results; }
            set
            {
                _results = value;
                NotifyOfPropertyChange(() => Results);
            }
        }



        public CookingRecipesDetailViewModel(Recipe item)
        {
            Item = item;

        }

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            Materials = new BindableCollection<RecipeMaterialModel>();
            Results = new BindableCollection<BdoItem>();
            LoadMaterials();
        }

        private void LoadMaterials()
        {
            var materials = BdoDataService.GetRecipeMatsByRecipeID(Item.Id);
            foreach (var material in materials)
            {
                if (material.IsItem)
                {
                    material.Item = BdoDataService.GetItemById(material.ItemId.Value);
                    Materials.Add(new RecipeMaterialModel
                    {
                        Name = material.Item.Name,
                        Img = material.Item.Img,
                        Grade = material.Item.Grade,
                        IsItem = true,
                        Id = material.Item.Id,
                        Amount = material.Amount
                    });
                }
                else
                {
                    //Item Group
                    material.ItemGroup = BdoDataService.GetItemGroupById(material.ItemGroupId.Value);
                    BdoItem item = BdoDataService.FirstItemFromItemGroup(material.ItemGroupId.Value);
                    Materials.Add(new RecipeMaterialModel
                    {
                        Name = material.ItemGroup.Name,
                        Img = material.ItemGroup.Items[0].Img,
                        Grade = ItemGrade.White,
                        IsItem = false,
                        Id = material.ItemGroup.Id,
                        Amount = material.Amount
                    });
                }
            }

            BdoItem result0 = BdoDataService.GetItemById(Item.Item1Id.Value);
            Results.Add(result0);

            if (Item.Item2Id.HasValue)
            {
                BdoItem result1 = BdoDataService.GetItemById(Item.Item2Id.Value);
                Results.Add(result1);
            }

        }

        public Recipe Item { get; }
    }
}
