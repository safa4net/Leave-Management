using System;
using System.ComponentModel.DataAnnotations;

namespace Leave_Management.Models.ViewModels
{
    public class EmployeeViewModel
    {
        [Display(Name = "نام")]
        [Required(ErrorMessage = "{0} کارمند می بایست وارد شود.")]
        [StringLength(100, ErrorMessage = "تعداد حروف {0} می بایست بین {1} حرف و {2} باشد.", MinimumLength = 3)]
        public string Firstname { get; set; }

        [Display(Name = "نام خانوادگی")]
        [Required(ErrorMessage = "{0} کارمند می بایست وارد شود.")]
        [StringLength(100, ErrorMessage = "تعداد حروف {0} می بایست بین {1} حرف و {2} باشد.", MinimumLength = 3)]
        public string Lastname { get; set; }

        [Display(Name = "تلفن همراه")]
        [Required(ErrorMessage = "{0} کارمند می بایست وارد شود")]
        [StringLength(11, ErrorMessage = "تعداد اعداد {0} می بایست {1} عدد باشد.", MinimumLength = 11)]
        [DataType(DataType.PhoneNumber, ErrorMessage = "{0} وارد شده صحیح نمی باشد.")]
        public string PhoneNumber { get; set; }

        [Display(Name = "کد مالیاتی")]
        [Required(ErrorMessage = "{0} می باست وارد شود.")]
        public string TaxId { get; set; }

        [Display(Name = "تاریخ تولد")]
        [Required(ErrorMessage = "{0} می بایست وارد شود.")]
        [DataType(DataType.Date, ErrorMessage = "")]
        public DateTime DateOfBirth { get; set; }

        [Display(Name = "تاریخ همراهی")]
        [DataType(DataType.Date)]
        public DateTime DateJoined { get; set; }

        [Display(Name = "تاریخ ایجاد")]
        [DataType(DataType.DateTime)]
        public DateTime DateCreated { get; set; }
    }
}