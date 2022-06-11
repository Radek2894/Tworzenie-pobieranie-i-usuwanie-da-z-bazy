﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using Radzio.Entites;
using Radzio.Models;
using System.Collections.Generic;
using System.Linq;

namespace Radzio.Services
{
    public interface IRestaurantService
    {
        RestaurantDto GetById(int id);
        IEnumerable<RestaurantDto> GetAll();
        int Create(CreateRestaurantDto dto);
        void Delete(int id);
        void Update(int id, UpdateRestaurantDto dto);
    }
    public class RestaurantService : IRestaurantService
    {
        private readonly RestaurantDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<RestaurantService> _logger;
        public RestaurantService(RestaurantDbContext dbContext, IMapper mapper, ILogger<RestaurantService> logger)
        { 
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        } 

        public void Update(int id, UpdateRestaurantDto dto)
        {
            _logger.LogWarning($"Restauracja o id: {id} USUŃ");

            var restaurant = _dbContext
              .Restaurants
              .FirstOrDefault(r => r.Id == id);

            if (restaurant == null)
                throw new NotFoundException("Restauracja nie została znaleziona"); 
        
            restaurant.Name = dto.Name;
            restaurant.Description = dto.Description;
            restaurant.HasDelivery = dto.HasDelivery;

            _dbContext.SaveChanges();

          
        }

        public void Delete(int id)
        {
            var restaurant = _dbContext
              .Restaurants
              .FirstOrDefault(r => r.Id == id);

            if (restaurant == null)
                throw new NotFoundException("Restauracja nie została znaleziona");


            _dbContext.Restaurants.Remove(restaurant);
            _dbContext.SaveChanges();

           

        }

        public RestaurantDto GetById(int id)
        {
            var restaurant = _dbContext
              .Restaurants
              .Include(r => r.Address)
              .Include(r => r.Dishes)
              .FirstOrDefault(r => r.Id == id);

            if (restaurant == null)
               throw new NotFoundException("Restauracja nie została znaleziona");


            var result = _mapper.Map<RestaurantDto>(restaurant);
            return result;
        }
        public IEnumerable<RestaurantDto> GetAll()
        {
            var restaurants = _dbContext
               .Restaurants
               .Include(r => r.Address)
               .Include(r => r.Dishes)
               .ToList();

            var RestaurantsDtos = _mapper.Map<List<RestaurantDto>>(restaurants);

            return RestaurantsDtos;

        }
        public int Create(CreateRestaurantDto dto)
        {
            var restaurant = _mapper.Map<Restaurant>(dto);
            _dbContext.Restaurants.Add(restaurant);
            _dbContext.SaveChanges();

            return restaurant.Id;
        }
     }
}
