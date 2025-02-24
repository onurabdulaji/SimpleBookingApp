using MediatR;
using SimpleBookingApp.Application.Features.Resources.ListResources.Dtos.GetResourcesDTO;
using SimpleBookingApp.Application.Interfaces;

namespace SimpleBookingApp.Application.Features.Resources.ListResources.Commands.GetResourcesList
{
    public class GetResourceListQueryHandler : IRequestHandler<GetResourceListQuery, List<GetResourceResponseDto>>
    {
        private readonly IResourceRepository _resourceRepository;

        public GetResourceListQueryHandler(IResourceRepository resourceRepository)
        {
            _resourceRepository = resourceRepository;
        }

        public async Task<List<GetResourceResponseDto>> Handle(GetResourceListQuery request, CancellationToken cancellationToken)
        {
            // Veritabanından tüm kaynakları alıyoruz
            var resources = await _resourceRepository.GetAllAsync();

            // Response formatına dönüştürüyoruz
            return resources.Select(r => new GetResourceResponseDto
            {
                Id = r.Id,
                Name = r.Name,
                Quantity = r.Quantity
            }).ToList();
        }
    }
}
