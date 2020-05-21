using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Leave_Management.Data;

namespace Leave_Management.Models.ViewModels
{
    public class LeaveRequestViewModel
    {
        public int Id { get; set; }

        public EmployeeViewModel RequestingEmployee { get; set; }

        [Display(Name = "درخواست دهنده")]
        public string RequestingEmployeeId { get; set; }

        [Display(Name = "تاریخ شروع")]
        [Required(ErrorMessage = "تاریخ شروع می بایست وارد شود.")]
        [DataType(DataType.Date, ErrorMessage = "فرمت {0} وارد شده صحیح نمی باشد.")]
        public DateTime StartDate { get; set; }

        [Display(Name = "تاریخ اتمام")]
        [Required(ErrorMessage = "تاریخ اتمام باید مشخص شود.")]
        [DataType(DataType.Date, ErrorMessage = "فرمت {0} وارد شده صحیح نمی باشد.")]
        public DateTime EndDate { get; set; }

        public LeaveTypeDetailsViewModel LeaveType { get; set; }
        
        [Display(Name = "نوع مرخصی")]
        public int LeaveTypeId { get; set; }

        [Display(Name = "تاریخ درخواست")]
        [DataType(DataType.Date, ErrorMessage = "فرمت {0} وارد شده صحیح نمی باشد.")]
        public DateTime DateRequested { get; set; }

        [Display(Name = "تاریخ بررسی")]
        [DataType(DataType.Date, ErrorMessage = "فرمت {0} وارد شده صحیح نمی باشد.")]
        public DateTime DateActioned { get; set; }

        [Display(Name = "وضعیت تائید")]
        public bool? Approved { get; set; }

        public EmployeeViewModel ApprovedBy { get; set; }
        
        [Display(Name = "تائید شده توسط")]
        public string ApprovedById { get; set; }
    }

    public class LeaveRequestAdminViewModel
    {
        public int TotalRequests { get; set; }
        public int ApprovedRequests { get; set; }
        public int PendingRequests { get; set; }
        public int RejectedRequests { get; set; }

        public List<LeaveRequestViewModel> LeaveRequest { get; set; }
    }
}
