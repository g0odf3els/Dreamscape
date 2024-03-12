using Dreamscape.Application.Files.Queries.GetFile;
using Dreamscape.Application.Users.Queries.GetUser;
using Dreamscape.UI.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Dreamscape.UI.Controllers.Api
{
    [Route("api/Users")]
    public class UserApiController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserApiController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        public new async Task<ActionResult<FileViewModel>> User(GetUserQuery request, CancellationToken cancellationToken)
        {
            return Ok(await _mediator.Send(request, cancellationToken));
        }
    }
}
