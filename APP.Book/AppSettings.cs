
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace APP.Book
{
    public class AppSettings
    {
        public static string Issuer { get; set; }
        public static string Audience { get; set; }
        public static int ExpirationInMinutes { get; set; }
        public static string SecurityKey { get; set; }
        public static SecurityKey SigningKey => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecurityKey));
    }
}
