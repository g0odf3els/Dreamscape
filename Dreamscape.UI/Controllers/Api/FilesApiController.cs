using Dreamscape.Application.Files.Commands.CreateFile;
using Dreamscape.Application.Files.Commands.DeleteFile;
using Dreamscape.Application.Files.Queries;
using Dreamscape.Application.Files.Queries.GetFile;
using Dreamscape.Application.Files.Queries.GetPagedFiles;
using Dreamscape.Application.Files.Queries.GetSimilarPagedFiles;
using Dreamscape.UI.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

namespace Dreamscape.API.Controllers
{
    [Route("api/Files")]
    [ApiController]
    public class FilesApiController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FilesApiController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<FileViewModel>>> Files([FromQuery] GetPagedFilesQuery request, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);
            return Ok(response);
        }

        [HttpGet("Similar")]
        public async Task<ActionResult<PagedList<FileViewModel>>> SimilarFiles([FromQuery] GetSimilarPagedFilesQuery request, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FileViewModel>> File([FromRoute] string id, CancellationToken cancellationToken)
        {
            var file = await _mediator.Send(new GetFileQuery(id), cancellationToken);
            return Ok(file);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("Upload")]
        public async Task<IActionResult> Upload([FromForm] IFormFile upload, [FromForm] string[] tagList, CancellationToken cancellationToken)
        {
            var request = new CreateFileCommand(upload, User.FindFirstValue(ClaimTypes.NameIdentifier)!, tagList);
            return Ok(await _mediator.Send(request, cancellationToken));
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] string id, CancellationToken cancellationToken)
        {
            var request = new DeleteFileCommand(User.FindFirstValue(ClaimTypes.NameIdentifier)!, id);
            return Ok(await _mediator.Send(request, cancellationToken));
        }
    }
}

