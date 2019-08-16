using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RoleProject.Models
{
    public class Car_properties
    {
        public Car_properties()
        {
            Car = new List<Car>();
        }
        
        [Key]
        public int id { get; set; }
        [Required]
        public string proprity_Name { get; set; }
        public List<Car> Car { get; set; }
    }
}