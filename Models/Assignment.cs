using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tasks_Handler.Models
{
    public class Assignment
    {
        public int Id { get; set; }
        public string AssignmentDescription { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Note { get; set; }
        public string Status { get; set; } = StatusClass.Created.ToString("G");
        public string Priority { get; set; }
        public User User { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime EditDate { get; set; } = DateTime.UtcNow;
        public int UserIdEdit { get; set; }
        public string TimeSpent { get; set; }
        public string WayOfSolving { get; set; }
    }
}