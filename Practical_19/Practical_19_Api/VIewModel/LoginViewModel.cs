﻿using System.ComponentModel.DataAnnotations;

namespace Practical_19_Api.VIewModel
{
	public class LoginViewModel
	{
		[Required]
		[StringLength(50)]
		[EmailAddress]
		public string Email { get; set; }

		[Required]
		[StringLength(50, MinimumLength = 5)]
		public string Password { get; set; }
	}
}
