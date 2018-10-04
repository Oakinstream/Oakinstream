using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Oakinstream.Models
{
    public class AboutImage
    {
        public int ID { get; set; }
        [Display(Name = "File")]
        [StringLength(100)]
        [Index(IsUnique = true)]
        public string FileName { get; set; }
        public ICollection<About> About { get; set; }
    }
}