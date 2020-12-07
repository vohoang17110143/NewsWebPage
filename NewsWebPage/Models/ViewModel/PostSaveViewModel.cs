using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsWebPage.Models.ViewModel
{
    public class PostSaveViewModel
    {
        public List<Post> Posts { get; set; }

        public PostSave PostSave { get; set; }
        public ApplicationUser Reader { get; set; }
    }
}