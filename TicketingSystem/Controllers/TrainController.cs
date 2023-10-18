/*
   File: TrainController.cs
   Description: This file contains the implementation of the TrainController class, which manages train-related operations.
   Author: Weerasiri R. T. K. , Weerasinghe T. K.(ActiveTrain Implementation)
   Creation Date: 2023/10/04
   Last Modified Date: 2023/10/11
*/


using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using TicketingSystem.Model;
using TicketingSystem.Repository;

namespace TicketingSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrainController : Controller
    {

        private readonly TrainService _trainService;

        public TrainController(TrainService trainService)
        {
            _trainService = trainService;
        }

       
        /// <summary>
        /// Handles the HTTP POST request to add a new train.
        /// </summary>
        /// <param name="train">A Train object containing details of the train to be added.</param>
        /// <returns>Returns an HTTP response with the added train information if successful,
        /// or a bad request response with an error message if an exception occurs.</returns>
        [Authorize(Policy = "backOfficersOnly")]
        [HttpPost("AddTrain")]
        public IActionResult AddTrain(Train train)
        {
            try
            {
                var res = _trainService.InsertTrain(train);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Handles the HTTP GET request to activate a train by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the train to be activated.</param>
        /// <returns>Returns an HTTP response with the activation status if successful, 
        /// or a bad request response with an error message if an exception occurs or the train is not found.</returns>
        [Authorize(Policy = "backOfficersOnly")]
        [HttpGet("ActiveTrain/{id}")]
        public IActionResult ActiveTrain(string id)
        {
            try
            {
                var res = _trainService.ActiveTrain(id);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Handles the HTTP GET request to retrieve a list of all trains.
        /// </summary>
        /// <returns>Returns an HTTP response with a list of all trains if successful,
        /// or a bad request response with an error message if an exception occurs.</returns>
        [Authorize]
        [HttpGet("GetAllTrains")]
        public IActionResult GetAllTrains()
        {
            try
            {
                var TrainList = _trainService.GetAllTrains();
                return Ok(TrainList);
            }
            catch (Exception ex)
            {

                return BadRequest($"Error: {ex.Message}");
            }

        }

        /// <summary>
        /// Handles the HTTP GET request to retrieve a train by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the train to retrieve.</param>
        /// <returns>Returns an HTTP response with the train information if found, 
        /// or a bad request response with an error message if an exception occurs or the train is not found.</returns>
        [Authorize(Policy = "backOfficersOnly")]
        [HttpGet("GetTrain/{id}")]
        public IActionResult GetTrainById(string id)
        {
            try
            {
                var res = _trainService.GetTrainById(id);
                return Ok(res);

            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Handles the HTTP PUT request to update a train by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the train to update.</param>
        /// <param name="updatedTrain">A Train object containing the updated train details.</param>
        /// <returns>Returns an HTTP response with the updated train information if successful,
        /// or a bad request response with an error message if an exception occurs or the train is not found.</returns>
        [Authorize(Policy = "backOfficersOnly")]
        [HttpPut("UpdateTrain/{id}")]
        public IActionResult UpdateTrain(string id, [FromBody] Train updatedTrain)
        {
            try
            {            
                _trainService.UpdateTrain(id, updatedTrain);
                return Ok(updatedTrain);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }

        }

        /// <summary>
        /// Handles the HTTP DELETE request to delete a train by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the train to be deleted.</param>
        /// <returns>Returns an HTTP response with a success message if the deletion is successful, 
        /// or a bad request response with an error message if an exception occurs or the train is not found.</returns>
        [Authorize(Policy = "backOfficersOnly")]
        [HttpDelete("DeleteTrain/{id}")]
        public IActionResult DeleteTrain(string id)
        {
            try
            {              
                var res = _trainService.DeleteTrain(id);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }

        }

    }
}
