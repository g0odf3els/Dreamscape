using Dreamscape.Application.Collections;
using Dreamscape.Application.Collections.Queries.GetPagedCollections;
using Dreamscape.Application.Files;
using Dreamscape.Application.Files.Queries.GetPagedFiles;
using Dreamscape.Application.Users.Commands.UpdateUserProfileImage;
using Dreamscape.Application.Users.Queries.GetUser;
using Dreamscape.Domain.Entities;
using Dreamscape.UI.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Dreamscape.UI.Controllers
{
    [Route("User")]
    public class UserController : Controller
    {
        private readonly IMediator _mediator;
        private readonly UserManager<User> _userManager;

        public UserController(IMediator mediator, UserManager<User>  userManager)
        {
            _mediator = mediator;
            _userManager = userManager;
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
            return View(new UpdatePasswordViewModel() { User = user });
        }

        [Authorize]
        [HttpPost("Settings/Password")]
        public async Task<IActionResult> SettingsPassword(UpdatePasswordViewModel updatePasswordViewModel)
        {
            var userViewModel = await _mediator.Send(new GetUserQuery(@User.FindFirstValue(ClaimTypes.NameIdentifier)!));
            updatePasswordViewModel.User = userViewModel;

            if (!ModelState.IsValid)
                return View(updatePasswordViewModel);

            var user = await _userManager.GetUserAsync(User);

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, updatePasswordViewModel.Password, updatePasswordViewModel.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }
                return View();
            }

           return RedirectToAction("Index", "Home");
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
