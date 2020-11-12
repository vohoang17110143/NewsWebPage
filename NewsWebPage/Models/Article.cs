using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NewsWebPage.Models
{
    public class Article
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Alias { get; set; }

        [StringLength(200)]
        public string Author { get; set; }

        public string Description { get; set; }

        public DateTime? DatePublished { get; set; }

        public int? CateId { get; set; }

        public string Link { get; set; }

        [ForeignKey("CateId")]
        public virtual Category Category { get; set; }
    }
}