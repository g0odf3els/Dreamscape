using Dreamscape.Application.Collections;
using Dreamscape.Application.Collections.Queries.GetPagedCollections;
using Dreamscape.Application.Files;
using Dreamscape.Application.Files.Queries.GetPagedFiles;
using Dreamscape.Application.Users.Commands.UpdateUserProfileImage;
using Dreamscape.Application.Users.Queries.GetUser;
using Dreamscape.UI.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

        [Authorize]
        [HttpGet("Settings/Profile")]
        public async Task<IActionResult> SettingsProfile()
        {
            var user = await _mediator.Send(new GetUserQuery(@User.FindFirstValue(ClaimTypes.NameIdentifier)!));
            return View(user);
        }

        [Authorize]
        [HttpGet("Settings/Password")]
        public async Task<IActionResult> SettingsPassword()
        {
            var user = await _mediator.Send(new GetUserQuery(@User.FindFirstValue(ClaimTypes.NameIdentifier)!));
            return View(user);

        }

        [Authorize]
        [HttpPost("Settings/ProfileImage")]
        public async Task<IActionResult> ProfileImage(IFormFile image)
        {
            var result = await _mediator.Send(new UpdateUserProfileImageCommand(image, @User.FindFirstValue(ClaimTypes.NameIdentifier)!));
            return Ok(result);
        }

    }
}
