using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Oakinstream.Models
{
    public class AboutFileMapping
    {
        public int ID { get; set; }
        public int AboutID { get; set; }
        public int AboutFileID { get; set; }
        public int FileNumber { get; set; }

        public virtual About About { get; set; }
        public virtual AboutFile AboutFile { get; set; }
    }
}