using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Oakinstream.Models
{
    public class CheckingAccount
    {
        public int ID { get; set; }
        [Required]
        [StringLength(10)]
        [Column(TypeName = "varchar")]
        [RegularExpression(@"\d{6,10}", ErrorMessage = "Must be beetween 6 and 10 digits.")]
        [Display(Name = "Account")]
        public string AccountNumber { get; set; }
        [Required]
        [Display(Name = "First name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        public string Name
        {
            get { return string.Format("{0} {1}", this.FirstName, this.LastName); }
        }

        public virtual ApplicationUser User { get; set; }
        [Required]
        public string ApplicationUserId { get; set; }
    }
}