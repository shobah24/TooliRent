using Microsoft.EntityFrameworkCore;
using TooliRent.Domain.Models;
using TooliRent.Domain.RepoInterfaces;
using TooliRent.Infrastructure.Data;

namespace TooliRent.Infrastructure.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly TooliRentDbContext _context;

        public BookingRepository(TooliRentDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Booking>> GetAllBookingsAsync()
        {
            return await _context.Bookings
                //.Include(b => b.Tool)
                //.Include(b => b.User)
                .Include(b => b.BookingTools)
                .ThenInclude(bt => bt.Tool)
                .ToListAsync();

        }
        public async Task<Booking?> GetBookingByIdAsync(int id)
        {
            return await _context.Bookings
                .Include(b => b.BookingTools)
                .ThenInclude(bt => bt.Tool)
                //.Include(b => b.User)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<IEnumerable<Booking>> GetBookingsByUserIdAsync(string userId)
        {
            return await _context.Bookings
                .Include(b => b.BookingTools)
                .ThenInclude(bt => bt.Tool)
                //.Include(b => b.User)
                .Where(b => b.UserId == userId)
                .ToListAsync();
        }
        public async Task<IEnumerable<Booking>> GetAllBookingsWithToolsAsync(int id)
        {
            return await _context.Bookings
                .Include(b => b.BookingTools)
                .ThenInclude(bt => bt.Tool)
                //.Include(b => b.User)
                .Where(b => b.Id == id)
                .ToListAsync();
        }
        public async Task AddBookingAsync(Booking booking)
        {
            await _context.Bookings.AddAsync(booking);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateBookingAsync(Booking booking)
        {
            _context.Bookings.Update(booking);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteBookingAsync(Booking booking)
        {
            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();
        }
    }
}
