using Domain.Entities;

namespace Application.Interfaces
{
    public interface IQueryRepository
    {
        Task<Product> GetProductByIdAsync(int id);
        Task<IEnumerable<Product>> GetAllProductsPaginatedAsync(int page, int pageSize);
        Task<IEnumerable<Product>> GetAllProductsAsync();
    }
}
