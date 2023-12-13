using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.BLL.Repositories;
using Demo.DAL.Models;
using Demo.PL.Helpers;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Demo.PL.Controllers
{
    public class EmployeeController : Controller
    {
        
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EmployeeController(IUnitOfWork unitOfWork,
            IMapper mapper)// Ask CLR FOR creating Object from class Implement IEmployeeRepository
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index(string SearchValue)
        {
            //1.ViewData => KeyValuePair[Dictionary Object]
            // Transfer Data from Controller [action] to its view 
            // .Net framework 3.5
            //ViewData["Message"] = "Hello from View Data";
            // ViewBag =>Dynamic Property [Based On Dynamic Keyword]
            // Transfer Data from Controller [action] to its view 
            // .Net Framework 4.0
            //ViewBag.Message = "Hello from View Bag";
            IEnumerable<Employee> employees;
            if (string.IsNullOrEmpty(SearchValue))
            {
                 employees = await _unitOfWork.EmployeeRepository.GetAllAsync();
                
            }
            else
            {
                 employees =  _unitOfWork.EmployeeRepository.SearchEmployeesByName(SearchValue);
                
            }
            var MappedEmployees = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees);
            return View(MappedEmployees);

        }
        [HttpGet]   
        public IActionResult Create()
        {
           //ViewBag.Departments = _departmentRepository.GetAll();
            return View();
        }
        [HttpPost]
        public async  Task<IActionResult> Create(EmployeeViewModel EmployeeVM)
        {
            if (ModelState.IsValid) // Server side validation 
            {

                #region Manula Mapping
                // Mapping from EmployeeViewModel to employee
                //Manual Mapping
                //    var employee = new Employee()
                //    {
                //        Name = EmployeeVM.Name,
                //        Age = EmployeeVM.Age,
                //        Address = EmployeeVM.Address,
                //        PhoneNumber = EmployeeVM.PhoneNumber,
                //};
                //Employee employee = (Employee)EmployeeVM; // Unsafe 
                #endregion
                EmployeeVM.ImageName = DocumentSettings.UploadFile(EmployeeVM.Image, "Images");

                var MappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(EmployeeVM);
                  await _unitOfWork.EmployeeRepository.AddAsync(MappedEmployee);
                // Add
                // Update
                // Delete

                //SaveChanges();
               await _unitOfWork.CompleteAsync();
                //if (Result > 0)
                //{
                //    TempData["EmpMessage"] = "Employee Is Added";
                //}
                return RedirectToAction(nameof(Index));

            }
            return View(EmployeeVM);
        }

        public async Task<IActionResult> Details(int? id, string ViewName = "Details")
        {
            if (id is null)
                return BadRequest(); // status code 400 
            var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(id.Value);
            if (employee is null)
                return NotFound();
            var MappedEmployee = _mapper.Map<Employee, EmployeeViewModel>(employee);
            return View(ViewName, MappedEmployee);

        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.Departments = await _unitOfWork.DepartmentRepository.GetAllAsync();

            //if (id is null)
            //    return BadRequest(); // status code 400 
            //var department = _departmentRepository.GetById(id.Value);
            //if (department is null)
            //    return NotFound();
            //return View(department);
            return await Details(id, "Edit");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EmployeeViewModel EmployeeVM , [FromRoute] int id)
        {

            if (id != EmployeeVM.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    if(EmployeeVM.Image is not null)
                    {
						EmployeeVM.ImageName = DocumentSettings.UploadFile(EmployeeVM.Image, "Images");

					}
					var MappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(EmployeeVM);

                    _unitOfWork.EmployeeRepository.Update(MappedEmployee);
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
            return View(EmployeeVM);
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
        public async Task<IActionResult> Delete(EmployeeViewModel EmployeeVM, [FromRoute] int id)
        {

            if (id != EmployeeVM.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {

                    var MappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(EmployeeVM);

                    _unitOfWork.EmployeeRepository.Delete(MappedEmployee);
                   int Result = await _unitOfWork.CompleteAsync();
                    if (Result >0)
                    {
                        DocumentSettings.DeleteFile(EmployeeVM.ImageName, "Images");
                    }
                    return RedirectToAction(nameof(Index));

                }
                catch (System.Exception ex)
                {
                    // 1.Log Exception
                    // 2.Form
                    ModelState.AddModelError(string.Empty, ex.Message);
                }

            }
            return View(EmployeeVM);
        }
    }
}
