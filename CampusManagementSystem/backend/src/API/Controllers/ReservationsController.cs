using CampusManagementSystem.Application.DTOs;
using CampusManagementSystem.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampusManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReservationsController : ControllerBase
{
    private readonly IReservationService _reservationService;

    public ReservationsController(IReservationService reservationService)
    {
        _reservationService = reservationService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ReservationDto>> GetReservationById(Guid id)
    {
        var reservation = await _reservationService.GetReservationByIdAsync(id);
        if (reservation == null)
            return NotFound();
        return Ok(reservation);
    }

    [HttpGet("staff/{staffId}")]
    [Authorize(Roles = "Staff,Admin")]
    public async Task<ActionResult<IEnumerable<ReservationDto>>> GetStaffReservations(Guid staffId)
    {
        var reservations = await _reservationService.GetStaffReservationsAsync(staffId);
        return Ok(reservations);
    }

    [HttpGet("classroom/{classroomId}")]
    public async Task<ActionResult<IEnumerable<ReservationDto>>> GetClassroomReservations(Guid classroomId)
    {
        var reservations = await _reservationService.GetClassroomReservationsAsync(classroomId);
        return Ok(reservations);
    }

    [HttpPost]
    [Authorize(Roles = "Staff,Admin")]
    public async Task<ActionResult<ReservationDto>> CreateReservation(CreateReservationDto createReservationDto)
    {
        try
        {
            var reservation = await _reservationService.CreateReservationAsync(createReservationDto);
            return CreatedAtAction(nameof(GetReservationById), new { id = reservation.ReservationId }, reservation);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Staff,Admin")]
    public async Task<ActionResult<ReservationDto>> UpdateReservation(Guid id, UpdateReservationDto updateReservationDto)
    {
        try
        {
            var reservation = await _reservationService.UpdateReservationAsync(id, updateReservationDto);
            return Ok(reservation);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Staff,Admin")]
    public async Task<IActionResult> DeleteReservation(Guid id)
    {
        var result = await _reservationService.DeleteReservationAsync(id);
        if (!result)
            return NotFound();
        return NoContent();
    }
}
