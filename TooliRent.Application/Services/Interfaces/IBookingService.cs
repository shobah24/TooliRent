using TooliRent.Application.Dto.Booking;
using TooliRent.Domain.Models;

namespace TooliRent.Application.Services.Interfaces
{
    public interface IBookingService
    {
        Task<IEnumerable<BookingResponseDto>> GetAllBookingsAsync();
        Task<BookingResponseDto?> GetBookingByIdAsync(int id);
        Task<IEnumerable<BookingResponseDto>> GetBookingsByUserIdAsync(string userId);
        Task<IEnumerable<BookingResponseDto>> GetBookingsWithToolIdAsync(int id);
        Task<BookingStatsDto> GetBookingStatsAsync();
        Task<BookingResponseDto> CreateBookingAsync(BookingReqDto dto, string userId);
        Task<BookingResponseDto?> UpdateBookingAsync(int id, UpdateBookingDto dto);
        Task<BookingResponseDto> MarkAsPickedUpAsync(int id);
        Task<BookingResponseDto> MarkAsReturnedAsync(int id);
        Task<bool> DeleteBookingAsync(int id);

    }
}
