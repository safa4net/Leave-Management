using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Leave_Management.Contracts;
using Leave_Management.Data;
using Leave_Management.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Leave_Management.Controllers
{
    [Authorize(Roles = "Administrator,Employee")]
    public class LeaveTypesController : Controller
    {
        private readonly ILeaveTypeRepository _repo;
        private readonly IMapper _mapper;

        public LeaveTypesController(ILeaveTypeRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        // GET: LeaveTypes
        public ActionResult Index()
        {
            var leaveTypes = _repo.FindAll().ToList();
            var model = _mapper.Map<List<LeaveType>, List<LeaveTypeDetailsViewModel>>(leaveTypes);
            return View(model);
        }

        // GET: LeaveTypes/Details/5
        public ActionResult Details(int id)
        {
            if (!_repo.IsExists(id))
                return NotFound();
            var leaveType = _repo.FindById(id);
            var model = _mapper.Map<LeaveTypeDetailsViewModel>(leaveType);
            return View(model);
        }

        // GET: LeaveTypes/Create
        [Authorize(Roles = "Administrator")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: LeaveTypes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult Create(LeaveTypeCreateViewModel viewModel)
        {
            try
            {
                // TODO: Add insert logic here
                if (!ModelState.IsValid) return RedirectToAction(nameof(Index));

                var leaveType = _mapper.Map<LeaveType>(viewModel);
                leaveType.DateCreated = DateTime.Now;

                if (_repo.Create(leaveType))
                    return RedirectToAction(nameof(Index));

                ModelState.AddModelError("", "بروز خطا در ثبت اطلاعات در سیستم");
                return View(viewModel);

            }
            catch
            {
                ModelState.AddModelError("", "بروز خطا در ثبت اطلاعات در سیستم");
                return View(viewModel);
            }
        }

        
        // GET: LeaveTypes/Edit/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(int id)
        {
            var isExists = _repo.IsExists(id);
            if (isExists == false)
                return NotFound();
            var leaveType = _repo.FindById(id);
            var model = _mapper.Map<LeaveTypeDetailsViewModel>(leaveType);

            return View(model);
        }

        
        // POST: LeaveTypes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(LeaveTypeDetailsViewModel viewModel)
        {
            try
            {
                // TODO: Add update logic here
                if (!ModelState.IsValid)
                    return View(viewModel);

                var leaveType = _mapper.Map<LeaveType>(viewModel);
                leaveType.DateCreated = viewModel.DateCreated;
                if (_repo.Update(leaveType))
                    return RedirectToAction(nameof(Index));

                ModelState.AddModelError("", "بروز خطا در به روز رسانی اطلاعات");
                return View(viewModel);
            }
            catch
            {
                ModelState.AddModelError("", "بروز خطا در به روز رسانی اطلاعات");
                return View(viewModel);
            }
        }

        // GET: LeaveTypes/Delete/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int id)
        {
            var isExists = _repo.IsExists(id);
            if (isExists == false)
                return NotFound();

            var leaveType = _repo.FindById(id);
            //var model = _mapper.Map<LeaveTypeDetailsViewModel>(leaveType);

            //return View(model);

            if (!_repo.Delete(leaveType))
                return BadRequest();
            return RedirectToAction(nameof(Index));
        }

        // POST: LeaveTypes/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int id, LeaveTypeDetailsViewModel viewModel)
        {
            try
            {
                // TODO: Add delete logic here
                if (_repo.IsExists(id) == false)
                    return NotFound();

                var leaveYpe = _repo.FindById(id);
                if (_repo.Delete(leaveYpe))
                    return RedirectToAction(nameof(Index));
                ModelState.AddModelError("", "بروز خطا در حذف اطلاعات از سیستم");
                return View(viewModel);
            }
            catch
            {
                ModelState.AddModelError("", "بروز خطا در حذف اطلاعات از سیستم");
                return View(viewModel);
            }
        }
    }
}