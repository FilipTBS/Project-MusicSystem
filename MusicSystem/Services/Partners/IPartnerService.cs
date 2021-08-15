namespace MusicSystem.Services.Partners
{
    public interface IPartnerService
    {
        public bool IsPartner(string userId);

        public string IdByUser(string userId);

        public bool Exists(string businessEmail);

        public void Delete(string businessEmail);
    }
}
