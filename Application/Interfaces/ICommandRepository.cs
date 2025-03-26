using Domain.Entities;

namespace Application.Interfaces
{
    public interface ICommandRepository
    {
        Task AddProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(int id);
    }
}
