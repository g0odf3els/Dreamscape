using AutoMapper;
using Dreamscape.Application.Users.Commands.CreateUser;
using Dreamscape.Domain.Entities;

namespace Dreamscape.Application.Users
{
    internal class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<CreateUserCommand, User>();
            CreateMap<User, UserViewModel>();
        }
    }
}
