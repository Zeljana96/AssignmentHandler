using System;
using Tasks_Handler.Models;

namespace Tasks_Handler.Dtos.Assignment
{
    public class GetAssignmentDto
    {
        public int Id { get; set; }
        public string AssignmentDescription { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Note { get; set; }
        public string Status { get; set; } = StatusClass.Created.ToString("G");
        public string Priority { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime EditDate { get; set; }
        public int UserIdEdit { get; set; }
        public string TimeSpent { get; set; }
        public string WayOfSolving { get; set; }
    }
}