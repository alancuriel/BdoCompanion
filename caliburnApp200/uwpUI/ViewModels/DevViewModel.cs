using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Caliburn.Micro;
using uwpUI.Core.Helpers;
using uwpUI.Core.Models;
using uwpUI.Core.Services;
using uwpUI.Helpers;
using Windows.Storage;

namespace uwpUI.ViewModels
{
    public class DevViewModel : Screen
    {
        public ObservableCollection<BdoItem> Source { get; } = new ObservableCollection<BdoItem>();
        public ObservableCollection<ItemGroup> Groups { get; } = new ObservableCollection<ItemGroup>();
        public ObservableCollection<Recipe> Recipes { get; } = new ObservableCollection<Recipe>();
        public ObservableCollection<RecipeMat> RecipeMats { get; } = new ObservableCollection<RecipeMat>();

        public DevViewModel()
        {
        }

        public async Task LoadItemsAsync()
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            picker.FileTypeFilter.Add(".json");

            StorageFile file = await picker.PickSingleFileAsync();

            string jsonText = await FileIO.ReadTextAsync(file);
            List<BdoItem> items = await Json.ToObjectAsync<List<BdoItem>>(jsonText);

            foreach (var item in items)
            {
                BdoItem bdoItem = BdoDataService.GetItemById(item.Id);
                if (bdoItem == null)
                {
                    BdoDataService.AddItem(item);
                }
                else
                {
                    bdoItem.Name = item.Name;
                    bdoItem.Grade = item.Grade;
                    bdoItem.Img = item.Img;
                    bdoItem.Description = item.Description;
                    bdoItem.Category = item.Category;
                    bdoItem.Weight = item.Weight;
                    bdoItem.ItemGroupId = item.ItemGroupId;
                    bdoItem.BuyPrice = item.BuyPrice;
                    bdoItem.SellPrice = item.SellPrice;
                    bdoItem.Knowledge = item.Knowledge;

                    BdoDataService.UpdateItem(bdoItem);
                }
                BdoDataService.Commit();
            }

            await LoadDataAsync();
        }

        public async Task LoadRecipesAsync()
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            picker.FileTypeFilter.Add(".json");

            StorageFile file = await picker.PickSingleFileAsync();

            string jsonText = await FileIO.ReadTextAsync(file);
            var recipes = await Json.ToObjectAsync<List<JsonRecipeModel>>(jsonText);

            foreach (var Jsonrecipe in recipes)
            {
                var recipe = new Recipe
                {
                    Id = Jsonrecipe.Id,
                    Name = Jsonrecipe.Name,
                    Img = Jsonrecipe.Img,
                    Exp = Jsonrecipe.Exp,
                    Grade = Jsonrecipe.Grade,
                    Type = Jsonrecipe.Type,
                    SkillLevel = Jsonrecipe.SkillLevel
                };

                for(int i = 1; i <= Jsonrecipe.CraftingResults.Count; i++)
                {
                    if (i == 1)
                        recipe.Item1Id = Int32.Parse(Jsonrecipe.CraftingResults[0].Replace("item--",""));

                    if (i == 2)
                        recipe.Item2Id = Int32.Parse(Jsonrecipe.CraftingResults[1].Replace("item--", ""));
                }

                BdoDataService.AddRecipe(recipe);
                BdoDataService.Commit();

                foreach (var mat in Jsonrecipe.ItemMaterials)
                {
                    var recipeMat = new RecipeMat
                    {
                        RecipeId = recipe.Id,
                        Amount = mat.Value
                    };
                    

                    string[] itemOrGroup = mat.Key.Split("--");

                    if(itemOrGroup[0] == "item")
                    {
                        recipeMat.IsItem = true;
                        recipeMat.ItemId = Int32.Parse(itemOrGroup[1]);
                    }
                    else
                    {
                        recipeMat.IsItem = false;
                        recipeMat.ItemGroupId = Int32.Parse(itemOrGroup[1]);
                    }

                    BdoDataService.AddRecipeMat(recipeMat);
                    BdoDataService.Commit();
                }
            }
            await LoadDataAsync();
        }

        public async Task LoadDescriptionsAsync()
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            picker.FileTypeFilter.Add(".txt");

            StorageFile file = await picker.PickSingleFileAsync();
            var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);

            using (StreamReader sr = new StreamReader(stream.AsStreamForRead()))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();

                    if (line[0] == '0')
                    {
                        var val = line.Split('\t');

                        if (val[4] == "1")
                        {
                            int id = Int32.Parse(val[1]);
                            BdoItem item = BdoDataService.GetItemById(id);

                            if (item != null)
                            {
                                string description = val[5].Trim('\"');
                                item.Description = description;

                                BdoDataService.UpdateItem(item);
                                BdoDataService.Commit();
                            }
                        }

                    }
                    else if (line.StartsWith("34"))
                    {
                        var val = line.Split('\t');

                        if (val[4] == "1")
                        {
                            BdoItem item = BdoDataService.GetItemsByKnowledge($"theme--{val[1]}")?.FirstOrDefault();


                            if (item != null)
                            {
                                string knowledge = val[5].Trim('\"');
                                item.Knowledge = knowledge;

                                BdoDataService.UpdateItem(item);
                                BdoDataService.Commit();
                            }
                        }
                    }
                }
            }
            await LoadDataAsync();
        }

        public async Task LoadItemGroups()
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            picker.FileTypeFilter.Add(".json");

            StorageFile file = await picker.PickSingleFileAsync();

            string jsonText = await FileIO.ReadTextAsync(file);
            var itemGroups = await Json.ToObjectAsync<List<ItemGroup>>(jsonText);

            foreach (var itemGroup in itemGroups)
            {
                ItemGroup dbitemgroup = BdoDataService.GetItemGroupById(itemGroup.Id);
                if (dbitemgroup != null)
                    continue;

                BdoDataService.AddItemGroup(new ItemGroup
                {
                    Id = itemGroup.Id,
                    Name = itemGroup.Name
                });

                BdoDataService.Commit();

                foreach (var item in itemGroup.Items)
                {
                    var dbItem = BdoDataService.GetItemById(item.Id);

                    if(dbItem == null)
                    {
                        item.ItemGroupId = itemGroup.Id;
                        BdoDataService.AddItem(item);
                    }
                    else
                    {
                        dbItem.ItemGroupId = itemGroup.Id;
                        BdoDataService.UpdateItem(dbItem);
                    }

                    BdoDataService.Commit();
                }

            }
            await LoadDataAsync();
        }

        public async Task LoadDataAsync()
        {
            Source.Clear();
            Groups.Clear();

            

            var data = BdoDataService.AllItems();
            var groupdata = BdoDataService.AllItemGroups();
            
            await Task.CompletedTask;

            foreach (var item in data)
            {
                Source.Add(item);
            }

            foreach (var group in groupdata)
            {
                Groups.Add(group);
            }

        }
    }
}
