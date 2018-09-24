using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Oakinstream.Models
{
    public class SearchItem
    {
        public int ItemID { get; set; }
        public string Type { get; set; }
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Display(Name = "Comment")]
        public string Comment { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}