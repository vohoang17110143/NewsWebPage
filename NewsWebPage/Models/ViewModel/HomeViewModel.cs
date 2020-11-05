using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsWebPage.Models.ViewModel
{
    public class HomeViewModel
    {
        public List<Post> Posts { get; set; }
        public List<SpecialTags> SpecialTags { get; set; }
    }
}