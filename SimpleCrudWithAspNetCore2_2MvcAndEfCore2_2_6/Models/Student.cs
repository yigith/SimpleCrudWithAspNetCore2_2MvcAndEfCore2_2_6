using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleCrudWithAspNetCore2_2MvcAndEfCore2_2_6.Models
{
    public class Student
    {
        public int Id { get; set; }

        [Required, Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required, Display(Name = "Last Name")]
        public string LastName { get; set; }
    }
}
