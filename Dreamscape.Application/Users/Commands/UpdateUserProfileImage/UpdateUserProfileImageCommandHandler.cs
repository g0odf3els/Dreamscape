﻿using MediatR;
using Dreamscape.Application.Repositories;
using Dreamscape.Application.Common.Exceptions;
using Dreamscape.Domain.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using ImageMagick;

namespace Dreamscape.Application.Users.Commands.UpdateUserProfileImage
{
    public class UpdateUserProfileImageCommandHandler(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IWebHostEnvironment webHostEnvironment)
        : IRequestHandler<UpdateUserProfileImageCommand, UserViewModel>
    {
        readonly IUserRepository _userRepository = userRepository;
        readonly IUnitOfWork _unitOfWork = unitOfWork;
        readonly IMapper _mapper = mapper;
        readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;
        public async Task<UserViewModel> Handle(UpdateUserProfileImageCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetAsync(
               [u => u.Id == request.UserId],
               [u => u.Collections],
               cancellationToken
            ) ?? throw new NotFoundException(nameof(User), request.UserId);

            var uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "userProfileImages");

            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            var uniqueFileName = $"{user.Id}{Path.GetExtension(request.File.FileName)}";
            var filePath = Path.Combine(uploadPath, uniqueFileName);

            using (MagickImage image = new MagickImage(request.File.OpenReadStream()))
            {
                int _previewWidth = 450;
                int _previewHeight = 450;

                image.Resize(new MagickGeometry(_previewWidth, _previewHeight)
                {
                    FillArea = true
                });

                image.Extent(_previewWidth, _previewHeight, Gravity.Center);
                image.Write(filePath);
            }

            user.UserProfileImagePath = "/userProfileImages/" + uniqueFileName;
            await unitOfWork.Save(cancellationToken);

            return _mapper.Map<UserViewModel>(user);
        }
    }
}
