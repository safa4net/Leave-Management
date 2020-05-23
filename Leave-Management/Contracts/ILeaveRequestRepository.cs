using System.Collections;
using System.Collections.Generic;
using Leave_Management.Data;

namespace Leave_Management.Contracts
{
    public interface ILeaveRequestRepository : IRepositoryBase<LeaveRequest>
    {
        ICollection<LeaveRequest> GetLeaveRequestByEmployee(string employeeId);
    }
}
