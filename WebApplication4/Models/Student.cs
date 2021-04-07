using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication4.Models
{
    public class Student
    {
       [Key]
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string FName { get; set; }
        [Required]
        public string Email { get; set; }
        
        public int DeptID { get; set; }

        public string Mobile { get; set; }

        public string Description { get; set; }

        
        [NotMapped]
        public string Department { get; set; }
       
    }
}
