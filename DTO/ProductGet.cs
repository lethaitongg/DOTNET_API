namespace DEMO_BUOI07_API.DTO
{
    public class ProductGet
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public required IEnumerable<string> ImageNames { get; set; }
    }
}
