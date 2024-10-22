using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Messenger.Models
{
	public class LoginViewModel
	{
		[Display(Name = "شماره تلفن :")]
		[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
		[MaxLength(11,ErrorMessage = "کارکتر وارد شده بیشتر از حد مجاز است")]
		public string PhoneNumber { get; set; }
		[Display(Name = "رمز عبور :")]
		[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
		[DataType(DataType.Password)]
		[MinLength(5,ErrorMessage = "{0} حداقل باید دارای 6 کارکتر باشد")]
		public string Password { get; set; }

		[Display(Name = "مرا به خاطر بسپار")]
		public bool RememberMe { get; set; }
    }
}