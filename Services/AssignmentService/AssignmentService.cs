using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Tasks_Handler.Data;
using Tasks_Handler.Dtos.Assignment;
using Tasks_Handler.Models;

namespace Tasks_Handler.Services.AssignmentService
{
    public class AssignmentService : IAssignmentService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AssignmentService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _mapper = mapper;

        }

        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

        private string GetUserRole() => _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);

        private List<string> GetAllowedStatuses()
        {
            List<string> allowedStatuses = new List<string>(){
               StatusClass.Created.ToString("G"), StatusClass.Assigned.ToString("G"), StatusClass.TakenOver.ToString("G"),
               StatusClass.InProgress.ToString("G"), StatusClass.OnHold.ToString("G"), StatusClass.SentForTesting.ToString("G"),
               StatusClass.Done.ToString("G")
            };
            return allowedStatuses;
        }
        private List<string> GetAllowedPriorities()
        {
            List<string> allowedPriorities = new List<string>(){
               PriorityClass.Low.ToString("G"), PriorityClass.Medium.ToString("G"), PriorityClass.High.ToString("G")
            };
            return allowedPriorities;
        }
        public async Task<ServiceResponse<List<GetAssignmentDto>>> AddAssignment(AddAssignmentDto newAssignment)
        {
            ServiceResponse<List<GetAssignmentDto>> serviceResponse = new ServiceResponse<List<GetAssignmentDto>>();
            List<string> allowedStatuses = GetAllowedStatuses();
            List<string> allowedPriorities = GetAllowedPriorities();
            if (!allowedStatuses.Contains(newAssignment.Status))
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Entered Status is not allowed.";

            }
            else if (!allowedPriorities.Contains(newAssignment.Priority))
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Entered Priority is not allowed.";
            }
            else if (DateTime.Compare(newAssignment.StartDate, newAssignment.EndDate) > 0)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Start date is later than End date.";
            }
            else
            {
                Assignment assignment = _mapper.Map<Assignment>(newAssignment);
                assignment.CreateDate = DateTime.UtcNow;
                assignment.EditDate = DateTime.UtcNow;
                assignment.UserIdEdit = GetUserId();
                assignment.User = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());

                await _context.Assignments.AddAsync(assignment);
                await _context.SaveChangesAsync();
                serviceResponse.Data = (_context.Assignments.Where(a => a.User.Id == GetUserId()).Select(a => _mapper.Map<GetAssignmentDto>(a))).ToList();
            }
            return (serviceResponse);
        }

        public async Task<ServiceResponse<List<GetAssignmentDto>>> GetAllAssignments()
        {
            ServiceResponse<List<GetAssignmentDto>> serviceResponse = new ServiceResponse<List<GetAssignmentDto>>();
            List<Assignment> dbAssignments =
            GetUserRole().Equals("Admin") ?
            await _context.Assignments.ToListAsync() :
            await _context.Assignments.Where(a => a.User.Id == GetUserId()).ToListAsync();
            serviceResponse.Data = (dbAssignments.Select(a => _mapper.Map<GetAssignmentDto>(a))).ToList();
            return (serviceResponse);
        }

        public async Task<ServiceResponse<GetAssignmentDto>> GetAssignmentById(int id)
        {
            ServiceResponse<GetAssignmentDto> serviceResponse = new ServiceResponse<GetAssignmentDto>();
            Assignment dbAssignment = await _context.Assignments
            .FirstOrDefaultAsync(a => a.Id == id && a.User.Id == GetUserId());
            serviceResponse.Data = _mapper.Map<GetAssignmentDto>(dbAssignment);
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetAssignmentDto>> UpdateAssignment(UpdateAssignmentDto updatedAssignment)
        {
            ServiceResponse<GetAssignmentDto> serviceResponse = new ServiceResponse<GetAssignmentDto>();
            try
            {
                Assignment assignment = await _context.Assignments.Include(a => a.User).FirstOrDefaultAsync(a => a.Id == updatedAssignment.Id);
                if (assignment.User.Id == GetUserId())
                {
                    if ((updatedAssignment.Status == "Done" && updatedAssignment.TimeSpent == null) ||
                        (updatedAssignment.Status == "Done" && updatedAssignment.WayOfSolving == null)
                    )
                    {
                        serviceResponse.Success = false;
                        serviceResponse.Message = "Please enter informations about time spent for solving this assignment and way of solving.";
                    }
                    else if (DateTime.Compare(updatedAssignment.StartDate, updatedAssignment.EndDate) > 0)
                    {
                        serviceResponse.Success = false;
                        serviceResponse.Message = "Start date is later than End date.";
                    }
                    else
                    {
                        assignment.AssignmentDescription = updatedAssignment.AssignmentDescription;
                        assignment.StartDate = updatedAssignment.StartDate;
                        assignment.EndDate = updatedAssignment.EndDate;
                        assignment.Note = updatedAssignment.Note;
                        assignment.Priority = updatedAssignment.Priority;
                        assignment.EditDate = DateTime.UtcNow;
                        assignment.UserIdEdit = GetUserId();
                        if (assignment.Status == "Done" && updatedAssignment.Status != "Done")
                        {
                            assignment.Status = updatedAssignment.Status;
                            assignment.TimeSpent = null;
                            assignment.WayOfSolving = null;
                        }
                        else
                        {
                            assignment.Status = updatedAssignment.Status;
                            assignment.TimeSpent = updatedAssignment.TimeSpent;
                            assignment.WayOfSolving = updatedAssignment.WayOfSolving;
                        }
                        _context.Assignments.Update(assignment);
                        await _context.SaveChangesAsync();

                        serviceResponse.Data = _mapper.Map<GetAssignmentDto>(assignment);
                    }
                }
                else
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Assignment not found.";
                }
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetAssignmentDto>>> DeleteAssignment(int id)
        {
            ServiceResponse<List<GetAssignmentDto>> serviceResponse = new ServiceResponse<List<GetAssignmentDto>>();
            try
            {
                Assignment assignment = await _context.Assignments
                .FirstOrDefaultAsync(a => a.Id == id && a.User.Id == GetUserId());
                if (assignment != null)
                {
                    _context.Assignments.Remove(assignment);
                    await _context.SaveChangesAsync();
                    serviceResponse.Data = (_context.Assignments.Where(a => a.User.Id == GetUserId())
                    .Select(a => _mapper.Map<GetAssignmentDto>(a))).ToList();
                }
                else
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Assignment not found.";
                }


            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }
    }
}