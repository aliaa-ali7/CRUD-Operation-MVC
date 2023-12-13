using Demo.DAL.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace Demo.PL.ViewModels
{
	[Authorize]

	public class EmployeeViewModel
    {
        public int Id { get; set; } //PK
        [Required(ErrorMessage = "Name is Required")]
        [MaxLength(50, ErrorMessage = "Max length is 50")]
        [MinLength(5, ErrorMessage = "Max length is 5")]
        public string Name { get; set; }
        [Range(22, 35, ErrorMessage = "Age must be in 22 and 35")]
        public int? Age { get; set; }
        [RegularExpression("^[0-9]{1,3}-[a-zA-Z]{5,10}-[a-zA-Z]{4,10}-[a-zA-Z]{5,10}$", ErrorMessage = "Address must be like 123-Street-City-Country")]
        public string Address { get; set; }
        [DataType(DataType.Currency)]
        public decimal? Salary { get; set; }

        public bool IsActive { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
        public DateTime HireDate { get; set; }
        public string ImageName { get; set; }

        public IFormFile Image { get; set; }
        [ForeignKey("Department")]
        public int? DepartmentId { get; set; } // FK
        // FK optional => OnDelete : Restrict (can not delete department contain employees)
        // FK Required => OnDelete " Cascade (if delete department will delete employees in it)
        [InverseProperty("Employees")]
        public Department Department { get; set; }
    }
}
