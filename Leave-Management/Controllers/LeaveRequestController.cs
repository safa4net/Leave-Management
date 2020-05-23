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
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Leave_Management.Controllers
{
    [Authorize]
    public class LeaveRequestController : Controller
    {
        #region Variables

        private readonly ILeaveAllocationRepository _leaveAllocationRepo;
        private readonly ILeaveRequestRepository _leaveRequestRepo;
        private readonly ILeaveTypeRepository _leaveTypeRepo;
        private readonly IMapper _mapper;
        private readonly UserManager<Employee> _userManager;

        #endregion

        #region Construsctor

        public LeaveRequestController(ILeaveRequestRepository leaveRequestRepo,
            IMapper mapper,
            UserManager<Employee> userManager, ILeaveTypeRepository leaveTypeRepo, ILeaveAllocationRepository leaveAllocationRepo)
        {
            _leaveRequestRepo = leaveRequestRepo;
            _mapper = mapper;
            _userManager = userManager;
            _leaveTypeRepo = leaveTypeRepo;
            _leaveAllocationRepo = leaveAllocationRepo;
        }

        #endregion

        #region Index

        [Authorize(Roles = "Administrator")]
        // GET: LeaveRequest
        public ActionResult Index()
        {
            var leaveRequests = _leaveRequestRepo.FindAll();
            var leaveRequestsModel = _mapper.Map<List<LeaveRequestViewModel>>(leaveRequests);
            var model = new LeaveRequestAdminViewModel
            {
                TotalRequests = leaveRequestsModel.Count,
                ApprovedRequests = leaveRequestsModel.Count(x => x.Approved == true),
                RejectedRequests = leaveRequestsModel.Count(x => x.Approved == false),
                PendingRequests = leaveRequestsModel.Count(x => x.Approved == null),
                LeaveRequest = leaveRequestsModel
            };
            return View(model);
        }

        #endregion

        #region Create

        // GET: LeaveRequest/Create
        public ActionResult Create()
        {
            var leaveTypes = _leaveTypeRepo.FindAll();
            var leaveTypeItems = leaveTypes.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });
            var model = new LeaveRequestCreateViewModel
            {
                LeaveTypes = leaveTypeItems
            };
            return View(model);
        }

        // POST: LeaveRequest/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(LeaveRequestCreateViewModel viewModel)
        {
            try
            {
                var leaveTypes = _leaveTypeRepo.FindAll();
                var leaveTypeItems = leaveTypes.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                });
                viewModel.LeaveTypes = leaveTypeItems;

                DateTime startDate = Convert.ToDateTime(viewModel.StartDate);
                DateTime endDate = Convert.ToDateTime(viewModel.EndDate);

                if (!ModelState.IsValid)
                    return View(viewModel);

                if (DateTime.Compare(startDate, endDate) > 1)
                {
                    ModelState.AddModelError("", "تاریخ پایان مرخصی نبایست کمتر از تاریخ شروع آن باشد.");
                    return View(viewModel);
                }

                var employee = _userManager.GetUserAsync(User).Result;
                if (employee == null)
                    return BadRequest();
                var allocation =
                    _leaveAllocationRepo.GetLeaveAllocationsByEmployeeAndType(employee.Id, viewModel.LeaveTypeId.Value);

                if (allocation == null)
                {
                    ModelState.AddModelError("", "برای کاربر شما در سیستم مرخصی مورد نظر تعیین نگردیده است.");
                    return View(viewModel);
                }
                int daysRequested = (int)(startDate.Date - endDate.Date).TotalDays;
                if (daysRequested > allocation.NumberOfDays)
                {
                    ModelState.AddModelError("", "تعداد روزهای درخواستی بیشتر از حد مجاز روزهای مرخصی شما است.");
                    return View(viewModel);
                }

                var leaveRequestModel = new LeaveRequestViewModel
                {
                    RequestingEmployeeId = employee.Id,
                    DateRequested = DateTime.Now,
                    StartDate = startDate,
                    EndDate = endDate,
                    Approved = null,
                    LeaveTypeId = (int)viewModel.LeaveTypeId,
                    DateActioned = DateTime.MinValue,
                    Cancelled = false,
                    RequestComments = viewModel.RequestComments
                };

                var leaveRequest = _mapper.Map<LeaveRequest>(leaveRequestModel);
                var isSuccess = _leaveRequestRepo.Create(leaveRequest);
                if (!isSuccess)
                {
                    ModelState.AddModelError("", "بروز خطا در ثبت اطلاعات");
                    return View(viewModel);
                }

                return RedirectToAction(nameof(MyLeave));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "بروز خطا در ثبت اطلاعات" + "\n" + ex.Message);
                return View(viewModel);
            }
        }

        #endregion

        #region Details

        // GET: LeaveRequest/Details/5
        public ActionResult Details(int id)
        {
            var leaveRequest = _leaveRequestRepo.FindById(id);
            if (leaveRequest == null)
                return NotFound();
            var model = _mapper.Map<LeaveRequestViewModel>(leaveRequest);
            return View(model);
        }

        #endregion

        #region Approve Leave Request

        public IActionResult ApproveRequest(int id)
        {
            try
            {
                var leaveRequest = _leaveRequestRepo.FindById(id);
                if (leaveRequest == null)
                    return NotFound();
                var employeeId = leaveRequest.RequestingEmployeeId;
                var leaveTypeId = leaveRequest.LeaveTypeId;

                var allocation = _leaveAllocationRepo.GetLeaveAllocationsByEmployeeAndType(employeeId, leaveTypeId);
                int daysRequested = (int)(leaveRequest.EndDate.Date - leaveRequest.StartDate.Date).TotalDays;

                allocation.NumberOfDays -= daysRequested;
                if (allocation.NumberOfDays >= 0)
                {
                    leaveRequest.Approved = true;
                    leaveRequest.DateActioned = DateTime.Now;
                    var user = _userManager.GetUserAsync(User).Result;
                    leaveRequest.ApprovedById = user.Id;

                    var isSuccessleaveRequest = _leaveRequestRepo.Update(leaveRequest);
                    var isSuccessLeaveAllocation = _leaveAllocationRepo.Update(allocation);

                    if (!isSuccessleaveRequest || !isSuccessLeaveAllocation)
                    {
                        return RedirectToAction("Details", routeValues: id);
                    }

                    return RedirectToAction(nameof(Index));
                }
                return RedirectToAction("Details", routeValues: id);

            }
            catch
            {
                return RedirectToAction("Details", routeValues: id);
            }
        }

        #endregion

        #region Reject Leave Request

        public IActionResult RejectRequest(int id)
        {
            try
            {
                var leaveRequest = _leaveRequestRepo.FindById(id);
                if (leaveRequest == null)
                    return NotFound();
                leaveRequest.Approved = false;
                leaveRequest.DateActioned = DateTime.Now;
                var user = _userManager.GetUserAsync(User).Result;
                leaveRequest.ApprovedById = user.Id;

                var isSuccess = _leaveRequestRepo.Update(leaveRequest);
                if (!isSuccess)
                {
                    return RedirectToAction("Details", routeValues: id);
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction("Details", routeValues: id);
            }


        }

        #endregion

        #region MyLeave

        public ActionResult MyLeave()
        {
            var employee = _userManager.GetUserAsync(User).Result;
            var employeeId = employee.Id;
            var employeeAllocations = _leaveAllocationRepo.GetLeaveAllocationsByEmployee(employeeId);
            var employeeRequests = _leaveRequestRepo.GetLeaveRequestByEmployee(employeeId);

            var employeeAllocationModel = _mapper.Map<List<LeaveAllocationViewModel>>(employeeAllocations);
            var employeeRequestModel = _mapper.Map<List<LeaveRequestViewModel>>(employeeRequests);

            var model = new LeaveRequestEmployeeViewViewModel
            {
                LeaveAllocations = employeeAllocationModel,
                LeaveRequests = employeeRequestModel
            };
            return View(model);
        }

        #endregion

        #region CancelRequest

        public IActionResult CancelRequest(int id)
        {
            var leaveRequest = _leaveRequestRepo.FindById(id);
            if (leaveRequest == null)
                return RedirectToAction(nameof(Index));
            leaveRequest.Cancelled = true;
            _leaveRequestRepo.Update(leaveRequest);

            return RedirectToAction(nameof(MyLeave));
        }

        #endregion

        #region Edit

        // GET: LeaveRequest/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: LeaveRequest/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        #endregion

        #region Delete

        // GET: LeaveRequest/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: LeaveRequest/Delete/5
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
    }
}