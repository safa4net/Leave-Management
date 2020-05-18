using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Leave_Management.Models.ViewModels;

namespace Leave_Management.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<LeaveHistory> LeaveHistories { get; set; }
        public DbSet<LeaveType> LeaveTypes { get; set; }
        public DbSet<LeaveAllocation> LeaveAllocations { get; set; }
        //public DbSet<Leave_Management.Models.ViewModels.LeaveTypeDetailsViewModel> LeaveTypeDetailsViewModel { get; set; }
        //public DbSet<Leave_Management.Models.ViewModels.LeaveTypeCreateViewModel> LeaveTypeCreateViewModel { get; set; }
    }
}
