﻿using Dreamscape.Application.Collections.Commands.AppendFileToCollection;
using Dreamscape.Application.Collections.Commands.AutoAppendFileToCollection;
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

        [HttpGet]
        [HttpGet("Collections")]
        public async Task<IActionResult> Collections(GetPagedCollectionsQuery request)
        {
            var collections = await _mediator.Send(request);

            return View(collections);
        }

        [Authorize]
        [HttpGet("User")]
        public async Task<IActionResult> GetUserCollections()
        {
            var collections = await _mediator.Send(new GetPagedCollectionsQuery()
            {
                OwnerId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                SortOrder = 2,
            });

            return Ok(collections);
        }

        [Authorize]
        [HttpGet("Manage")]
        public async Task<IActionResult> Manage(GetPagedCollectionsQuery request)
        {
            var collections = await _mediator.Send(new GetPagedCollectionsQuery()
            {
                OwnerId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                Page = request.Page,
                PageSize = request.PageSize
            });

            return View(collections);
        }

        [Authorize]
        [HttpPost("Create")]
        public async Task<IActionResult> Create(string name, string description, bool isPrivate = false, string[]? filesId = null)
        {
            await _mediator.Send(new CreateCollectionCommand(User.FindFirstValue(ClaimTypes.NameIdentifier)!, name, description, isPrivate, filesId));
            return Ok();
        }

        [Authorize]
        [HttpPost("Append")]
        public async Task<IActionResult> Append(string collectionId, string fileId)
        {
            await _mediator.Send(new AppendFileToCollectionCommand(User.FindFirstValue(ClaimTypes.NameIdentifier)!, collectionId, fileId));
            return Ok();
        }

        [Authorize]
        [HttpPost("AutoAppend")]
        public async Task<IActionResult> AutoAppend(string fileId)
        {
            await _mediator.Send(new AutoAppendFileToCollectionCommand(User.FindFirstValue(ClaimTypes.NameIdentifier)!, fileId));
            return Ok();
        }

        [Authorize]
        [HttpPost("Remove")]
        public async Task<IActionResult> Remove(string collectionId, string fileId)
        {
            await _mediator.Send(new RemoveFileFromCollectionCommand(User.FindFirstValue(ClaimTypes.NameIdentifier)!, collectionId, fileId));
            return Ok();
        }

        [Authorize]
        [HttpPost("Delete{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _mediator.Send(new DeleteCollectionCommand(User.FindFirstValue(ClaimTypes.NameIdentifier)!, id));
            return Ok();
        }
    }
}
