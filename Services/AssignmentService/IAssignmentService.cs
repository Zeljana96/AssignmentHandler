using System.Collections.Generic;
using System.Threading.Tasks;
using Tasks_Handler.Dtos.Assignment;
using Tasks_Handler.Models;

namespace Tasks_Handler.Services.AssignmentService
{
    public interface IAssignmentService
    {
         Task<ServiceResponse<List<GetAssignmentDto>>> GetAllAssignments();

         Task<ServiceResponse<GetAssignmentDto>> GetAssignmentById(int id);
         Task<ServiceResponse<List<GetAssignmentDto>>> AddAssignment(AddAssignmentDto newAssignment);
         Task<ServiceResponse<GetAssignmentDto>> UpdateAssignment(UpdateAssignmentDto newAssignment);
         Task<ServiceResponse<List<GetAssignmentDto>>> DeleteAssignment(int id);
    }
}