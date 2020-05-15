using AutoMapper;
using Leave_Management.Models.ViewModels;

namespace Leave_Management.Data.Mappings
{
    public class Maps : Profile
    {
        public Maps()
        {
            CreateMap<LeaveType, LeaveTypeDetailsViewModel>().ReverseMap();
            CreateMap<LeaveType, LeaveTypeCreateViewModel>().ReverseMap();

            CreateMap<LeaveHistory, LeaveHistoryViewModel>().ReverseMap();

            CreateMap<LeaveAllocation, LeaveAllocationViewModel>().ReverseMap();

            CreateMap<Employee, EmployeeViewModel>().ReverseMap();
        }
    }
}
