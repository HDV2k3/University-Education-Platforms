
using UNIVERSITY.EDUCATION.PLATFORMS.Service.Interface;
namespace UNIVERSITY.EDUCATION.PLATFORMS.Service.Implementation
{
    public class BCryptEncryptionService : IBCryptEncryptionService
    {
        private const int WorkFactor = 12; 

        public string HashPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentNullException(nameof(password));

            return BCrypt.Net.BCrypt.HashPassword(password, WorkFactor);
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(hashedPassword))
                return false;

            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}
