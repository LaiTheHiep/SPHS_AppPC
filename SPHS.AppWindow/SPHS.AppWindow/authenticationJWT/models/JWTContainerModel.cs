using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace AuthenticationService.Models
{
    public class JWTContainerModel : IAuthContainerModel
    {
        #region Public Methods
        public int ExpireMinutes { get; set; } = 24 * 60; // 1 days.
        public string SecretKey { get; set; } = "SGF0IERhdSBOaG8gLSBNYW5jaGVzdGVyIFVuaXRlZA==";
        public string SecurityAlgorithm { get; set; } = SecurityAlgorithms.HmacSha256Signature;

        public Claim[] Claims { get; set; }
        #endregion
    }
}