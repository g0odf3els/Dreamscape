using AutoMapper;
using Dreamscape.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;

namespace Dreamscape.Application.Users.Commands.CreateUser
{
    internal class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, CreateUserView>
    {
        readonly UserManager<User> _userManager;
        readonly IConfiguration _config;
        readonly IMapper _mapper;

        public CreateUserCommandHandler(UserManager<User> userManager, IConfiguration configuration, IMapper mapper)
        {
            _userManager = userManager;
            _config = configuration;
            _mapper = mapper;
        }

        public async Task<CreateUserView> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = _mapper.Map<User>(request);
            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                throw new Exception("Error");
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            var token = JwtTokenHelper.GenerateJwtToken(claims, _config);
            var refreshToken = JwtTokenHelper.GenerateRefreshToken(claims, _config);

            return new CreateUserView()
            {
                Id = user.Id,
                Username = user.UserName,
                AccessToken = token,
                RefreshToken = refreshToken
            };
        }
    }
}
