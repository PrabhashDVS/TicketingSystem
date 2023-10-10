using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using TicketingSystem.Model;
using TicketingSystem.Repository;

namespace TicketingSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationController : Controller
    {

        private readonly ReservationService _reservationService;

        public ReservationController(ReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        //[Authorize]
        [HttpPost("AddReservation")]
        public IActionResult AddReservation(Reservation reservation)
        {
            try
            {
                var res = _reservationService.InsertReservation(reservation);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        //[Authorize]
        [HttpGet("GetAllReservations")]
        public IActionResult GetAllReservations()
        {
            try
            {
                var ReservationList = _reservationService.GetAllReservations();
                return Ok(ReservationList);
            }
            catch (Exception ex)
            {

                return BadRequest($"Error: {ex.Message}");
            }

        }

        //[Authorize]
        [HttpGet("GetReservation/{id}")]
        public IActionResult GetReservationById(string id)
        {
            try
            {

                var res = _reservationService.GetReservationById(id);

                if (res == null)
                {
                    return NotFound("Reservation not found");
                }

                return Ok(res);

            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
        //[Authorize]
        [HttpGet("GetReservationByUser/{id}")]
        public IActionResult GetReservationsByUserId(string id)
        {
            try
            {

                var res = _reservationService.GetReservationsByUserId(id);

                if (res == null)
                {
                    return NotFound("Reservation not found");
                }

                return Ok(res);

            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        //[Authorize]
        [HttpPut("UpdateReservation/{id}")]
        public IActionResult UpdateReservation(string id, [FromBody] Reservation updatedReservation)
        {
            try
            {

                var existingReservation = _reservationService.GetReservationById(id);
                if (existingReservation == null)
                {
                    return NotFound("Reservation not found");
                }

                _reservationService.UpdateReservation(id, updatedReservation);
                return Ok(updatedReservation);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }

        }

        //[Authorize]
        [HttpDelete("DeleteReservation/{id}")]
        public IActionResult DeleteReservation(string id)
        {
            try
            {

                var Reservation = _reservationService.GetReservationById(id);
                if (Reservation == null)
                {
                    return NotFound("Reservation not found");
                }

                _reservationService.DeleteReservation(id);
                return Ok(Reservation);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }

        }

    }
}
