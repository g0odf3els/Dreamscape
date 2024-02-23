using Dreamscape.Application.Collections.Queries.GetPagedCollections;
using Dreamscape.Application.Files.Queries.GetPagedFiles;
using Dreamscape.Application.Users.Queries.GetUser;
using Dreamscape.UI.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Dreamscape.UI.Controllers
{
    public class UserController : Controller
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("Profile/{id}")]
        public async Task<IActionResult> Profile(GetUserQuery query)
        {
            var user = await _mediator.Send(query);

            return View(new UserProfileViewModel()
            {
                User = user,
                RecentUploads = await _mediator.Send(new GetPagedFilesQuery()
                {
                    UploaderId = user.Id.ToString(),
                    PageSize = 16
                })
            });
        }
    }
}
