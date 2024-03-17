using Dreamscape.Application.Collections.Commands.AppendFileToCollection;
using Dreamscape.Application.Collections.Commands.CreateCollection;
using Dreamscape.Application.Collections.Commands.DeleteCollection;
using Dreamscape.Application.Collections.Commands.RemoveFileFromCollection;
using Dreamscape.Application.Collections.Queries.GetCollection;
using Dreamscape.Application.Collections.Queries.GetPagedCollections;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Dreamscape.UI.Controllers.Api
{
    [Route("api/Collections")]
    public class CollectionApiController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CollectionApiController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Collection(int page = 1, int pageSize = 16, String? ownerId = null)
        {
            var result = await _mediator.Send(new GetPagedCollectionsQuery() {
                Page = page,
                PageSize = pageSize,
                OwnerId = ownerId,
                Private = false,
            });
            return Ok(result);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("My")]
        public async Task<IActionResult> MyCollections(int page = 1, int pageSize = 16)
        {
            var result = await _mediator.Send(new GetPagedCollectionsQuery()
            {
                Page = page,
                PageSize = pageSize,
                OwnerId = User.FindFirstValue(ClaimTypes.NameIdentifier)!,
                Private = true,
            }); return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Collection(string id)
        {
            var result = await _mediator.Send(new GetCollectionQuery(id));
            return Ok(result);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("Create")]
        public async Task<IActionResult> Create(string name, string description, bool isPublic, string[] filesId)
        {
            var result = await _mediator.Send(new CreateCollectionCommand(User.FindFirstValue(ClaimTypes.NameIdentifier)!, name, description, isPublic, filesId));
            return Ok(result);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _mediator.Send(new DeleteCollectionCommand(User.FindFirstValue(ClaimTypes.NameIdentifier)!, id));
            return Ok(result);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("{collectionId}/append/{fileId}")]
        public async Task<IActionResult> Append(string collectionId, string fileId)
        {
            await _mediator.Send(new AppendFileToCollectionCommand(User.FindFirstValue(ClaimTypes.NameIdentifier)!, collectionId, fileId));
            return Ok();
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("{collectionId}/remove/{fileId}")]
        public async Task<IActionResult> Remove(string collectionId, string fileId)
        {
            await _mediator.Send(new RemoveFileFromCollectionCommand(User.FindFirstValue(ClaimTypes.NameIdentifier)!, collectionId, fileId));
            return Ok();
        }
    }
}
