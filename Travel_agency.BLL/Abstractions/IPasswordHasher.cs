namespace Travel_agency.BLL.Abstractions
{
    public interface IPasswordHasher
    {
        string GenerateHash(string password);
        bool VerifyHash(string password, string hash);
    }
}