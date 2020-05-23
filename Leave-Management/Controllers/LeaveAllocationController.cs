using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Leave_Management.Contracts;
using Leave_Management.Data;
using Leave_Management.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Leave_Management.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class LeaveAllocationController : Controller
    {
        #region Variables DI

        private readonly ILeaveTypeRepository _leaveTypeRepository;
        private readonly ILeaveAllocationRepository _leaveAllocationRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<Employee> _userManager;

        #endregion

        #region Constructor

        public LeaveAllocationController(ILeaveTypeRepository leaveTypeRepository,
            ILeaveAllocationRepository leaveAllocationRepository,
            IMapper mapper, UserManager<Employee> userManager)
        {
            _leaveTypeRepository = leaveTypeRepository;
            _leaveAllocationRepository = leaveAllocationRepository;
            _mapper = mapper;
            _userManager = userManager;
        }

        #endregion

        #region Index

        // GET: LeaveAllocation
        public ActionResult Index()
        {
            var leaveTypes = _leaveTypeRepository.FindAll().ToList();
            var mappedLeaveTypes =
                _mapper.Map<List<LeaveType>, List<LeaveTypeDetailsViewModel>>(leaveTypes);
            var model = new LeaveAllocationCreateViewModel
            {
                LeaveTypes = mappedLeaveTypes,
                NumberUpdated = 0
            };
            return View(model);
        }

        #endregion

        #region SetLeave

        public IActionResult SetLeave(int id)
        {
            var leaveType = _leaveTypeRepository.FindById(id);
            var employees = _userManager.GetUsersInRoleAsync("Employee").Result;
            foreach (var employee in employees)
            {
                if (_leaveAllocationRepository.CheckAllocation(id, employee.Id))
                    continue;
                var allocation = new LeaveAllocationViewModel
                {
                    DateCreated = DateTime.Now,
                    EmployeeId = employee.Id,
                    LeaveTypeId = id,
                    NumberOfDays = leaveType.DefaultDays,
                    Period = DateTime.Now.Year
                };
                var leaveAllocation = _mapper.Map<LeaveAllocation>(allocation);
                _leaveAllocationRepository.Create(leaveAllocation);
            }

            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region List Employees

        public IActionResult ListEmployees()
        {
            var employees = _userManager.GetUsersInRoleAsync("Employee").Result;
            var model = _mapper.Map<List<EmployeeViewModel>>(employees);
            return View(model);
        }

        #endregion

        #region Details

        // GET: LeaveAllocation/Details/5
        public ActionResult Details(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest();
            var employee = _mapper.Map<EmployeeViewModel>(_userManager.FindByIdAsync(id).Result);
            var period = DateTime.Now.Year;

            var allocations =
                _mapper.Map<List<LeaveAllocationViewModel>>(_leaveAllocationRepository.GetLeaveAllocationsByEmployee(id));

            var model = new LeaveAllocationsViewViewModel
            {
                Employee = employee,
                LeaveAllocations = allocations,
                EmployeeId = id
            };
            return View(model);
        }

        #endregion

        #region Edit

        // GET: LeaveAllocation/Edit/5
        public ActionResult Edit(int id)
        {
            var leaveAllocation = _leaveAllocationRepository.FindById(id);
            if (leaveAllocation == null)
                return BadRequest();
            var model = _mapper.Map<LeaveAllocationEditViewModel>(leaveAllocation);

            return View(model);
        }

        // POST: LeaveAllocation/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(LeaveAllocationEditViewModel viewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(viewModel);
                }

                var record = _leaveAllocationRepository.FindById(viewModel.Id);
                record.NumberOfDays = viewModel.NumberOfDays;
                var isSuccess = _leaveAllocationRepository.Update(record);
                if (!isSuccess)
                {
                    ModelState.AddModelError("", "بروز خطا در ثبت اطلاعات");
                    return View(viewModel);
                }

                return RedirectToAction(nameof(Details), new { id = viewModel.EmployeeId });
            }
            catch
            {
                ModelState.AddModelError("", "بروز خطا در ثبت اطلاعات");
                return View(viewModel);
            }
        }

        #endregion

        #region Delete

        // GET: LeaveAllocation/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: LeaveAllocation/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        #endregion

        #region Create

        // GET: LeaveAllocation/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LeaveAllocation/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        #endregion
    }
}