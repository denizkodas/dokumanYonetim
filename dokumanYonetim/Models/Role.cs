namespace dokumanYonetim.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }  // Örneğin: "Reader", "Writer", "Admin"
        public List<UserRole> UserRoles { get; set; }

    }
}
