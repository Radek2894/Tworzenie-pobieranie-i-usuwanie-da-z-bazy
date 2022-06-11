namespace Radzio.Models
{
    public class DishDto // utworzono bez atrybutów walidacji wyłącznie z właściwościami niezbędnymi do opisania dania
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public decimal Price { get; set; }

    }
}
