using System;
using System.ComponentModel.DataAnnotations;

namespace Dresses.Models
{
	public class LoginModel
	{
		[Required]
    	public string Username { get; set; }
		[Required]
		public string Password { get; set; }

		public string Token { get; set; }
		public  DateTime ExpireTime { get; set; }
	}
}