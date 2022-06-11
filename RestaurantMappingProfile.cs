using AutoMapper;
using Radzio.Entites;
using Radzio.Models;

namespace Radzio
{
    public class RestaurantMappingProfile : Profile
    {
        public RestaurantMappingProfile()
        {
            CreateMap<Restaurant, RestaurantDto>()
                .ForMember(m => m.City, c => c.MapFrom(s => s.Address.City))
                .ForMember(m => m.Street, c => c.MapFrom(s => s.Address.Street))
                .ForMember(m => m.PostalCode, c => c.MapFrom(s => s.Address.PostalCode));

            CreateMap<Dish, DishDto>();

            CreateMap<RestaurantDto, Restaurant>()
                .ForMember(r => r.Address, c => c.MapFrom(dto => new Address()
                    { City = dto.City, Street = dto.Street }));

            CreateMap<CreateDishDto, Dish>(); // mapowanie z klasy DishDto do klasy dish
        }
    }
}
