﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Leave_Management.Data
{
    public class LeaveRequest
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(RequestingEmployeeId))]
        public Employee RequestingEmployee { get; set; }

        public string RequestingEmployeeId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        [ForeignKey(nameof(LeaveTypeId))]
        public LeaveType LeaveType { get; set; }

        public int LeaveTypeId { get; set; }

        public DateTime DateRequested { get; set; }

        public DateTime DateActioned { get; set; }

        public bool? Approved { get; set; }

        [ForeignKey(nameof(ApprovedById))]
        public Employee ApprovedBy { get; set; }

        public string ApprovedById { get; set; }
    }
}