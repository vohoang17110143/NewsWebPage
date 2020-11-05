using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsWebPage.Models.ViewModel
{
    public class ListPostViewModel
    {
        public List<Post> Post { get; set; }

        //Danh sách nội dung tìm kiếm
        public SelectList Category { get; set; }

        public SelectList SpecialTags { get; set; }

        //Từ khóa tìm kiếm

        public string postCate { get; set; }

        public string productSTag { get; set; }

        public string SearchString { get; set; }
    }
}