using System;
using System.Collections.Generic;
using Microsoft.IdentityModel.Tokens;

namespace Doppler.ImageAnalysisApi.DopplerSecurity
{
    public class DopplerSecurityOptions
    {
        public IEnumerable<SecurityKey> SigningKeys { get; set; } = Array.Empty<SecurityKey>();
    }
}
