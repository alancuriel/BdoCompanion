using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace uwpUI.Services
{
    public static class BdoDataLoadService
    {
        public static async Task InitializeAsync()
        {
            var dbFile = await ApplicationData.Current.LocalFolder.TryGetItemAsync("bdoData.db") as StorageFile;

            if (null == dbFile || SystemInformation.IsFirstRun || SystemInformation.IsAppUpdated)
            { 
                var localFolder = ApplicationData.Current.LocalFolder;
                var originalDbFileUri = new Uri("ms-appx:///Assets/bdoData.db");
                var originalDbFile = await StorageFile.GetFileFromApplicationUriAsync(originalDbFileUri);

                if (null != originalDbFile)
                {
                    dbFile = await originalDbFile.CopyAsync(localFolder, "bdoData.db", NameCollisionOption.ReplaceExisting);
                }
            }
        }
    }
}
