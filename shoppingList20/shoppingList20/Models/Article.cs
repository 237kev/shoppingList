using System.ComponentModel.DataAnnotations;

namespace shoppingList20.Models
{
    public class Article
    {
        public long ID  { get; set; }
        [Required]
        public string? Name { get; set; }
        public string? Amount { get; set; }
        public string? Remark { get; set; }
        public bool IsBought { get; set; }

       
    }
}
