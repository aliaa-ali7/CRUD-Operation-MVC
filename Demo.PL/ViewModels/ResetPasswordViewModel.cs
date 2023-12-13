using System.ComponentModel.DataAnnotations;

namespace Demo.PL.ViewModels
{
	public class ResetPasswordViewModel
	{
		[Required(ErrorMessage ="New Password Is Required")]
		[DataType(DataType.Password)]
        public string NewPassword { get; set; }

		[Required(ErrorMessage = "New Password Is Required")]
		[DataType(DataType.Password)]
		[Compare("NewPassword", ErrorMessage ="Password Does not match")]
		public string ConfirmNewPassword { get; set; }

	}
}
