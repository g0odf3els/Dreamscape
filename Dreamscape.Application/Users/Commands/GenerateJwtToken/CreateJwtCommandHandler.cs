using AutoMapper;
using Dreamscape.Application.Common.Exceptions;
using Dreamscape.Application.Repositories;
using Dreamscape.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;

namespace Dreamscape.Application.Users.Commands.GenerateJwtToken
{

    internal class CreateJwtCommandHandler : IRequestHandler<CreateJwtCommand, CreateJwtCommandView>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public CreateJwtCommandHandler(
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IConfiguration configuration)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _config = configuration;
        }

        public async Task<CreateJwtCommandView> Handle(CreateJwtCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetAsync(
                [u => u.UserName == request.Username],
                null,
                cancellationToken
            ) ?? throw new NotFoundException(nameof(User), request.Username);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            var token = JwtTokenHelper.GenerateJwtToken(claims, _config);

            var refreshToken = JwtTokenHelper.GenerateRefreshToken(claims, _config);

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTimeOffset.UtcNow.Add(TimeSpan.FromDays(7));

            _userRepository.Update(user);
            await _unitOfWork.Save(cancellationToken);

            return new CreateJwtCommandView
            {
                Id = user.Id,
                Username = user.UserName,
                AccessToken = token,
                RefreshToken = refreshToken
            };
        }
    }
}
