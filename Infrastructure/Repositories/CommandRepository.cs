using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CommandRepository : ICommandRepository
    {
        private readonly AppDbContext _context;

        public CommandRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddProductAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProductAsync(Product product)
        {
            var getProduct = _context.Products.FirstOrDefault(p => p.Id == product.Id);

            if (getProduct != null)
            {
                _context.Entry(getProduct).CurrentValues.SetValues(product);
            }
            else
            {
                _context.Products.Attach(product);
                _context.Entry(product).State = EntityState.Modified;
            }
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(int id)
        {
            var getProduct = _context.Products.FirstOrDefault(p => p.Id == id);
            _context.Products.Remove(getProduct);
            await _context.SaveChangesAsync();
        }
    }
}
