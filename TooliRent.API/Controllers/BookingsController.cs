using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TooliRent.Application.Dto.Booking;
using TooliRent.Application.Services.Interfaces;

namespace TooliRent.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _service;
        public BookingsController(IBookingService service)
        {
            _service = service;
        }
       
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(BookingResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<BookingResponseDto>>> GetAllBookings()
        {
            var bookings = await _service.GetAllBookingsAsync();
            return Ok(bookings);
        }
        
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(BookingResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BookingResponseDto>> GetBookingById(int id)
        {
            var booking = await _service.GetBookingByIdAsync(id);
            if (booking is null)
            {
                return NotFound(new { Message = $"Booking med Idt {id} finns inte!" });
            }
            return Ok(booking);
        }
        
        [HttpGet("tool/{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(BookingResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<BookingResponseDto>>> GetBookingsWithToolId(int id)
        {
            var bookings = await _service.GetBookingsWithToolIdAsync(id);
            return Ok(bookings);
        }
        
        [HttpGet("user/")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(BookingResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<BookingResponseDto>>> GetBookingsByUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return NotFound("User id invalid");
            }
            var bookings = await _service.GetBookingsByUserIdAsync(userId);
            return Ok(bookings);
        }
        [HttpGet("Overall-stats")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(BookingStatsDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BookingStatsDto>> GetBookingStats()
        {
            var stats = await _service.GetBookingStatsAsync();
            return Ok(stats);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Member")]
        [ProducesResponseType(typeof(BookingResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BookingResponseDto>> CreateBooking([FromBody] BookingReqDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null)
            {
                return Unauthorized(new { Message = "Du måste vara inloggad för att boka." });
            }
            var booking = await _service.CreateBookingAsync(dto, userId);
            return CreatedAtAction(nameof(GetBookingById), new { id = booking.Id }, booking);

        }
        
        [HttpPut("{id}")]
        //[Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(BookingResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BookingResponseDto>> UpdateBooking(int id, [FromBody] UpdateBookingDto dto)
        {
            try
            {
                var updatedBooking = await _service.UpdateBookingAsync(id, dto);
                if (updatedBooking == null)
                {
                    return NotFound(new { Message = $"Booking med ID:t {id} finns inte!" });
                }
                return Ok(updatedBooking);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }
        [HttpPatch("{id}/Pickup-status")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(BookingResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> MarkAsPickedUp(int id)
        {
            try
            {
                var booking = await _service.MarkAsPickedUpAsync(id);
                return Ok(booking);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        [HttpPatch("{id}/return")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(BookingResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> MarkAsReturned(int id)
        {
            try
            {
                var booking = await _service.MarkAsReturnedAsync(id);
                return Ok(booking);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }


        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteBooking(int id)
        {
            var deleted = await _service.DeleteBookingAsync(id);
            if (!deleted)
            {
                return NotFound(new { Message = $"Booking med ID:t {id} finns inte!" });
            }
            return NoContent();
        }

    }
}
