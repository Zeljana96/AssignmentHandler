using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tasks_Handler.Dtos.Assignment;
using Tasks_Handler.Models;
using Tasks_Handler.Services.AssignmentService;

namespace Tasks_Handler.Controllers
{
    [Authorize(Roles = "User,Admin")]
    [ApiController]
    [Route("[controller]")]
    public class AssignmentsController : ControllerBase
    {
        private readonly IAssignmentService _assignmentService;
        public AssignmentsController(IAssignmentService assignmentService)
        {
            _assignmentService = assignmentService;

        }

        [HttpGet]
        public async Task<IActionResult> GetAllAssignments()
        {
            return Ok(await _assignmentService.GetAllAssignments());
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAssignmentById(int id)
        {
            return Ok(await _assignmentService.GetAssignmentById(id));
        }

        [HttpPost]
        public async Task<IActionResult> AddAssignment(AddAssignmentDto newAssignment)
        {
            ServiceResponse<List<GetAssignmentDto>> response = await _assignmentService.AddAssignment(newAssignment);
            if(response.Success == false)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateAssignment(UpdateAssignmentDto updatedAssignment)
        {
            ServiceResponse<GetAssignmentDto> response = await _assignmentService.UpdateAssignment(updatedAssignment);
            
            if(response.Data == null){
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAssignment(int id)
        {
            ServiceResponse<List<GetAssignmentDto>> response = await _assignmentService.DeleteAssignment(id);
            ViewBag.Team = new SelectedList();
            if(response.Data == null){
                return NotFound(response);
            }
            return Ok(response);
        }
    }
}