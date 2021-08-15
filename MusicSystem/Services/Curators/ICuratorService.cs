namespace MusicSystem.Services.Curators
{
    public interface ICuratorService
    {
        public string Add(string nickname, string email, string userId);

        public bool IsCurator(string userId);

        public string IdByUser(string userId);

        public bool Exists(string email);

        public void Delete(string email);
    }
}