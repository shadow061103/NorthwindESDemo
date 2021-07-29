using AutoMapper;
using NorthwindDemo.Repository.Models.Entities;
using NorthwindDemo.Service.Models.Dtos;

namespace NorthwindDemo.Service.Infrastructure.Mapping
{
    public class ServiceProfile : Profile
    {
        public ServiceProfile()
        {
            this.CreateMap<Orders, OrdersDto>();

            this.CreateMap<OrderDetails, OrderDetailsDto>();

            this.CreateMap<Products, ProductsDto>();

            this.CreateMap<Categories, CategoriesDto>();

            this.CreateMap<Suppliers, SuppliersDto>();

            this.CreateMap<Shippers, ShippersDto>();

            this.CreateMap<Customers, CustomersDto>();

            this.CreateMap<Employees, EmployeesDto>();
        }
    }
}