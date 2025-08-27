using dokumanYonetim.Data;
using Microsoft.EntityFrameworkCore;

namespace dokumanYonetim.Models.Repositories
{
    public class DocumentRepository : Repository<Document>, IDocumentRepository  //önce class, sonra interface
    {
        private ApplicationDbContext _context;
        private readonly DbSet<Document> _documents;
        public DocumentRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
            _documents = _context.Documents;
        }

        public void Guncelle(Document document)
        {
          _context.Update(document);
        }

        public void Kaydet()
        {
            _context.SaveChanges(); 
        }

        public Document GetById(int id)
        {
            return _documents.Find(id);
        }

        public IEnumerable<Document> GetPendingDocumentsForUser(int userId)
        {
            return _documents.Where(d => d.IsApproved == false && d.CreatedByUserId == userId).ToList();
        }


    }
}
