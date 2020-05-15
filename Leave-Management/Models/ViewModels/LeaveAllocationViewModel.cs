using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Leave_Management.Models.ViewModels
{
    public class LeaveAllocationViewModel
    {
        public int Id { get; set; }

        [Display(Name = "تعداد روز")]
        [Required(ErrorMessage = "تعداد روز باید وارد شود.")]
        public int NumberOfDays { get; set; }

        [Display(Name = "تاریخ ایجاد")]
        [DataType(DataType.DateTime)]
        public DateTime DateCreated { get; set; }

        [Display(Name = "نام کارمند")]
        public string EmployeeId { get; set; }

        public LeaveTypeDetailsViewModel LeaveType { get; set; }

        [Display(Name = "دلیل مرخصی")]
        public int LeaveTypeId { get; set; }

        public IEnumerable<SelectListItem> Employees { get; set; }
        
        public IEnumerable<SelectListItem> LeaveTypes { get; set; }
    }
}