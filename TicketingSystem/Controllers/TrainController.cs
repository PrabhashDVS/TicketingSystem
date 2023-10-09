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

        [HttpGet("GetTrain/{id}")]
        public IActionResult GetTrainById(string id)
        {
            try
            {

                var res = _trainService.GetTrainById(id);

                if (res == null)
                {
                    return NotFound("Train not found");
                }

                return Ok(res);

            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpPut("UpdateTrain/{id}")]
        public IActionResult UpdateTrain(string id, [FromBody] Train updatedTrain)
        {
            try
            {

                var existingTrain = _trainService.GetTrainById(id);
                if (existingTrain == null)
                {
                    return NotFound("Train not found");
                }

                _trainService.UpdateTrain(id, updatedTrain);
                return Ok(updatedTrain);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }

        }


        [HttpDelete("DeleteTrain/{id}")]
        public IActionResult DeleteTrain(string id)
        {
            try
            {

                var Train = _trainService.GetTrainById(id);
                if (Train == null)
                {
                    return NotFound("Train not found");
                }

                _trainService.DeleteTrain(id);
                return Ok(Train);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }

        }

    }
}
