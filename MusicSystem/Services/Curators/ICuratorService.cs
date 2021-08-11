namespace MusicSystem.Services.Curators
{
    public interface ICuratorService
    {
        public bool IsCurator(string userId);

        public string IdByUser(string userId);
    }
}
