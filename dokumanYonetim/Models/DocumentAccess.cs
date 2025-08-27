using System.ComponentModel.DataAnnotations;

namespace dokumanYonetim.Models
{
    public class DocumentAccess
    {
        [Key]
        public int DocumentAccessId { get; set; } // Primary Key

        public int DocumentId { get; set; } // Foreign Key
        public int UserId { get; set; } // Foreign Key
        public string? AccessLevel { get; set; } // Erişim Düzeyi (View, Edit, Delete)

        // Navigation properties
        public Document Document { get; set; } // Belge Referansı
        public User User { get; set; } // Kullanıcı Referansı

    }
}
