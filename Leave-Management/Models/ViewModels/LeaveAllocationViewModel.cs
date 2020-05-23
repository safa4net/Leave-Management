using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Leave_Management.Data;

namespace Leave_Management.Models.ViewModels
{
    public class LeaveAllocationViewModel
    {
        public int Id { get; set; }

        [Display(Name = "تعداد روز")]
        [Required(ErrorMessage = "تعداد روز باید وارد شود.")]
        public int NumberOfDays { get; set; }

        public int Period { get; set; }

        [Display(Name = "تاریخ ایجاد")]
        [DataType(DataType.DateTime)]
        public DateTime DateCreated { get; set; }

        [Display(Name = "نام کارمند")]
        public string EmployeeId { get; set; }

        public LeaveTypeDetailsViewModel LeaveType { get; set; }

        [Display(Name = "دلیل مرخصی")]
        public int LeaveTypeId { get; set; }
    }

    public class LeaveAllocationCreateViewModel
    {
        public int NumberUpdated { get; set; }
        public List<LeaveTypeDetailsViewModel> LeaveTypes { get; set; }
    }

    public class LeaveAllocationsViewViewModel
    {
        public EmployeeViewModel Employee { get; set; }
        public string EmployeeId { get; set; }
        public List<LeaveAllocationViewModel> LeaveAllocations { get; set; }
    }

    public class LeaveAllocationEditViewModel
    {
        public int Id { get; set; }

        [Display(Name = "تعداد روز")]
        [Required(ErrorMessage = "تعداد روز باید وارد شود.")]
        public int NumberOfDays { get; set; }

        public LeaveTypeDetailsViewModel LeaveType { get; set; }

        public EmployeeViewModel Employee { get; set; }
        public string EmployeeId { get; set; }
    }
}