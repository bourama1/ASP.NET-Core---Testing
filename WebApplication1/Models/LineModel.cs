﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class LineModel
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }

        public ICollection<RouteModel> Routes { get; set; }
    }
}
