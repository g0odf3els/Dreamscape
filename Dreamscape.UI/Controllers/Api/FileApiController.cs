using Dreamscape.Application.Files.Commands.CreateFile;
using Dreamscape.Application.Files.Queries;
using Dreamscape.Application.Files.Queries.GetFile;
using Dreamscape.Application.Files.Queries.GetPagedFiles;
using Dreamscape.UI.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Dreamscape.API.Controllers
{
    [Route("api/File")]
    public class FileApiController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FileApiController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Route("Files")]
        [HttpGet]
        public async Task<ActionResult<PagedList<FileViewModel>>> Files(GetPagedFilesQuery request, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);
            return Ok(response);
        }

        [Route("File")]
        [HttpGet]
        public async Task<ActionResult<FileViewModel>> File(GetFileQuery request, CancellationToken cancellationToken)
        {
            return Ok(await _mediator.Send(request, cancellationToken));
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [Route("Upload")]
        [HttpPost]
        public async Task<IActionResult> CreateFile(IFormFile upload, string[] tagList, CancellationToken cancellationToken)
        {
            var request = new CreateFileCommand(upload, User.FindFirstValue(ClaimTypes.NameIdentifier), tagList);

            return Ok(await _mediator.Send(request, cancellationToken));
        }
    }
}
