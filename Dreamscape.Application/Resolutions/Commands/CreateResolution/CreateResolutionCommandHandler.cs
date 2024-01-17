using AutoMapper;
using Dreamscape.Application.Repositories;
using Dreamscape.Domain.Entities;
using MediatR;

namespace Dreamscape.Application.Resolutions.Commands.CreateResolution
{
    public class CreateResolutionCommandHandler
        : IRequestHandler<CreateResolutionCommand, ResolutionViewModel>
    {
        private readonly IResolutionRepository _resolutionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateResolutionCommandHandler(IResolutionRepository resolutionRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _resolutionRepository = resolutionRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResolutionViewModel> Handle(CreateResolutionCommand request, CancellationToken cancellationToken)
        {
            var resolution = _mapper.Map<Resolution>(request);

            _resolutionRepository.Create(_mapper.Map<Resolution>(request));
            await _unitOfWork.Save(cancellationToken);

            return _mapper.Map<ResolutionViewModel>(resolution);
        }
    }
}
