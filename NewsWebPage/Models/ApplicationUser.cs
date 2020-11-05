using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NewsWebPage.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Display(Name = "Tên")]
        public string Name { get; set; }

        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }

        [Display(Name = "Ngày sinh")]
        public string DOB { get; set; }

        public string Role { get; set; }
        public bool isLockRole { get; set; }
    }
}