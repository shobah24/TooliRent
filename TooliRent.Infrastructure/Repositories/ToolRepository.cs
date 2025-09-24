using Microsoft.EntityFrameworkCore;
using TooliRent.Domain.Enums;
using TooliRent.Domain.Models;
using TooliRent.Domain.RepoInterfaces;
using TooliRent.Infrastructure.Data;
using static TooliRent.Domain.Enums.Status;

namespace TooliRent.Infrastructure.Repositories
{
    public class ToolRepository : IToolRepository
    {
        private readonly TooliRentDbContext _context;

        public ToolRepository(TooliRentDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Tool>> GetAllToolAsync()
        {
            return await _context.Tools
            .Include(t => t.Category)
            .ToListAsync();

        }

        public async Task<IEnumerable<Tool>> GetAvailableToolAsync()
        {
            return await _context.Tools
            .Include(t => t.Category)
            .Where(t => t.Status == ToolStatus.Available)
            .ToListAsync();
        }

        public async Task<Tool?> GetToolByIdAsync(int id)
        {
            return await _context.Tools
                .Include(t => t.Category)
                .FirstOrDefaultAsync(t => t.Id == id);
            
        }

        public async Task<IEnumerable<Tool>> GetToolByCategoryAsync(int categoryId)
        {
            return await _context.Tools
                .Include(t => t.Category)
                .Where(t => t.CategoryId == categoryId)
                .ToListAsync();
            
        }
        public async Task AddToolAsync(Tool tool)
        {
            await _context.Tools.AddAsync(tool);
            await _context.SaveChangesAsync();
        }
        //kolla upp igen
        public async Task<IEnumerable<Tool>> FilterToolAsync(Status.ToolStatus status, int? categoryId)
        {
            var query = _context.Tools.AsQueryable();

            query = query.Where(t => t.Status == status);

            if (categoryId != null)
                query = query.Where(t => t.CategoryId == categoryId);

            return await query
                .Include(t => t.Category)
                .ToListAsync();

            //    return await _context.Tools
            //.Include(t => t.Category)
            //.Where(t => t.Status == status && t.CategoryId == categoryId)
            //.ToListAsync();
        }

        public async Task UpdateToolAsync(Tool tool)
        {
            _context.Tools.Update(tool);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteToolAsync(int id)
        {
            var tool = await _context.Tools.FirstOrDefaultAsync(a => a.Id == id);
            if (tool != null)
            {
                _context.Tools.Remove(tool);
                await _context.SaveChangesAsync();
            }
        }

    }
}
