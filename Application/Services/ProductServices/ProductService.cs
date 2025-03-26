using Application.Dto.Product;
using Application.Dto.Response.ProductResponse;
using Application.Interfaces;
using Domain.Entities;

namespace Application.Services.ProductServices
{
    public class ProductService : IProductService
    {
        private readonly IQueryRepository _queryRepository;
        private readonly ICommandRepository _commandRepository;

        public ProductService(IQueryRepository queryRepository, ICommandRepository commandRepository)
        {
            _queryRepository = queryRepository;
            _commandRepository = commandRepository;
        }

        public async Task<ProductResponse> RemoveProductAsync(int id)
        {
            if (id == null)
            {
                return new ProductResponse(false, $"Product {id} not found");
            }

            await _commandRepository.DeleteProductAsync(id);

            return new ProductResponse(true, $"Product {id} deleted");
        }

        public async Task<ProductResponse> InsertProductAsync(ProductDto productDto)
        {
            if (productDto == null)
            {
                return new ProductResponse(false, "Product details cannot be null");
            }

            var newProduct = new Product
            {
                ProductName = productDto.ProductName,
                Description = productDto.Description,
                Price = productDto.Price,
                Category = productDto.Category,
                DateCreated = DateTime.UtcNow
            };

            await _commandRepository.AddProductAsync(newProduct);

            return new ProductResponse(true, $"Product added successfully");
        }

        public async Task<ProductResponse> UpdateProductAsync(int id, ProductDto productDto)
        {
            var getProduct = await _queryRepository.GetProductByIdAsync(id);

            if (getProduct == null)
            {
                return new ProductResponse(false, "Product not found");
            }

            getProduct.ProductName = productDto.ProductName;
            getProduct.Description = productDto.Description;
            getProduct.Price = productDto.Price;
            getProduct.Category = productDto.Category;

            await _commandRepository.UpdateProductAsync(getProduct);

            return new ProductResponse(true, $"Product {getProduct.Id} updated");
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            var getProduct = await _queryRepository.GetProductByIdAsync(id);

            return getProduct;
        }

        public async Task<IEnumerable<Product>> GetAllProductsPaginatedAsync(int page, int pageSize)
        {
            var getAllProductsPaginate = await _queryRepository.GetAllProductsPaginatedAsync(page, pageSize);

            return getAllProductsPaginate;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            var getAllProducts = await _queryRepository.GetAllProductsAsync();

            return getAllProducts;
        }
    }
}
