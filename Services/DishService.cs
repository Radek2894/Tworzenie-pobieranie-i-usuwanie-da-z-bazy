using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Radzio.Entites;
using Radzio.Exceptions;
using Radzio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Radzio.Services
{
    public interface IDishService // utworzenie abstrakcji w celu wstrzyknięcia zależności na podstawie interfejsu
    {
        int Create(int restaurantId, CreateDishDto dto);
        DishDto GetById(int restaurantId, int dishId);
        List<DishDto> GetAll(int restaurantId);
        void RemoveAll(int restaurantId);
    }
    public class DishService: IDishService 
    {
        private readonly RestaurantDbContext _context;
        private readonly IMapper _mapper;
        public DishService(RestaurantDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public int Create(int restaurantId, CreateDishDto dto)
        {
            var restaurant = GetRestaurantById(restaurantId);
            var dishEntity = _mapper.Map<Dish>(dto); // jeśli istnieje. to mapujemy dto do encji Dish

            dishEntity.RestaurantId = restaurantId;

            _context.Dishes.Add(dishEntity); // dodanie do kontekstu bazy danych 
            _context.SaveChanges();

            return dishEntity.Id;
        }

        public DishDto GetById(int restaurantId, int dishId)
        {
            var restaurant = GetRestaurantById(restaurantId);
            var dish = _context.Dishes.FirstOrDefault(d => d.Id == dishId);
            if (dish is null || dish.RestaurantId != restaurantId) // sprawdzenie czy danie w ogóle istnieje lub czy nie istnieje w kontekście danej restauracji
            {
                throw new NotFoundException("Danie nie zostało znalezione");
            }

            var dishDto = _mapper.Map<DishDto>(dish); // jeśli danie istnieje, to mapujemy je do modelu dto

            return dishDto;
                
        }

        public List<DishDto> GetAll(int restaurantId)
        {

            var restaurant = GetRestaurantById(restaurantId);
            var dishDtos = _mapper.Map<List<DishDto>>(restaurant.Dishes); // mapowanie dań z restauracji do listy dishDto

            return dishDtos;

        }

        public void RemoveAll(int restaurantId)
        {
            var restaurant = GetRestaurantById(restaurantId);

            _context.RemoveRange(restaurant.Dishes);
            _context.SaveChanges();

        }

        private Restaurant GetRestaurantById(int restaurantId)
        {
            var restaurant = _context
               .Restaurants
               .Include(r => r.Dishes)
               .FirstOrDefault(r => r.Id == restaurantId);

            if (restaurant is null)
                throw new NotFoundException("Restauracja nie została znaleziona");

            return restaurant;
        }
    }
}
