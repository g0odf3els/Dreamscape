using Dreamscape.Application.Collections.Commands.AppendFileToCollection;
using Dreamscape.Application.Collections.Commands.CreateCollection;
using Dreamscape.Application.Collections.Queries.GetCollection;
using Dreamscape.Application.Collections.Queries.GetPagedCollections;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Dreamscape.UI.Controllers.Api
{
    [Route("api/Collection")]
    public class CollectionApiController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CollectionApiController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("Collections")]
        public async Task<IActionResult> Collection(GetPagedCollectionsQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("Collection/{id}")]
        public async Task<IActionResult> Collection(string id)
        {
            var result = await _mediator.Send(new GetCollectionQuery(id));
            return Ok(result);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("Create")]
        public async Task<IActionResult> Create(string name)
        {
            var result = await _mediator.Send(new CreateCollectionCommand(User.FindFirstValue(ClaimTypes.NameIdentifier), name));
            return Ok(result);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("Append")]
        public async Task<IActionResult> Append(string collectionId, string fileId)
        {
            await _mediator.Send(new AppendFileToCollectionCommand(User.FindFirstValue(ClaimTypes.NameIdentifier), collectionId, fileId));
            return Ok();
        }
    }
}
