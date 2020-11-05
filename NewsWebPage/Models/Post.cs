using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NewsWebPage.Models
{
    public class Post
    {
        public int Id { get; set; }

        [Display(Name = "Tiêu đề")]
        public string Name { get; set; }

        [Display(Name = "Lượt xem")]
        public int ViewCount { get; set; }

        [Display(Name = "Mô tả")]
        public string Description { set; get; }

        [Display(Name = "Nội dung")]
        [DataType(DataType.Text)]
        public string Content { get; set; }

        [Display(Name = "Ảnh đại diện")]
        public string Image { get; set; }

        [Display(Name = "Ngày đăng")]
        public DateTime DateCreated { get; set; }

        [Display(Name = "Danh mục")]
        public int CategoryID { get; set; }

        [ForeignKey("CategoryID")]
        public virtual Category Category { get; set; }

        [Display(Name = "Người viết")]
        public int AuthorID { get; set; }

        [ForeignKey("AuthorID")]
        public virtual Author Author { get; set; }

        [Display(Name = "Nhãn đặc biệt")]
        public int SpecialTagsID { get; set; }

        [ForeignKey("SpecialTagsID")]
        public virtual SpecialTags SpecialTags { get; set; }
    }
}