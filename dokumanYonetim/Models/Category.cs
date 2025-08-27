using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;

namespace dokumanYonetim.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }

        public ICollection<Document>? Documents { get; set; }

    }
}
