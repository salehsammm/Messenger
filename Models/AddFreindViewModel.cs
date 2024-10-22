using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Messenger.Models
{
	public class AddFreindViewModel
	{
		[Display(Name = "شماره تلفن :")]
		[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
		[MaxLength(11, ErrorMessage = "کارکتر وارد شده بیشتر از حد مجاز است")]
		[DataType(DataType.PhoneNumber)]
		public string PhoneNumber { get; set; }

		[Display(Name = "نام و نام خانوادگی :")]
		[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
		public string FullName { get; set; }
    }
}