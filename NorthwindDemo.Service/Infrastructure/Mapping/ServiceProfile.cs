using AutoMapper;
using NorthwindDemo.Repository.Models.Entities;
using NorthwindDemo.Repository.Models.ES;
using NorthwindDemo.Service.Models;
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

            //ES

            this.CreateMap<OrdersDto, OrdersESModel>().ReverseMap();

            this.CreateMap<OrderDetailsDto, OrderDetailsESModel>().ReverseMap();

            this.CreateMap<ProductsDto, ProductsESModel>().ReverseMap();

            this.CreateMap<CategoriesDto, CategoriesESModel>().ReverseMap();

            this.CreateMap<SuppliersDto, SuppliersESModel>().ReverseMap();

            this.CreateMap<ShippersDto, ShippersESModel>().ReverseMap();

            this.CreateMap<CustomersDto, CustomersESModel>().ReverseMap();

            this.CreateMap<EmployeesDto, EmployeesESModel>().ReverseMap();

            this.CreateMap<SearchOrderDto, SearchESModel>();
        }
    }
}