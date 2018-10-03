using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Oakinstream.Models
{
    public class Contact
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Please enter your name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please enter your name")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "The name must be between 3 and 100 characters in length")]
        public string Email { get; set; }
        public string Subject { get; set; }
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Please enter a message")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "The Message must be between 3 and 200 characters in length")]
        public string Message { get; set; }
    }
}