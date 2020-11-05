using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NewsWebPage.Models
{
    public class SpecialTags
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Tên nhãn đặc biệt")]
        public string Name { get; set; }
    }
}