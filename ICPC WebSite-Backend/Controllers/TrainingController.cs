using ICPC_WebSite_Backend.Data.Models.DTO;
using ICPC_WebSite_Backend.Repository;
using ICPC_WebSite_Backend.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ICPC_WebSite_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainingController : ControllerBase
    {
        private readonly ITrainingRepository _trainingRepository;

        public TrainingController(ITrainingRepository trainingRepository) {
            _trainingRepository = trainingRepository;
        }
        [HttpPost("addtraining")]
        public async Task<IActionResult> addtraining(TrainingDTO trainingDTO) {
            var validate = Validate.IsValidTraining(trainingDTO);
            if (!validate.Succeeded)
                return BadRequest(validate);
            var result = await _trainingRepository.AddTraining(trainingDTO);
            if (!result.Succeeded) {
                return Unauthorized(result.Errors);
            }
            return Ok(result);
        }
        [HttpPut("updatetraining")]
        public async Task<IActionResult> updatetraining(int trainingId, TrainingDTO trainingDTO) {
            var validate = Validate.IsValidTraining(trainingDTO);
            if (!validate.Succeeded)
                return BadRequest(validate);
            var result = await _trainingRepository.UpdateTraining(trainingId, trainingDTO);
            if (!result.Succeeded) {
                return Unauthorized(result.Errors);
            }
            return Ok(result);
        }
        [HttpGet("alltrainings")]
        public async Task<IActionResult> getAlltrainings() {
            var result = await _trainingRepository.GetAllTrainings();
            if (!result.Succeeded) {
                return Unauthorized(result.Errors);
            }
            return Ok(result);
        }
        [HttpGet("gettraining")]
        public async Task<IActionResult> getTraining(int trainingId) {
            var result = await _trainingRepository.GetTraining(trainingId);
            if (!result.Succeeded) {
                return Unauthorized(result.Errors);
            }
            return Ok(result);
        }
        [HttpDelete("deletetraining")]
        public async Task<IActionResult> deletetraining(int trainingId) {
            var result = await _trainingRepository.DeleteTraining(trainingId);
            if (!result.Succeeded) {
                return NotFound(result.Errors);
            }
            return Ok(result);
        }
    }
}
