using System.Collections.Generic;
using Leave_Management.Data;

namespace Leave_Management.Contracts
{
    public interface ILeaveTypeRepository:IRepositoryBase<LeaveType>
    {
        ICollection<LeaveType> GetEmployeesByLeaveType(int id);
    }
}
