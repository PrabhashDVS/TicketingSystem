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

        //[Authorize]
        [HttpPost("AddTrain")]
        public IActionResult AddTrain(Train Train)
        {
            try
            {
                var res = _trainService.InsertTrain(Train);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
        //[Authorize]
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

        //[Authorize]
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

        //[Authorize]
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

        //[Authorize]
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
