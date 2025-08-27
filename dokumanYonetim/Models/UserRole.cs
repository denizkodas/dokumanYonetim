using System.ComponentModel.DataAnnotations;
using System.Data;

namespace dokumanYonetim.Models
{
    public class UserRole
    {
        [Key]
        public int UserRoleId { get; set; } // Primary Key, genellikle bu şekilde birincil anahtar kullanılır.

        public int UserId { get; set; } // Foreign Key
        public User User { get; set; } // Navigation property for User

        public int RoleId { get; set; } // Foreign Key
        public Role Role { get; set; } // Navigation property for Role
    }
}
