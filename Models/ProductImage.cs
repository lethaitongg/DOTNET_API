using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DEMO_BUOI07_API.Models
{
    public class ProductImage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]   
        public int Id { get; set; }

        [Required]
        public required string Name { get; set; }

        [Required]
        public int ProductId { get; set; }

        public Product Product { get; set; } = null!;
    }
}
