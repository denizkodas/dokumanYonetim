namespace dokumanYonetim.Models.Repositories
{
    public interface IDocumentRepository: IRepository<Document>
    {
       
        void Guncelle(Document document);

        Document GetById(int id);
        IEnumerable<Document> GetPendingDocumentsForUser(int userId);
        void Kaydet();

    }
}
