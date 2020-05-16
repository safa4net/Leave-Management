using System.Collections.Generic;
using System.Linq;
using Leave_Management.Contracts;
using Leave_Management.Data;

namespace Leave_Management.Repository
{
    public class LeaveHistoryRepository : ILeaveHistoryRepository
    {
        private readonly ApplicationDbContext _db;

        public LeaveHistoryRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public ICollection<LeaveHistory> FindAll()
        {
            var result = _db.LeaveHistories.ToList();
            return result;
        }

        public LeaveHistory FindById(int id)
        {
            var result = _db.LeaveHistories.Find(id);
            return result;
        }

        public bool IsExists(int id)
        {
            var exists = _db.LeaveHistories.Any(x => x.Id == id);
            return exists;
        }

        public bool Create(LeaveHistory entity)
        {
            _db.LeaveHistories.Add(entity);
            return Save();
        }

        public bool Update(LeaveHistory entity)
        {
            _db.LeaveHistories.Update(entity);
            return Save();
        }

        public bool Delete(LeaveHistory entity)
        {
            _db.LeaveHistories.Remove(entity);
            return Save();
        }

        public bool Save()
        {
            return _db.SaveChanges() > 0;
        }
    }
}
