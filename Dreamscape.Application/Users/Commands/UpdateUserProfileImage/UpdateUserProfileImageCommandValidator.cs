using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dreamscape.Application.Users.Commands.UpdateUserProfileImage
{
    public class UpdateUserProfileImageCommandValidator : AbstractValidator<UpdateUserProfileImageCommand>
    {
        public UpdateUserProfileImageCommandValidator()
        {
        }
    }
}
