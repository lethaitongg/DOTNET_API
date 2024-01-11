using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DEMO_BUOI07_API.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Column(TypeName ="decimal(10,2)")]
        public decimal? Price { get; set; }

        [Required]
        public int? Quantity { get; set; }

        public ICollection<ProductImage> ProductImages { get; } = new List<ProductImage>();

    }
}
