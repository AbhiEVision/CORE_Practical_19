﻿using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Practical_19.Models
{
	public class RegisterModel
	{
		[Required]
		[StringLength(50)]
		public string FirstName { get; set; }

		[Required]
		[StringLength(50)]
		public string LastName { get; set; }

		[Required]
		[StringLength(50)]
		[EmailAddress]
		public string Email { get; set; }

		[Required]
		[StringLength(50, MinimumLength = 5)]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[Required]
		[DataType(DataType.Password)]
		public string ConfirmPassword { get; set; }

		[Required]
		//[JsonConverter(typeof(StringEnumConverter))]
		public string Roles { get; set; }

	}
}
