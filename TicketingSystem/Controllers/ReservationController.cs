/*
   File: ReservationController.cs
   Description: This file contains the implementation of the ReservationController class, which handles reservation-related
   operations.
   Author: Piyumantha H. P. A. H.
   Creation Date: 2023/10/04
   Last Modified Date: 2023/10/10
*/

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

        /// <summary>
        /// Handles the HTTP POST request to add a new reservation.
        /// </summary>
        /// <param name="reservation">A Reservation object containing details of the reservation to be added.</param>
        /// <returns>Returns an HTTP response with the added reservation information if successful,
        /// or a bad request response with an error message if an exception occurs.</returns>
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

        /// <summary>
        /// Handles the HTTP GET request to retrieve a list of all reservations.
        /// </summary>
        /// <returns>Returns an HTTP response with a list of all reservations if successful, 
        /// or a bad request response with an error message if an exception occurs.</returns>
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

        /// <summary>
        /// Handles the HTTP GET request to retrieve a reservation by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the reservation to retrieve.</param>
        /// <returns>Returns an HTTP response with the reservation information if found, 
        /// or a bad request response with an error message if an exception occurs or the reservation is not found.</returns>
        //[Authorize]
        [HttpGet("GetReservation/{id}")]
        public IActionResult GetReservationById(string id)
        {
            try
            {
                var res = _reservationService.GetReservationById(id);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
        /// <summary>
        /// Handles the HTTP GET request to retrieve reservations associated with a specific user by their user identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the user for whom reservations are to be retrieved.</param>
        /// <returns>Returns an HTTP response with a list of reservations associated with the user if successful,
        /// or a bad request response with an error message if an exception occurs.</returns>
        //[Authorize]
        [HttpGet("GetReservationByUser/{id}")]
        public IActionResult GetReservationsByUserId(string id)
        {
            try
            {
                var res = _reservationService.GetReservationsByUserId(id);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        //[Authorize]
        /// <summary>
        /// Handles the HTTP PUT request to update a reservation by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the reservation to update.</param>
        /// <param name="updatedReservation">A Reservation object containing the updated reservation details.</param>
        /// <returns>Returns an HTTP response with the updated reservation information if successful,
        /// or a bad request response with an error message if an exception occurs or the reservation is not found.</returns>
        [HttpPut("UpdateReservation/{id}")]
        public IActionResult UpdateReservation(string id, [FromBody] Reservation updatedReservation)
        {
            try
            {
                var res = _reservationService.UpdateReservation(id, updatedReservation);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }

        }

        //[Authorize]
        /// <summary>
        /// Handles the HTTP DELETE request to delete a reservation by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the reservation to be deleted.</param>
        /// <returns>Returns an HTTP response with a success message if the deletion is successful,
        /// or a bad request response with an error message if an exception occurs or the reservation is not found.</returns>
        [HttpDelete("DeleteReservation/{id}")]
        public IActionResult DeleteReservation(string id)
        {
            try
            {
                var res = _reservationService.DeleteReservation(id);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }

        }

    }
}
