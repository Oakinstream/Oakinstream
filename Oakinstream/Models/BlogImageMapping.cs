using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Oakinstream.Models
{
    public class BlogImageMapping
    {
        public int ID { get; set; }
        public int BlogID { get; set; }
        public int BlogImageID { get; set; }
        public int ImageNumber { get; set; }
        public virtual BlogModels BlogPost { get; set; }
        public virtual BlogImage BlogImage { get; set; }
    }
}