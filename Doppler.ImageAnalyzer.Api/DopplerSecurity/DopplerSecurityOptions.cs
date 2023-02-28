using Microsoft.IdentityModel.Tokens;

namespace Doppler.ImageAnalyzer.Api.DopplerSecurity
{
    public class DopplerSecurityOptions
    {
        public IEnumerable<SecurityKey> SigningKeys { get; set; } = Array.Empty<SecurityKey>();
    }
}
