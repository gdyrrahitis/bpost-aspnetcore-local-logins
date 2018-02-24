namespace Authentication.Local.Services
{
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Authentication.Local.Models;
    using Microsoft.AspNetCore.Authentication;

    public class ProfileClaimsTransformationService: IClaimsTransformation
    {
        private readonly IUserClaimsService _service;
        private const string Issuer = "Claims.Transformation.Service.Authority";

        public ProfileClaimsTransformationService(IUserClaimsService service) => _service = service;

        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            var identity = principal.Identities.FirstOrDefault();
            if (identity == null)
            {
                return await Task.FromResult<ClaimsPrincipal>(null);
            }

            var identifier = identity.FindFirst(ClaimTypes.NameIdentifier);
            if (identifier == null)
            {
                return principal;
            }

            var userClaims = (await _service.FindUserClaimsByUserNameAsync(identifier.Value)).ToList();
            if (!userClaims.Any())
            {
                return principal;
            }

            var claims = userClaims.Select(c => new Claim(c.Type, c.Value, c.ValueType, GetIssuer(c))).ToList();
            claims.AddRange(identity.Claims);
            var claimsIdentity = new ClaimsIdentity(claims, identity.AuthenticationType);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            return claimsPrincipal;

            string GetIssuer(UserClaims claim)
            {
                return string.IsNullOrWhiteSpace(claim.Issuer) ? Issuer : claim.Issuer;
            }
        }
    }
}