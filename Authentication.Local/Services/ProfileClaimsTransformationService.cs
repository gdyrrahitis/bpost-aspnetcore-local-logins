namespace Authentication.Local.Services
{
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authentication;

    public class ProfileClaimsTransformationService: IClaimsTransformation
    {
        private readonly IUserClaimsService _service;

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

            var userClaims = (await _service.FindUserClaimsByUsername(identifier.Value)).ToList();
            if (!userClaims.Any())
            {
                return principal;
            }

            var claims = userClaims.Select(c => new Claim(c.Type, c.Value, c.ValueType, c.Issuer)).ToList();
            claims.AddRange(identity.Claims);

            var claimsIdentity = new ClaimsIdentity(claims, identity.AuthenticationType);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            return claimsPrincipal;
        }
    }
}