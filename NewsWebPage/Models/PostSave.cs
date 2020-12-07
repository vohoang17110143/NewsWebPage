using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NewsWebPage.Models
{
    public class PostSave
    {
        public int Id { get; set; }

        [Display(Name = "Ngày đọc")]
        public DateTime DateRead { get; set; }

        [Display(Name = "Người đọc")]
        public string ReaderID { get; set; }

        [ForeignKey("ReaderID")]
        public virtual IdentityUser Reader { get; set; }

        [Display(Name = "Bài viết đã lưu")]
        public int PostID { get; set; }

        [ForeignKey("PostID")]
        public virtual Post Posts { get; set; }
    }
}