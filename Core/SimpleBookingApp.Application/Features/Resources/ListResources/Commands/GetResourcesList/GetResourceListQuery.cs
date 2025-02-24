using MediatR;
using SimpleBookingApp.Application.Features.Resources.ListResources.Dtos.GetResourcesDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleBookingApp.Application.Features.Resources.ListResources.Commands.GetResourcesList
{
    public class GetResourceListQuery : IRequest<List<GetResourceResponseDto>>
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Quantity { get; set; }
    }
}
