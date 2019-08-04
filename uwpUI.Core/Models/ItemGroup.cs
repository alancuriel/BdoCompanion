using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace uwpUI.Core.Models
{
    public class ItemGroup
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        [MaxLength(30)]
        [Required]
        public int Name { get; set; }

        public List<BdoItem> Items { get; set; }
    }
}
