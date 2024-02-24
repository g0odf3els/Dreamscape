using Dreamscape.Application.Collections;
using Dreamscape.Application.Collections.Queries.GetPagedCollections;
using Dreamscape.Application.Files;
using Dreamscape.Application.Files.Queries.GetPagedFiles;
using Dreamscape.Application.Users.Queries.GetUser;
using Dreamscape.UI.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Dreamscape.UI.Controllers
{
    [Route("User")]
    public class UserController : Controller
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{userId}/Uploads")]
        public async Task<IActionResult> Uploads(string userId, int page = 1, int pageSize = 16)
        {
            var user = await _mediator.Send(new GetUserQuery(userId));

            return View(new UserProfileViewModel<ImageFileViewModel>()
            {
                User = user,
                Items = await _mediator.Send(new GetPagedFilesQuery()
                {
                    UploaderId = user.Id.ToString(),
                    Page = page,
                    PageSize = pageSize
                })
            });
        }

        [HttpGet("{userId}/Collections")]
        public async Task<IActionResult> Collections(string userId, int page = 1, int pageSize = 16)
        {
            var user = await _mediator.Send(new GetUserQuery(userId));

            return View(new UserProfileViewModel<CollectionViewModel>()
            {
                User = user,
                Items = await _mediator.Send(new GetPagedCollectionsQuery()
                {
                    OwnerId = user.Id.ToString(),
                    Page = page,
                    PageSize = pageSize
                })
            });
        }
    }
}
