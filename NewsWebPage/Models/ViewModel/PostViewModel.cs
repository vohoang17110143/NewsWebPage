using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsWebPage.Models.ViewModel
{
    public class PostViewModel
    {
        public Post Post { get; set; }

        public IEnumerable<Category> Categories { get; set; }

        public IEnumerable<SpecialTags> SpecialTags { get; set; }
    }
}