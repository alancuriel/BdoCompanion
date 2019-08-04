using System;
using System.Collections.Generic;
using System.Text;

namespace uwpUI.Core.Models
{
    public class BdoItemModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Img { get; set; }
        public string Category { get; set; }
        public ItemGrade Grade { get; set; }
        public string Weight { get; set; }
        public string Description { get; set; }
        public int ItemGroupId { get; set; }
    }
}
