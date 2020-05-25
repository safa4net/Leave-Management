using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Leave_Management.Models.ViewModels
{
    public class LeaveRequestViewModel
    {
        public int Id { get; set; }

        [Display(Name = "درخواست دهنده")]
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

        [Display(Name = "مرخصی درخواستی")]
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

        public bool? Cancelled { get; set; }

        [Display(Name = "توضیحات")]
        [DataType(DataType.Text)]
        [MaxLength(1000,ErrorMessage = "حداکثر تعداد حروف 1000 حرف می باشد.")]
        public string RequestComments { get; set; }
    }

    public class LeaveRequestAdminViewModel
    {
        [Display(Name = "مجموع درخواست ها")]
        public int TotalRequests { get; set; }

        [Display(Name = "تائید شده")]
        public int ApprovedRequests { get; set; }

        [Display(Name = "در دست بررسی")]
        public int PendingRequests { get; set; }

        [Display(Name = "رد شده")]
        public int RejectedRequests { get; set; }

        public List<LeaveRequestViewModel> LeaveRequest { get; set; }
    }

    public class LeaveRequestCreateViewModel
    {
        [Display(Name = "تاریخ شروع")]
        [Required(ErrorMessage = "وارد نمودن {0} الزامی است.")]
        public string StartDate { get; set; }

        [Display(Name = "تاریخ پایان")]
        [Required(ErrorMessage = "وارد نمودن {0} الزامی است.")]
        public string EndDate { get; set; }
        
        [Display(Name = "نوع مرخصی")]
        [Required(ErrorMessage = "وارد نمودن {0} الزامی است.")]
        public int? LeaveTypeId { get; set; }

        [Display(Name = "توضیحات")]
        [DataType(DataType.Text)]
        [MaxLength(1000,ErrorMessage = "حداکثر تعداد حروف 1000 حرف می باشد.")]
        public string RequestComments { get; set; }

        public IEnumerable<SelectListItem> LeaveTypes { get; set; }
    }

    public class LeaveRequestEmployeeViewViewModel
    {
        public List<LeaveAllocationViewModel> LeaveAllocations { get; set; }
        public List<LeaveRequestViewModel> LeaveRequests { get; set; }
    }
}
