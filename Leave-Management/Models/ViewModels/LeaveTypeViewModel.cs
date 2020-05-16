using System;
using System.ComponentModel.DataAnnotations;

namespace Leave_Management.Models.ViewModels
{
    public class LeaveTypeDetailsViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "ورود نام برای دلیل خروج الزامی است.")]
        [Display(Name = "نام")]
        [MaxLength(200,ErrorMessage = "حداکثر طول نام می بایست 200 حرف باشد.")]
        public string Name { get; set; }

        [Display(Name = "تاریخ ایجاد")]
        [DataType(DataType.Date)]
        public DateTime DateCreated { get; set; }
    }

    public class LeaveTypeCreateViewModel
    {
        public int Id { get; set; }

        [Display(Name = "علت مرخصی")]
        [Required(ErrorMessage = "ورود نام برای {0} الزامی است.")]
        [StringLength(100, ErrorMessage = "تعداد حروف {0} می بایست بین {1} حرف و {2} باشد.", MinimumLength = 3)]
        public string Name { get; set; }
    }
}