using Dreamscape.Application.Collections.Commands.AppendFileToCollection;
using Dreamscape.Application.Collections.Commands.CreateCollection;
using Dreamscape.Application.Collections.Commands.DeleteCollection;
using Dreamscape.Application.Collections.Commands.RemoveFileFromCollection;
using Dreamscape.Application.Collections.Queries.GetPagedCollections;
using Dreamscape.Application.Users.Queries.GetUser;
using Dreamscape.UI.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Dreamscape.UI.Controllers
{
    [Route("Collection")]
    public class CollectionController : Controller
    {
        private readonly IMediator _mediator;

        public CollectionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize]
        [HttpPost("Create")]
        public async Task<IActionResult> Create(string name)
        {
            await _mediator.Send(new CreateCollectionCommand(User.FindFirstValue(ClaimTypes.NameIdentifier), name));
            return Ok();
        }

        [Authorize]
        [HttpGet("ManageCollections")]
        public async Task<IActionResult> ManageCollections()
        {
            var collections = await _mediator.Send(new GetPagedCollectionsQuery()
            {
                OwnerId = User.FindFirstValue(ClaimTypes.NameIdentifier)
            });

            var user = await _mediator.Send(new GetUserQuery(User.FindFirstValue(ClaimTypes.NameIdentifier)));

            return View(new ManageCollectionsViewModel()
            {
                Collections = collections,
                User = user
            });
        }

        [Authorize]
        [HttpGet("UserCollections")]
        public async Task<IActionResult> UserCollections()
        {
            var collections = await _mediator.Send(new GetPagedCollectionsQuery()
            {
                OwnerId = User.FindFirstValue(ClaimTypes.NameIdentifier)
            });

            return Ok(collections);
        }

        [Authorize]
        [HttpPost("Append")]
        public async Task<IActionResult> Append(string collectionId, string fileId)
        {
            await _mediator.Send(new AppendFileToCollectionCommand(User.FindFirstValue(ClaimTypes.NameIdentifier), collectionId, fileId));
            return Ok();
        }

        [Authorize]
        [HttpPost("Remove")]
        public async Task<IActionResult> Remove(string fileId)
        {
            await _mediator.Send(new RemoveFileFromCollectionCommand(User.FindFirstValue(ClaimTypes.NameIdentifier), fileId));
            return RedirectToAction("File", "File", new { Id = fileId });
        }

        [Authorize]
        [HttpPost("Delete{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _mediator.Send(new DeleteCollectionCommand(User.FindFirstValue(ClaimTypes.NameIdentifier), id));
            return RedirectToAction("MangeCollections", "Collection");
        }
    }
}
