using System;
using System.ComponentModel.DataAnnotations;

namespace Sample.Domains
{
    public class Employee
    {
        [Required]
        [Key()]
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public int Age { get; set; }

        [Required]
        public string Gender { get; set; }

        public string FullName => $"{FirstName} {LastName}";

    }
}
