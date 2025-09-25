using Microsoft.EntityFrameworkCore;
using TooliRent.Domain.Models;
using TooliRent.Domain.RepoInterfaces;
using TooliRent.Infrastructure.Data;

namespace TooliRent.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly TooliRentDbContext _context;

        public CategoryRepository(TooliRentDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category?> GetCategoryByIdAsync(int id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task<Category?> GetCategoryWithToolsAsync(int id)
        {
            return await _context.Categories
                .Include(c => c.Tools)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
        public async Task AddCategoryAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync(); 
        }
        public async Task UpdateCategoryAsync(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteCategoryAsync(Category category)
        {
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }
    }
}
