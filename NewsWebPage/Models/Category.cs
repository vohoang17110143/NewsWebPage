using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NewsWebPage.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Danh mục")]
        public string Name { get; set; }
    }
}