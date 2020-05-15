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
            var result = _db.LeaveAllocations.AsNoTracking().ToList();
            return result;
        }

        public LeaveAllocation FindById(int id)
        {
            var result = _db.LeaveAllocations.Find(id);
            return result;
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
    }
}
