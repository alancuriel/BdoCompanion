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

        public async Task LoadDescriptionsAsync()
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            picker.FileTypeFilter.Add(".txt");

            StorageFile file = await picker.PickSingleFileAsync();
            var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);

            using(StreamReader sr = new StreamReader(stream.AsStreamForRead()))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();

                    if (line[0] == '0')
                    {
                        var val = line.Split('\t');

                        if(val[4] == "1")
                        {
                            int id = Int32.Parse(val[1]);
                            BdoItem item = BdoDataService.GetItemById(id);

                            if(item != null)
                            {
                                string description = val[5].Trim('\"');
                                item.Description = description;

                                BdoDataService.UpdateItem(item);
                                BdoDataService.Commit();
                            }
                        }

                    }
                    else if(line.StartsWith("34"))
                    {
                        var val = line.Split('\t');

                        if(val[4] == "1")
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

        }

        public async Task LoadDataAsync()
        {
            Source.Clear();

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
