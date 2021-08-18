namespace MusicSystem.Services.Partners
{
    public interface IPartnerService
    {
        public string Add(string companyName, string businessEmail,
            string website, string userId);

        public bool IsPartner(string userId);

        public string IdByUser(string userId);

        public bool Exists(string businessEmail);

        public void Delete(string businessEmail);

        public bool CheckForSameEmail(string businessEmail);
    }
}
