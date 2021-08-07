using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace uwpUI.Core.Models
{
    public class Recipe
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Img { get; set; }
        public ItemGrade Grade { get; set; }
        public RecipeType Type { get; set; }
        public string SkillLevel { get; set; }
        public int? Exp { get; set; }
        public List<RecipeMat> Mats { get; set; }
        public BdoItem Item1 { get; set; }
        public int? Item1Id { get; set; }
        public BdoItem Item2 { get; set; }
        public int? Item2Id { get; set; }
    }
}
