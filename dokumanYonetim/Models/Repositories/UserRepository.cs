using dokumanYonetim.Data;
using Microsoft.EntityFrameworkCore;

namespace dokumanYonetim.Models.Repositories
{
    public class UserRepository:Repository<User>, IUserRepository
    {
        private ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Guncelle(User user)
        {
            _context.Update(user);
        }

        public void Kaydet()
        {
            _context.SaveChanges();
        }


    }
}
