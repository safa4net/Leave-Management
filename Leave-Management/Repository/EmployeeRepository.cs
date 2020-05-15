using System.Collections.Generic;
using System.Linq;
using Leave_Management.Contracts;
using Leave_Management.Data;
using Microsoft.EntityFrameworkCore;

namespace Leave_Management.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _db;

        public EmployeeRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public ICollection<Employee> FindAll()
        {
            var result = _db.Employees.AsNoTracking().ToList();
            return result;
        }

        public Employee FindById(int id)
        {
            var result = _db.Employees.Find(id);
            return result;
        }

        public bool Create(Employee entity)
        {
            _db.Employees.Add(entity);
            return Save();
        }

        public bool Update(Employee entity)
        {
            _db.Employees.Update(entity);
            return Save();
        }

        public bool Delete(Employee entity)
        {
            _db.Employees.Remove(entity);
            return Save();
        }

        public bool Save()
        {
            return _db.SaveChanges() > 0;
        }
    }
}
