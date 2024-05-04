using Dreamscape.Application.Files.Commands.AddTagToFile;
using Dreamscape.Application.Files.Commands.CreateFile;
using Dreamscape.Application.Files.Commands.DeleteFile;
using Dreamscape.Application.Files.Commands.RemoveTagFromFile;
using Dreamscape.Application.Files.Queries.GetFile;
using Dreamscape.Application.Files.Queries.GetPagedFiles;
using Dreamscape.Application.Files.Queries.GetSimilarPagedFiles;
using Dreamscape.UI.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Dreamscape.Controllers
{
    [Route("Files")]
    public class FileController : Controller
    {
        private readonly IMediator _mediator;

        public FileController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Files(GetPagedFilesQuery query)
        {
            var result = await _mediator.Send(query);
            return View(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> File(GetFileQuery query)
        {
            var image = await _mediator.Send(query);
            var similarFiles = await _mediator.Send(new GetSimilarPagedFilesQuery() { FileId = image.Id.ToString(), PageSize = 6 });
            var isInUserCollection = false;

            return View(new FileViewModel()
            {
                Image = image,
                SimilarImages = similarFiles,
                IsInUserCollection = isInUserCollection
            });
        }

        [Authorize]
        [HttpGet("Upload")]
        public IActionResult Upload()
        {
            return View();
        }

        [Authorize]
        [HttpPost("Upload")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(IFormFile[] upload, string[] tagList, CancellationToken cancellationToken)
        {
            foreach (var file in upload)
            {
                var request = new CreateFileCommand(file, User.FindFirstValue(ClaimTypes.NameIdentifier), tagList);
                await _mediator.Send(request, cancellationToken);
            }

            return RedirectToAction("Files", "File");
        }

        [Authorize]
        [HttpPost("{id}/Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            await _mediator.Send(new DeleteFileCommand(User.FindFirstValue(ClaimTypes.NameIdentifier), id));

            return RedirectToAction("Files", "File");
        }

        [HttpPost("{id}/Download")]
        public async Task<IActionResult> Download(GetFileQuery request)
        {
            var file = await _mediator.Send(request);

            return file == null ? NotFound() : File(file.FullSizePath, "text/plain", file.Name);
        }

        [HttpGet("{id}Resize")]
        public async Task<IActionResult> Resize(string id)
        {
            var file = await _mediator.Send(new GetFileQuery(id));

            return file == null ? NotFound() : View(file);
        }

        [Authorize]
        [HttpPost("AddTag")]
        public async Task<IActionResult> AddTag(string fileId, string tagName)
        {
            await _mediator.Send(new AddTagToFileCommand(User.FindFirstValue(ClaimTypes.NameIdentifier), fileId, tagName));

            return RedirectToAction("File", "File", new { Id = fileId });
        }

        [Authorize]
        [HttpPost("RemoveTag")]
        public async Task<IActionResult> RemoveTag(string fileId, string tagName)
        {
            await _mediator.Send(new RemoveTagFromFileCommand(User.FindFirstValue(ClaimTypes.NameIdentifier), fileId, tagName));

            return RedirectToAction("File", "File", new { Id = fileId });
        }
    }
}