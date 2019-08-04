using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace uwpUI.Core.Models
{
    public class BdoItem
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        [MaxLength(100)]
        public string Img { get; set; }
        [MaxLength(25)]
        public string Category { get; set; }
        public ItemGrade Grade { get; set; }
        [MaxLength(25)]
        public string Weight { get; set; }
        [MaxLength(3000)]
        public string Description { get; set; }
        public int? ItemGroupId { get; set; }
    }
}
