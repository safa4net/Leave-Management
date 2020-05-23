using System;
using System.Collections.Generic;
using System.Linq;
using Leave_Management.Contracts;
using Leave_Management.Data;
using Microsoft.EntityFrameworkCore;

namespace Leave_Management.Repository
{
    public class LeaveAllocationRepository : ILeaveAllocationRepository
    {
        private readonly ApplicationDbContext _db;

        public LeaveAllocationRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public ICollection<LeaveAllocation> FindAll()
        {
            var result = _db.LeaveAllocations
                .Include(q => q.LeaveType)
                .Include(q => q.Employee)
                .ToList();
            return result;
        }

        public LeaveAllocation FindById(int id)
        {
            var result = _db.LeaveAllocations
                .Include(q => q.LeaveType)
                .Include(q => q.Employee).FirstOrDefault(q => q.Id == id);
            return result;
        }

        public bool IsExists(int id)
        {
            var exists = _db.LeaveAllocations.Any(x => x.Id == id);
            return exists;
        }

        public bool Create(LeaveAllocation entity)
        {
            _db.LeaveAllocations.Add(entity);
            return Save();
        }

        public bool Update(LeaveAllocation entity)
        {
            _db.LeaveAllocations.Update(entity);
            return Save();
        }

        public bool Delete(LeaveAllocation entity)
        {
            _db.LeaveAllocations.Remove(entity);
            return Save();
        }

        public bool Save()
        {
            return _db.SaveChanges() > 0;
        }

        public bool CheckAllocation(int leaveTypeId, string employeeId)
        {
            var period = DateTime.Now.Year;
            return FindAll().Any(x => x.EmployeeId == employeeId &&
                                      x.LeaveTypeId == leaveTypeId &&
                                      x.Period == period);
        }

        public ICollection<LeaveAllocation> GetLeaveAllocationsByEmployee(string employeeId)
        {
            var period = DateTime.Now.Year;
            return FindAll().Where(q => q.EmployeeId == employeeId &&
                                                     q.Period == period)
                            .ToList();
        }

        public LeaveAllocation GetLeaveAllocationsByEmployeeAndType(string employeeId, int leaveTypeId)
        {
            var period = DateTime.Now.Year;
            return FindAll().FirstOrDefault(q =>
                q.EmployeeId == employeeId &&
                q.Period == period &&
                q.LeaveTypeId == leaveTypeId);
        }
    }
}
