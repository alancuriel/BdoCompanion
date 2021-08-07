namespace uwpUI.Core.Models
{
    public class RecipeMat
    {
        public int Id { get; set; }
        public int Amount { get; set; }
        public int RecipeId { get; set; }
        public bool IsItem { get; set; }
        public BdoItem Item { get; set; }
        public int? ItemId { get; set; }
        public ItemGroup ItemGroup { get; set; }
        public int? ItemGroupId { get; set; }
    }
}