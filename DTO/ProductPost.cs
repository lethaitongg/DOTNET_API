using DEMO_BUOI07_API.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DEMO_BUOI07_API.DTO
{
    public class ProductPost
    {
        public required string Name { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public required IEnumerable<IFormFile> Images { get; set; }
    }
}
