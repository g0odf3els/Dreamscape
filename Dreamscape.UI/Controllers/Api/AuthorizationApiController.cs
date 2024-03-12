using Dreamscape.Application.Users;
using Dreamscape.Application.Users.Commands.CreateUser;
using Dreamscape.Application.Users.Commands.GenerateJwtToken;
using Dreamscape.Application.Users.Commands.GenerateRefreshToken;
using Dreamscape.Application.Users.Queries.GetUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Dreamscape.API.Controllers
{
    [Route("api/Authorization")]
    public class AuthorizationApiController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthorizationApiController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult<UserViewModel>> Register(CreateUserCommand request,
            CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);
            return Ok(response);
        }

        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult<CreateJwtCommandView>> Login(CreateJwtCommand request,
          CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);
            return Ok(response);
        }

        [HttpPost]
        [Route("Refresh")]
        public async Task<ActionResult<GenerateRefreshTokenView>> Refresh(GenerateRefreshTokenCommand requset,
            CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(requset, cancellationToken);
            return Ok(response);
        }
    }
}
