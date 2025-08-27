using System.Linq.Expressions;

namespace dokumanYonetim.Models.Repositories
{
    public interface IRepository<T> where T : class // T bir classdır.(Document,User gibi)
    {
        IQueryable<T> GetAll ();
        T Get(Expression<Func<T, bool>> filtre); //filtreleme için(id si şu olanı getir gibi)
        void Ekle(T entity);
        void Sil(T entity);
        void SilAralik(IEnumerable<T> entities); //Belli bir aralıktaki verileri sil
        void Update(T entity);
        void SaveChanges();

    }
}
