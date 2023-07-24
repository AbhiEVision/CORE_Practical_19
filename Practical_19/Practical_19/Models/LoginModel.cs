using System.ComponentModel.DataAnnotations;

namespace Practical_19.Models
{
	public class LoginModel
	{
		[Required]
		[StringLength(50)]
		[EmailAddress]
		public string Email { get; set; }

		[Required]
		[StringLength(50, MinimumLength = 5)]
		[DataType(DataType.Password)]
		public string Password { get; set; }
	}
}
