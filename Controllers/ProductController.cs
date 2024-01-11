using AutoMapper;
using DEMO_BUOI07_API.DTO;
using DEMO_BUOI07_API.Helpers;
using DEMO_BUOI07_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DEMO_BUOI07_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly DatabaseContext databaseContext;

        public ProductController(IMapper mapper, DatabaseContext databaseContext)
        {
            this.mapper = mapper;
            this.databaseContext = databaseContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductGet>>> GetProducts()
        {
            IEnumerable<Product> products = await databaseContext.Products.Include(product => product.ProductImages).ToListAsync();
            var productGet = mapper.Map<IEnumerable<ProductGet>>(products);

            return Ok(productGet);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductGet>> GetProduct(int id)
        {
            Product? product = await databaseContext.Products
                .Include(product => product.ProductImages)
                .FirstOrDefaultAsync(product => product.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            var productGet = mapper.Map<ProductGet>(product);

            return Ok(productGet);
        }

        [HttpPost]
        public async Task<ActionResult<ProductGet>> CreateProduct([FromForm] ProductPost productPost)
        {
            try
            {
                Product product = mapper.Map<Product>(productPost);
                await databaseContext.Products.AddAsync(product);
                await databaseContext.SaveChangesAsync();

                foreach (IFormFile file in productPost.Images)
                {
                    string imageName = FileUpload.SaveFile("ProductImages", file);
                    ProductImage productImage = new() { Name = imageName, ProductId = product.Id };
                    await databaseContext.ProductImages.AddAsync(productImage);
                }
                await databaseContext.SaveChangesAsync();
                var productGet = mapper.Map<ProductGet>(product);
                
                return CreatedAtAction("GetProduct", new { id = product.Id }, productGet);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProductGet>> UpdateProduct(int id, [FromForm] ProductPost productPost)
        {
            try
            {
                Product? oldProduct = await databaseContext.Products
                    .AsNoTracking()
                    .Include(product => product.ProductImages)
                    .FirstOrDefaultAsync(product => product.Id == id);

                if (oldProduct == null)
                {
                    return NotFound();
                }

                var product = mapper.Map<Product>(productPost);
                product.Id = id;
                databaseContext.Products.Update(product);
                await databaseContext.SaveChangesAsync();

                if (productPost.Images.Count() > 0)
                {
                    databaseContext.Products.Entry(product).State = EntityState.Detached;
                    
                    IEnumerable<string> oldImages = mapper.Map<ProductGet>(oldProduct).ImageNames;
                    databaseContext.ProductImages.RemoveRange(oldProduct.ProductImages);
                    
                    foreach (string oldImage in oldImages) {
                        FileUpload.DeleteFile(oldImage);
                    }

                    List<ProductImage> productImages = new();
                    foreach (IFormFile file in productPost.Images)
                    {
                        string imageName = FileUpload.SaveFile("ProductImages", file);
                        productImages.Add(new() { Name = imageName, ProductId = id });
                    }
                    await databaseContext.ProductImages.AddRangeAsync(productImages);
                }
                await databaseContext.SaveChangesAsync();

                await databaseContext.Products.Entry(oldProduct).ReloadAsync();

                var productGet = mapper.Map<ProductGet>(oldProduct);
                return Ok(productGet);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ProductGet>> DeleteProduct(int id)
        {
            try
            {
                Product product = await databaseContext.Products
                    .Include(product => product.ProductImages)
                    .FirstAsync(product => product.Id == id);

                if (product == null)
                {
                    return NotFound();
                }

                var productGet = mapper.Map<ProductGet>(product);
                
                if (productGet.ImageNames.Count() > 0)
                {
                    databaseContext.ProductImages.RemoveRange(product.ProductImages);
                    await databaseContext.SaveChangesAsync();

                    foreach (string image in productGet.ImageNames)
                    {
                        FileUpload.DeleteFile(image);
                    }
                }
                
                databaseContext.Products.Remove(product);
                await databaseContext.SaveChangesAsync();

                return Ok(productGet);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        private async Task<bool> IsExisted(int id)
        {
            return await databaseContext.Products.AnyAsync(p => p.Id == id);
        }
    }
}
