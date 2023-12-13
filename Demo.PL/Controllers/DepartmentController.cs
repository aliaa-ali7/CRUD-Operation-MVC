using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.BLL.Repositories;
using Demo.DAL.Models;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    [Authorize]
	public class DepartmentController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DepartmentController(IUnitOfWork unitOfWork, IMapper mapper) // Ask CLR For Creating Object form Class IDepartmentRepository 
		{
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }
        public async Task<IActionResult> Index()
		{
			var departments = await _unitOfWork.DepartmentRepository.GetAllAsync();
            var MappedDepartments = _mapper.Map<IEnumerable<Department>, IEnumerable<DepartmentViewModel>>(departments);

            return View(MappedDepartments);
		}
        [HttpGet]
        public IActionResult Create()
        {
            
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(DepartmentViewModel DepartmentVM)
        {
            if (ModelState.IsValid) // Server side validation 
            {
                var MappedDepartment =  _mapper.Map<DepartmentViewModel, Department>(DepartmentVM);
                await _unitOfWork.DepartmentRepository.AddAsync(MappedDepartment);
               int Result = await _unitOfWork.CompleteAsync();
                if (Result > 0)
                {
                    TempData["Message"] = "Department Is Created";
                }
                return RedirectToAction(nameof(Index));
                
            }
            return View(DepartmentVM);
        }

        public  async Task<IActionResult> Details(int? id, string ViewName="Details")
        {
            if (id is null)
                return BadRequest(); // status code 400 
            var department = await _unitOfWork.DepartmentRepository.GetByIdAsync(id.Value);

            if (department is null)
                return NotFound();
            var MappedDepartment = _mapper.Map<Department, DepartmentViewModel>(department);

            return View(ViewName, MappedDepartment);
            
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            //if (id is null)
            //    return BadRequest(); // status code 400 
            //var department = _departmentRepository.GetById(id.Value);
            //if (department is null)
            //    return NotFound();
            //return View(department);
            return await Details(id,"Edit");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DepartmentViewModel DepartmentVM, [FromRoute]int id)
        {
            
            if (id != DepartmentVM.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    var MappedDepartment = _mapper.Map<DepartmentViewModel, Department>(DepartmentVM);

                    _unitOfWork.DepartmentRepository.Update(MappedDepartment);
                   await _unitOfWork.CompleteAsync();
                    return RedirectToAction(nameof(Index));

                }
                catch(System.Exception ex)
                {
                    // 1.Log Exception
                    // 2.Form
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
               
            }
            return View(DepartmentVM);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            //if (id is null)
            //    return BadRequest(); // status code 400 
            //var department = _departmentRepository.GetById(id.Value);
            //if (department is null)
            //    return NotFound();
            //return View(department);
            return await Details(id, "Delete");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(DepartmentViewModel DepartmentVM, [FromRoute] int id)
        {

            if (id != DepartmentVM.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    var MappedDepartment = _mapper.Map<DepartmentViewModel, Department>(DepartmentVM);

                    _unitOfWork.DepartmentRepository.Delete(MappedDepartment);
                   await _unitOfWork.CompleteAsync();
                    return RedirectToAction(nameof(Index));

                }
                catch (System.Exception ex)
                {
                    // 1.Log Exception
                    // 2.Form
                    ModelState.AddModelError(string.Empty, ex.Message);
                }

            }
            return View(DepartmentVM);
        }
    }
}
	 