using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace dokumanYonetim.Models
{

    public class Document
    {
        [Key]
        public int DocumentId { get; set; }

        public string? DocumentTitle { get; set; }
        public string? DocumentDescription { get; set; }
        public string? FilePath { get; set; }
        public string? FileType { get; set; }
        public DateTime? UploadDate { get; set; }

        public int? CategoryId { get; set; }      
        public Category Category { get; set; } // Navigation property
        public int? CreatedByUserId { get; set; }
        public User? CreatedByUser { get; set; }

        // Onay sürecini takip etmek için yeni alanlar
        public int? CurrentApproverId { get; set; } // Mevcut onaylayıcı
        public int? HighestManagerId { get; set; } // En üst yönetici
        public bool IsApproved { get; set; } // Dokümanın tamamen onaylanıp onaylanmadığını takip eder

        public ICollection<DocumentAccess>? DocumentAccesses { get; set; }
    }


}
