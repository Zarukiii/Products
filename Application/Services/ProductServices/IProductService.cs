using Application.Dto.Product;
using Application.Dto.Response.ProductResponse;
using Domain.Entities;

namespace Application.Services.ProductServices
{
    public interface IProductService
    {
        Task<Product> GetProductByIdAsync(int id);
        Task<IEnumerable<Product>> GetAllProductsPaginatedAsync(int page, int pageSize);
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<ProductResponse> InsertProductAsync(ProductDto productDto);
        Task<ProductResponse> UpdateProductAsync(int id, ProductDto product);
        Task<ProductResponse> RemoveProductAsync(int id);
    }
}
