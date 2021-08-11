namespace MusicSystem.Services.Partners
{
    public interface IPartnerService
    {
        public bool IsPartner(string userId);

        public string IdByUser(string userId);
    }
}
