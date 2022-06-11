using Microsoft.AspNetCore.Mvc;
using Radzio.Models;
using Radzio.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Radzio.Controllers
{
    [Route("api/restaurant/{restaurantId}/dish")]  // ścieżka do dania zależy od id restauracji, ponieważ danie nie
                                                   // jest oddzielną encją i bez restauracji nie istnieje
    [ApiController]
    public class DishController: ControllerBase
    {
        private readonly IDishService _dishService;
        public DishController(IDishService dishService)
        {
            _dishService = dishService;
        }


        [HttpDelete]
        public ActionResult Delete([FromRoute] int restaurantId)
        {
            _dishService.RemoveAll(restaurantId);

            return NoContent();
        }

        [HttpPost]
        public ActionResult Post([FromRoute] int restaurantId, [FromBody] CreateDishDto dto) // mapowanie parametru restaurantId 
        {
           var newDishId =  _dishService.Create(restaurantId, dto);

            return Created($"api/restaurant/{restaurantId}/dish/{newDishId}", null); // zwrócenie statusu dla klienta
        }

        [HttpGet("{dishId}")]
        public ActionResult<DishDto> Get([FromRoute] int restaurantId, [FromRoute] int dishId) // mapowanie parametru dishId
        {
            var dish = _dishService.GetById(restaurantId, dishId); // paramater restaurantId w celu sprawdzenia czy dane danie jest zawarte w konkretnej 

            return Ok(dish);

        }

        [HttpGet]
        public ActionResult<List<DishDto>> Get([FromRoute] int restaurantId) // mapowanie parametru dishId
        {
            var result = _dishService.GetAll(restaurantId);

            return Ok(result);

        }

    }
}
