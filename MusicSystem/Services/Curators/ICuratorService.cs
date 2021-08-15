namespace MusicSystem.Services.Curators
{
    public interface ICuratorService
    {
        public bool IsCurator(string userId);

        public string IdByUser(string userId);

        public bool Exists(string email);

        public void Delete(string email);
    }
}
