namespace Darts.Domain.Abstracts
{
    public interface IPassword
    {
        bool Verify(string password, string hash);
        string Hash(string plainPassword);
    }
}
