﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Oakinstream.Models
{
    public class BlogImage
    {
        public int ID { get; set; }
        [Display(Name = "Image")]
        [StringLength(100)]
        [Index(IsUnique = true)]
        public string FileName { get; set; }
        public virtual ICollection<BlogImageMapping> BlogImageMappings { get; set; }
    }
}