using AutoMapper;
using DEMO_BUOI07_API.DTO;
using DEMO_BUOI07_API.Helpers;
using DEMO_BUOI07_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DEMO_BUOI07_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductImageController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly DatabaseContext databaseContext;

        public ProductImageController(IMapper mapper, DatabaseContext databaseContext)
        {
            this.mapper = mapper;
            this.databaseContext = databaseContext;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductImages(int id)
        {
            ProductImage productImage = await databaseContext.ProductImages.FindAsync(id);

            if (productImage == null)
            {
                return NotFound();
            }

            return Ok(productImage);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProductImage([FromForm] ProductImage productImage)
        {
            try
            {
                await databaseContext.ProductImages.AddAsync(productImage);
                await databaseContext.SaveChangesAsync();
                return CreatedAtAction("GetProductImages", new { id = productImage.Id }, productImage);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
