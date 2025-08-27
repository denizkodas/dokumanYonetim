namespace dokumanYonetim.Models.Repositories
{
    public interface IUserRepository: IRepository<User>
    {
        void Guncelle(User user);
        void Kaydet();

       
    }
}
