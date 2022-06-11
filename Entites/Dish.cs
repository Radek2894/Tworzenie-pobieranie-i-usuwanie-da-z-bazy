using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Radzio.Entites
{
    public class Dish
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public decimal Price { get; set; }

        public int RestaurantId { get; set; } // dzięki przypisaniu id restauracji do dania możliwe jest utworzenie referencji do osobnej tabeli w bazie danych

        public virtual Restaurant Restaurant { get; set; }


    }
}
