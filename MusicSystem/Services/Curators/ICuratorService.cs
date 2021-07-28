namespace MusicSystem.Services.Curators
{
    public interface ICuratorService
    {
        public bool IsCurator(string userId);

        public int IdByUser(string userId);
    }
}
