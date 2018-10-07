using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Oakinstream.Models
{
    public class HomeImage
    {
        public int ID { get; set; }
        [Display(Name = "Image")]
        [StringLength(100)]
        [Index(IsUnique = true)]
        public string FileName { get; set; }
        public string ImageHeader { get; set; }
        public string ImageCaption { get; set; }
        public string ImageLink { get; set; }
        public virtual ICollection<HomeImageMapping> HomeImageMappings { get; set; }
    }
}