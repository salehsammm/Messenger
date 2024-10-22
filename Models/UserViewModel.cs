using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Messenger.Models
{
	public class UserViewModel
	{
		[Display(Name = "نام :")]
		public string Fname { get; set; }
		[Display(Name = "نام خانوادگی :")]
		public string Lname { get; set; }
		//[Display(Name = "جنسیت :")]
		//[Required(ErrorMessage = "لطفا {0} خود را انتخاب کنید")]
		//public bool Gender { get; set; }
		//public IEnumerable<SelectListItem> GenderOptions { get; set; }
		[Display(Name = "شماره تلفن :")]
		[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
		[MaxLength(11, ErrorMessage = "کارکتر وارد شده بیشتر از حد مجاز است")]
		public string PhoneNumber { get; set; }
		[Display(Name = "نام کاربری :")]
		[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
		public string UserName { get; set; }
		[Display(Name = "رمز عبور :")]
		[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
		[DataType(DataType.Password)]
		[MinLength(5, ErrorMessage = "{0} حداقل باید دارای 6 کارکتر باشد")]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "کلمه‌های عبور با هم مطابقت ندارند")]
		[Display(Name = "تکرار کلمه عبور :")]
		[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
		public string RePassword { get; set; }
		public System.DateTime RegisterDate { get; set; }
	}
}