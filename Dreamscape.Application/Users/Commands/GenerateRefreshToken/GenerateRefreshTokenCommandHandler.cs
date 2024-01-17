using AutoMapper;
using Dreamscape.Application.Repositories;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Dreamscape.Application.Users.Commands.GenerateRefreshToken
{
    public class GenerateRefreshTokenCommandHandler(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IConfiguration config)
        : IRequestHandler<GenerateRefreshTokenCommand, GenerateRefreshTokenView>
    {
        readonly IUserRepository _userRepository = userRepository;
        readonly IUnitOfWork _unitOfWork = unitOfWork;
        readonly IMapper _mapper = mapper;
        readonly IConfiguration _config = config;

        public async Task<GenerateRefreshTokenView> Handle(GenerateRefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["AuthOptions:Key"])),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(request.AccessToken, tokenValidationParameters, out securityToken);

            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            var username = principal.Identity.Name;

            var user = await _userRepository.GetAsync([u => u.UserName == username], null, cancellationToken);

            if (user is null || user.RefreshToken != request.RefreshToken)
                throw new SecurityTokenException("Invalid request");

            _userRepository.Update(user);
            await _unitOfWork.Save(cancellationToken);

            var claims = new List<Claim>
            {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            var newRefreshToken = JwtTokenHelper.GenerateRefreshToken(claims, _config);

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);

            var token = JwtTokenHelper.GenerateJwtToken(claims, _config);

            return new GenerateRefreshTokenView()
            {
                AccessToken = token,
                RefreshToken = newRefreshToken
            };
        }
    }
}
