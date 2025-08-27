using dokumanYonetim.Data;

namespace dokumanYonetim.Models.Repositories
{
    public class CategoryRepository:Repository<Category>
    {
        private ApplicationDbContext _context;
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
