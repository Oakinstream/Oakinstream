using Oakinstream.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Oakinstream.ViewModels
{
    public class HomeViewModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string[] HomeImages { get; set; }
        public List<SelectList> ImageLists { get; set; }
        [DataType(DataType.MultilineText)]
        public string Info1 { get; set; }
        [DataType(DataType.MultilineText)]
        public string Info2 { get; set; }
        [DataType(DataType.MultilineText)]
        public string Info3 { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; } = "";
    }
}