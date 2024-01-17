using Dreamscape.Application.Features.UserFeatures.CreateUser;
using Dreamscape.Domain.Entities;
using Dreamscape.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WallpaperPortal.Controllers
{
    public class UserController : Controller
    {
        private readonly IMediator _mediator;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public UserController(IMediator mediator, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _mediator = mediator;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(CreateUserCommand command, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(command, cancellationToken);

            if (response.Success)
            {
                return RedirectToAction("Login", "User");
            }

            foreach (var error in response.Errors)
            {
                ModelState.AddModelError(string.Empty, error);
            }

            return View(command);
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            return View(new LoginViewModel
            {
                ReturnUrl = returnUrl
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, false);

            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                {
                    return Redirect(model.ReturnUrl);
                }
                else
                {
                    return RedirectToAction("Login", "User");
                }
            }

            ModelState.AddModelError("", "Incorrect login and/or password");

            return View(model);
        }
    }
}
