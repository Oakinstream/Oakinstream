using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Oakinstream.Models
{
    public class HomeImageMapping
    {
        public int ID { get; set; }
        public int HomeID { get; set; }
        public int HomeImageID { get; set; }
        public int ImageNumber { get; set; }
        public virtual Home Home { get; set; }
        public virtual HomeImage HomeImage { get; set; }
    }
}