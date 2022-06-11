using Radzio.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Radzio
{
    public class RestaurantSeeder
    {
        private readonly RestaurantDbContext _dbContext;
        public RestaurantSeeder(RestaurantDbContext dbContext) //konstruktor 
        {
            _dbContext = dbContext;
        }
        public void Seed() // dodanie początkowych wartości do tabeli z restauracjami
        {
            
            if(_dbContext.Database.CanConnect())
            {

                if(!_dbContext.Restaurants.Any())
                {
                    var restaurants = GetRestaurants();
                    _dbContext.Restaurants.AddRange(restaurants);
                    _dbContext.SaveChanges();

                }
            


            }
        }

        private IEnumerable<Restaurant> GetRestaurants() //zwaraca restauracje, które zawsze będą istnieć w tabeli restaurant
        {
            var restaurants = new List<Restaurant>()
            {
                new Restaurant()
                {
                    Name = "KFC",
                    Category = "Fast Food",
                    Description = "KFC (short for Kentucky Fried Chicken) is an American fast food restaurant",
                    ContactEmail = "contact@kfc.com",
                    HasDelivery = true,
                    Dishes = new List<Dish>()
                    {
                        new Dish()
                        {
                            Name = "Nashville Hot Chicken",
                            Price = 5.30M,
                        },

                          new Dish()
                        {
                            Name = "Chicken Nuggets",
                            Price = 10.30M,
                        },
                    },
                        Address = new Address()
                        {
                            City = "Kraków",
                            Street = "Długa 5",
                            PostalCode = "30-001"



                        }

                }

            };

            return restaurants;

        }
    }
}

