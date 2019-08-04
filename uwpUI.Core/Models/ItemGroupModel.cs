using System;
using System.Collections.Generic;
using System.Text;

namespace uwpUI.Core.Models
{
    public class ItemGroupModel
    {
        public int Id { get; set; }
        public int Name { get; set; }

        public List<BdoItemModel> Items { get; set; }
    }
}
