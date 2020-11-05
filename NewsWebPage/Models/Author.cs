using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NewsWebPage.Models
{
    public class Author
    {
        public int Id { get; set; }

        [Display(Name = "Tên")]
        [Required(ErrorMessage = "Vui lòng nhập người viết !")]
        public string Name { get; set; }

        [Display(Name = "Địa chỉ")]
        [Required(ErrorMessage = "Vui lòng nhập địa chỉ người viết !")]
        public string Address { get; set; }

        [Display(Name = "Số điện thoại")]
        [Required(ErrorMessage = "Vui lòng nhập SĐT liên hệ của người viết!")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Mô tả")]
        [Required(ErrorMessage = "Hãy mô tả sơ về bản thân !")]
        public string Description { get; set; }
    }
}