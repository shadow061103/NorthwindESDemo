using AutoMapper;
using NorthwindDemo.Api.Models.Parameter;
using NorthwindDemo.Service.Models;

namespace NorthwindDemo.Api.Infrastructure.Mapping
{
    public class ControllerProfile : Profile
    {
        public ControllerProfile()
        {
            this.CreateMap<SearchOrderParameter, SearchOrderDto>();
        }
    }
}