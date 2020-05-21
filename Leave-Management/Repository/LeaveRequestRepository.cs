using System.Collections.Generic;
using System.Linq;
using Leave_Management.Contracts;
using Leave_Management.Data;
using Microsoft.EntityFrameworkCore;

namespace Leave_Management.Repository
{
    public class LeaveRequestRepository : ILeaveRequestRepository
    {
        private readonly ApplicationDbContext _db;

        public LeaveRequestRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public ICollection<LeaveRequest> FindAll()
        {
            var result = _db.LeaveRequests
                                            .Include(q => q.RequestingEmployee)
                                            .Include(q => q.ApprovedBy)
                                            .Include(q => q.LeaveType)
                                            .ToList();
            return result;
        }

        public LeaveRequest FindById(int id)
        {
            var result = _db.LeaveRequests
                                            .Include(q => q.RequestingEmployee)
                                            .Include(q => q.ApprovedBy)
                                            .Include(q => q.LeaveType)
                                            .FirstOrDefault(q => q.Id == id);
            return result;
        }

        public bool IsExists(int id)
        {
            var exists = _db.LeaveRequests.Any(x => x.Id == id);
            return exists;
        }

        public bool Create(LeaveRequest entity)
        {
            _db.LeaveRequests.Add(entity);
            return Save();
        }

        public bool Update(LeaveRequest entity)
        {
            _db.LeaveRequests.Update(entity);
            return Save();
        }

        public bool Delete(LeaveRequest entity)
        {
            _db.LeaveRequests.Remove(entity);
            return Save();
        }

        public bool Save()
        {
            return _db.SaveChanges() > 0;
        }
    }
}
