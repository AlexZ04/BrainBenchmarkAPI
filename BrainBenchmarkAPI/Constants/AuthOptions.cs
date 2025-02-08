using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BrainBenchmarkAPI.Tokens
{
    public class AuthOptions
    {
        public const string ISSUER = "BrainBenchmarkAPIServer";
        public const string AUDIENCE = "BrainBenchmarkAPIClient";
        public const string KEY = "superKeyPuPuPu_52#Peterburg-eeeAaALike";
        public const int LIFETIME_MINUTES = 60 * 24;

        public static SymmetricSecurityKey GetSymmetricSecurityKey() => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
    }
}
