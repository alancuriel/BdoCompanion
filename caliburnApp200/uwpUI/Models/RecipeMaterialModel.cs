using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uwpUI.Core.Models;

namespace uwpUI.Models
{
    public class RecipeMaterialModel
    {
        public string Name { get; set; }
        public string Img { get; set; }
        public int Amount { get; set; }
        public ItemGrade Grade { get; set; }
        public bool IsItem { get; set; }
        public int Id { get; set; }
    }
}
