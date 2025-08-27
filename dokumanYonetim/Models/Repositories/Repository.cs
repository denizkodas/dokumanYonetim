using dokumanYonetim.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace dokumanYonetim.Models.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        internal DbSet<T> dbSet; //dbSet= _context.Documents

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            this.dbSet=_context.Set<T>();
        }
        public void Ekle(T entity)
        {
            dbSet.Add(entity);
        }

        public T Get(Expression<Func<T, bool>> filtre)
        {
            IQueryable<T> sorgu = dbSet;
            sorgu = sorgu.Where(filtre); // örneğin id si 5 ile 10 arasında olan nesneleri getirir ama bize bir tane nesne lazım,
            return sorgu.FirstOrDefault(); // o yüzden first or default kullanırız.
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }
        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public IQueryable<T> GetAll()
        {
            return dbSet;
        }

        public void Sil(T entity)
        {
           dbSet.Remove(entity);
        }

        public void SilAralik(IEnumerable<T> entities)
        {
            dbSet.RemoveRange(entities); //birden fazla kaydı silmek için
        }
    }
}
