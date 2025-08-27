using System.ComponentModel.DataAnnotations;

namespace dokumanYonetim.Models
{
    public class User 
    {
      
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; }
        public int? ManagerId { get; set; } // Yöneticinin UserId'sini tutar
        public User? Manager { get; set; }  // Yöneticinin kendisini temsil eder
        public List<Document> DocumentsToApprove { get; set; } = new List<Document>();

        public ICollection<Document>? Documents { get; set; } //bir kullanıcı birden fazla belge saklayabilir.
        public ICollection<DocumentAccess>? DocumentAccesses { get; set; } //bir kullanıcı birden fazla belgeye erişebilir.
        

    }
}
