using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

using Caliburn.Micro;

using uwpUI.Core.Models;
using uwpUI.Core.Services;
using uwpUI.Helpers;

namespace uwpUI.ViewModels
{
    public class DevViewModel : Screen
    {
        public ObservableCollection<BdoItem> Source { get; } = new ObservableCollection<BdoItem>();

        public DevViewModel()
        {
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
