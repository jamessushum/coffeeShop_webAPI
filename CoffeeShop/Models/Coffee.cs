using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoffeeShop.Models
{
    public class Coffee
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public int BeanVarietyId { get; set; }

        public BeanVariety BeanVariety { get; set; }
    }
}
