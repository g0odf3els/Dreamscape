using AutoMapper;
using Dreamscape.Application.Common.Exceptions;
using Dreamscape.Application.Repositories;
using Dreamscape.Domain.Entities;
using MediatR;


namespace Dreamscape.Application.Users.Queries.GetUser
{
    public class GetUserQueryHandler(
        IUserRepository userRepository,
        IMapper mapper) : IRequestHandler<GetUserQuery, UserViewModel>
    {
        readonly IUserRepository _userRepository = userRepository;
        readonly IMapper _mapper = mapper;

        public async Task<UserViewModel> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var result = await _userRepository.GetAsync(
                [u => u.Id == request.Id],
                [u => u.Collections],
                cancellationToken
            ) ?? throw new NotFoundException(nameof(User), request.Id);

            return _mapper.Map<UserViewModel>(result);
        }
    }
}
