using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

            foreach(var item in items)
            {
                BdoItem bdoItem = BdoDataService.GetItemById(item.Id);
                if(bdoItem == null)
                {
                    BdoDataService.AddItem(item);
                }
                else
                {
                    BdoDataService.UpdateItem(item);
                }
                BdoDataService.Commit();
            }

            await LoadDataAsync();
            

        }

        public async Task LoadDataAsync()
        {
            Source.Clear();

            //BdoDataService.AddItem(new BdoItem
            //{
            //    Id = 4435,
            //    Name = "Vell's Concentrated Magic",
            //    Category = "General",
            //    Grade = ItemGrade.Orange
            //});
            //BdoDataService.Commit();

            // TODO WTS: Replace this with your actual data
            var data = BdoDataService.AllItems();
            await Task.CompletedTask;

            foreach (var item in data)
            {
                Source.Add(item);
            }
        }
    }
}
