using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Leave_Management.Data
{
    public class LeaveAllocation
    {
        [Key]
        public int Id { get; set; }

        public int NumberOfDays { get; set; }

        public DateTime DateCreated { get; set; }

        [ForeignKey(nameof(EmploeeId))]
        public Employee Employee { get; set; }

        public string EmploeeId { get; set; }

        [ForeignKey(nameof(LeaveTypeId))]
        public LeaveType LeaveType { get; set; }

        public int LeaveTypeId { get; set; }
    }
}