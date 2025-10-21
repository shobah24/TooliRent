using AutoMapper;
using TooliRent.Application.Dto.Booking;
using TooliRent.Application.Services.Interfaces;
using TooliRent.Domain.Models;
using TooliRent.Domain.RepoInterfaces;
using static TooliRent.Domain.Enums.Status;

namespace TooliRent.Application.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _repo;
        private readonly IMapper _mapper;

        public BookingService(IBookingRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BookingResponseDto>> GetAllBookingsAsync()
        {
            var bookings = await _repo.GetAllBookingsAsync();
            return _mapper.Map<IEnumerable<BookingResponseDto>>(bookings);

        }
        public async Task<BookingResponseDto?> GetBookingByIdAsync(int id)
        {
            var booking = await _repo.GetBookingByIdAsync(id);
            return booking == null ? null : _mapper.Map<BookingResponseDto?>(booking);
        }
        public async Task<IEnumerable<BookingResponseDto>> GetBookingsWithToolIdAsync(int id)
        {
            var bookings = await _repo.GetAllBookingsWithToolsAsync(id);
            return _mapper.Map<IEnumerable<BookingResponseDto>>(bookings);

        }
        public async Task<IEnumerable<BookingResponseDto>> GetBookingsByUserIdAsync(string userId)
        {
            var bookings = await _repo.GetBookingsByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<BookingResponseDto>>(bookings);

        }
        public async Task<BookingStatsDto> GetBookingStatsAsync()
        {
            var bookings = await _repo.GetAllBookingsAsync();

            return new BookingStatsDto
            {
                Total = bookings.Count(),
                Pending = bookings.Count(b => b.Status == BookingStatus.Pending),
                Active = bookings.Count(b => b.Status == BookingStatus.Active),
                Returned = bookings.Count(b => b.Status == BookingStatus.Returned),
                Late = bookings.Count(b => b.Status == BookingStatus.Late)
            };
        }
        public async Task<BookingResponseDto> CreateBookingAsync(BookingReqDto dto, string userId)
        {
            var booking = new Booking
            {
                UserId = userId,
                //ToolId = tooldId,
                LoanDate = dto.LoanDate,
                ReturnDate = dto.ReturnDate,
                Status = BookingStatus.Pending,
                PickupDate = DateTime.MinValue,
                ReturnedDate = DateTime.MinValue,
                BookingTools = dto.ToolId.Select(id => new BookingTool
                {
                    ToolId = id,
                }).ToList()
            };

            await _repo.AddBookingAsync(booking);
            var createdBooking = await _repo.GetBookingByIdAsync(booking.Id);
            return _mapper.Map<BookingResponseDto>(createdBooking);
        }
        public async Task<BookingResponseDto?> UpdateBookingAsync(int id, UpdateBookingDto dto)
        {
            var booking = await _repo.GetBookingByIdAsync(id);
            if (booking is null)
            {
                throw new KeyNotFoundException($"Booking med ID:t {id} finns inte!");
            }

            if (dto.ToolId != null && dto.ToolId.Any())
            {
                var existingToolIds = booking.BookingTools.Select(bt => bt.ToolId).ToList();
                var toAdd = dto.ToolId.Except(existingToolIds);
                foreach (var toolId in toAdd)
                {
                    booking.BookingTools.Add(new BookingTool { ToolId = toolId, Booking = booking });
                }

                var toRemove = booking.BookingTools
                    .Where(bt => !dto.ToolId.Contains(bt.ToolId))
                    .ToList();
                foreach (var bt in toRemove)
                {
                    booking.BookingTools.Remove(bt);
                }
            }
            //if (dto.LoanDate.HasValue)
            //{
            //    booking.LoanDate = dto.LoanDate.Value;
            //}
            //if (dto.ReturnDate.HasValue)
            //{
            //    booking.ReturnDate = dto.ReturnDate.Value;
            //}
            //if (dto.Status.HasValue)
            //{
            //    booking.Status = dto.Status.Value;
            //}
            
            _mapper.Map(dto, booking);

            await _repo.UpdateBookingAsync(booking);
            return _mapper.Map<BookingResponseDto>(booking);

        }
        public async Task<BookingResponseDto> MarkAsPickedUpAsync(int id)
        {
            var booking = await _repo.GetBookingByIdAsync(id);
            if (booking is null)
            {
                throw new KeyNotFoundException($"Booking med Idt {id} finns inte!");
            }

            if (DateTime.Now > booking.ReturnDate)
            {
                throw new InvalidOperationException("Din bokning är hämtad försent och kan därför inte lämnas ut.");
            }
            booking.Status = BookingStatus.Active;
            booking.PickupDate = DateTime.Now;

            //var loanDays = (booking.ReturnDate - booking.LoanDate).Days;
            //booking.ReturnDate = booking.PickupDate.AddDays(loanDays);


            await _repo.UpdateBookingAsync(booking);
            return _mapper.Map<BookingResponseDto>(booking);
        }
        public async Task<BookingResponseDto> MarkAsReturnedAsync(int id)
        {
            var booking = await _repo.GetBookingByIdAsync(id);
            if (booking is null)
            {
                throw new KeyNotFoundException($"Booking med Idt {id} finns inte!");
            }

            booking.ReturnedDate = DateTime.Now;
            booking.Status = booking.ReturnedDate > booking.ReturnDate
                ? BookingStatus.Late
                : BookingStatus.Returned;

            await _repo.UpdateBookingAsync(booking);
            return _mapper.Map<BookingResponseDto>(booking);
        }

        public async Task<bool> DeleteBookingAsync(int id)
        {
            var booking = await _repo.GetBookingByIdAsync(id);
            if (booking is null)
            {
                return false;
            }
            await _repo.DeleteBookingAsync(booking);
            return true;
        }
    }
}
