using Application.Dto.Product;
using Application.Interfaces;
using Application.Services.ProductServices;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IQueryRepository _queryRepository;
        private readonly IProductService _productService;

        public ProductsController(IQueryRepository queryRepository, IProductService productService)
        {
            _queryRepository = queryRepository;
            _productService = productService;
        }

        [HttpGet(":{id}")]
        public async Task<ActionResult<Product>> Get(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] bool paginate = true)
        {
            var products = await _productService.GetAllProductsPaginatedAsync(page, pageSize);

            if (!paginate)
            {
                products = await _productService.GetAllProductsAsync();
                return Ok(products);
            }

            return Ok(products);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Insert(ProductDto productDto)
        {
            if (productDto == null)
            {
                return BadRequest("Invalid product details");
            }

            await _productService.InsertProductAsync(productDto);
            return Ok(productDto);
        }

        [HttpPut(":{id}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, ProductDto productDto)
        {
            if (id == null)
            {
                return NotFound("Product not found.");
            }

            var product = await _productService.UpdateProductAsync(id, productDto);

            return Ok(productDto);
        }

        [HttpDelete(":{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return NotFound("Product not found");
            }

            var product = _productService.RemoveProductAsync(id);

            return Ok($"Deleted product");
        }
    }
}
