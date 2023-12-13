using System.ComponentModel.DataAnnotations;

namespace Demo.PL.ViewModels
{
	public class RegisterViewModel
	{
		[Required(ErrorMessage ="First Name is Required")]
		public string FName { get; set; }
		[Required(ErrorMessage = "Last Name is Required")]
		public string LName { get; set; }
		[Required(ErrorMessage ="Email Is Required")]
		[EmailAddress(ErrorMessage ="Invalid Email")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Password is required")]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[Required(ErrorMessage ="Confirm Password is required")]
		[DataType(DataType.Password)]
		[Compare("Password", ErrorMessage ="Password Does not Match")]
		public string ConfirmPassword { get; set;}

		public bool IsAgree { get; set; }

	}
}
